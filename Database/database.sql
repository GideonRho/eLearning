--
-- PostgreSQL database dump
--

-- Dumped from database version 12.7 (Ubuntu 12.7-1.pgdg20.10+1)
-- Dumped by pg_dump version 12.7 (Ubuntu 12.7-1.pgdg20.10+1)

-- Started on 2021-06-06 11:38:20 CEST

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA public;


--
-- TOC entry 3177 (class 0 OID 0)
-- Dependencies: 3
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 202 (class 1259 OID 246666)
-- Name: answer_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.answer_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 203 (class 1259 OID 246668)
-- Name: answer; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.answer (
    id integer DEFAULT nextval('public.answer_seq'::regclass) NOT NULL,
    question_id integer NOT NULL,
    questionnaire_id integer NOT NULL,
    choice integer NOT NULL,
    index integer NOT NULL
);


--
-- TOC entry 204 (class 1259 OID 246672)
-- Name: course_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.course_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 205 (class 1259 OID 246674)
-- Name: course; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.course (
    id integer DEFAULT nextval('public.course_seq'::regclass) NOT NULL,
    name text NOT NULL,
    category integer NOT NULL,
    active boolean NOT NULL,
    is_deleted boolean,
    type integer NOT NULL,
    mode integer,
    duration integer,
    images text[],
    tags text[],
    memory_delay integer,
    memory_duration integer,
    question_amount integer
);


--
-- TOC entry 206 (class 1259 OID 246687)
-- Name: course_question_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.course_question_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 207 (class 1259 OID 246689)
-- Name: course_question; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.course_question (
    id integer DEFAULT nextval('public.course_question_seq'::regclass) NOT NULL,
    course_id integer NOT NULL,
    question_id integer NOT NULL,
    index integer NOT NULL
);


--
-- TOC entry 211 (class 1259 OID 246738)
-- Name: question_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.question_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 212 (class 1259 OID 246740)
-- Name: question; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.question (
    id integer DEFAULT nextval('public.question_seq'::regclass) NOT NULL,
    title text NOT NULL,
    category integer NOT NULL,
    correct_answer integer NOT NULL,
    comment_text text,
    is_deleted boolean NOT NULL,
    image text,
    comment_image text,
    tags text[],
    options text[] NOT NULL,
    infos text[],
    text_id integer,
    key text
);


--
-- TOC entry 228 (class 1259 OID 254418)
-- Name: course_question_bytag_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.course_question_bytag_view AS
 SELECT c.id AS course_id,
    question.id AS question_id,
    question.title,
    question.key,
    question.correct_answer,
    question.comment_image,
    question.comment_text,
    question.image,
    question.options,
    question.infos
   FROM (public.question
     JOIN public.course c ON ((c.tags && question.tags)));


--
-- TOC entry 229 (class 1259 OID 254423)
-- Name: course_question_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.course_question_view AS
 SELECT course_question.course_id,
    question.id AS question_id,
    question.title,
    question.key,
    question.correct_answer,
    question.comment_image,
    question.comment_text,
    question.image,
    question.options,
    question.infos
   FROM (public.question
     JOIN public.course_question ON ((course_question.question_id = question.id)))
  ORDER BY course_question.index;


--
-- TOC entry 223 (class 1259 OID 250616)
-- Name: course_tag_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.course_tag_view AS
 SELECT DISTINCT tag.tag
   FROM public.course,
    LATERAL unnest(course.tags) tag(tag);


--
-- TOC entry 208 (class 1259 OID 246693)
-- Name: course_text_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.course_text_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 209 (class 1259 OID 246695)
-- Name: course_text; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.course_text (
    id integer DEFAULT nextval('public.course_text_seq'::regclass) NOT NULL,
    course_id integer NOT NULL,
    text_id integer NOT NULL,
    index integer NOT NULL
);


--
-- TOC entry 218 (class 1259 OID 246823)
-- Name: text_question_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.text_question_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 219 (class 1259 OID 246825)
-- Name: text_question; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.text_question (
    id integer DEFAULT nextval('public.text_question_seq'::regclass) NOT NULL,
    text_id integer NOT NULL,
    question_id integer NOT NULL,
    index integer NOT NULL
);


--
-- TOC entry 230 (class 1259 OID 254427)
-- Name: course_text_question_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.course_text_question_view AS
 SELECT course_text.course_id,
    question.id AS question_id,
    text_question.text_id,
    question.title,
    question.key,
    question.correct_answer,
    question.comment_image,
    question.comment_text,
    question.image,
    question.options,
    question.infos
   FROM ((public.question
     JOIN public.text_question ON ((text_question.question_id = question.id)))
     JOIN public.course_text ON ((course_text.text_id = text_question.text_id)))
  ORDER BY course_text.index, text_question.index;


--
-- TOC entry 216 (class 1259 OID 246814)
-- Name: text_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.text_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 217 (class 1259 OID 246816)
-- Name: text; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.text (
    id integer DEFAULT nextval('public.text_seq'::regclass) NOT NULL,
    content text NOT NULL,
    title text NOT NULL,
    is_deleted boolean NOT NULL
);


--
-- TOC entry 222 (class 1259 OID 250389)
-- Name: course_text_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.course_text_view AS
 SELECT course_text.course_id,
    course_text.text_id,
    text.title,
    text.content
   FROM (public.text
     JOIN public.course_text ON ((text.id = course_text.text_id)));


--
-- TOC entry 210 (class 1259 OID 246732)
-- Name: product_key; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.product_key (
    key text NOT NULL,
    type integer NOT NULL,
    user_id integer,
    activation_date timestamp(0) without time zone,
    expiration_date timestamp(0) without time zone,
    duration integer
);


--
-- TOC entry 227 (class 1259 OID 254390)
-- Name: progress_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.progress_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 226 (class 1259 OID 254375)
-- Name: progress; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.progress (
    id integer DEFAULT nextval('public.progress_seq'::regclass) NOT NULL,
    user_id integer NOT NULL,
    question_id integer NOT NULL,
    training integer,
    simulation integer
);


--
-- TOC entry 224 (class 1259 OID 250620)
-- Name: question_tag_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.question_tag_view AS
 SELECT DISTINCT tag.tag
   FROM public.question,
    LATERAL unnest(question.tags) tag(tag);


--
-- TOC entry 213 (class 1259 OID 246780)
-- Name: questionnaire_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.questionnaire_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 214 (class 1259 OID 246782)
-- Name: questionnaire; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.questionnaire (
    id integer DEFAULT nextval('public.questionnaire_seq'::regclass) NOT NULL,
    user_id integer NOT NULL,
    course_id integer NOT NULL,
    "timestamp" timestamp without time zone DEFAULT now() NOT NULL,
    status integer NOT NULL,
    state_timestamp timestamp without time zone NOT NULL,
    completion_time integer,
    correct_answers integer,
    wrong_answers integer
);


--
-- TOC entry 225 (class 1259 OID 254362)
-- Name: questionnaire_answer_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.questionnaire_answer_view AS
 SELECT a.questionnaire_id,
    a.id AS answer_id,
    a.choice,
    q.id AS question_id,
    q.title,
    q.correct_answer,
    q.comment_image,
    q.comment_text,
    q.image,
    q.options,
    q.infos,
    q.text_id
   FROM ((public.answer a
     JOIN public.questionnaire ON ((a.questionnaire_id = questionnaire.id)))
     JOIN public.question q ON ((q.id = a.question_id)))
  ORDER BY a.index;


--
-- TOC entry 232 (class 1259 OID 254436)
-- Name: questionnaire_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.questionnaire_view AS
 SELECT questionnaire.id,
    questionnaire.user_id,
    questionnaire.course_id,
    questionnaire."timestamp",
    questionnaire.status,
    questionnaire.state_timestamp,
    questionnaire.completion_time,
    questionnaire.correct_answers,
    questionnaire.wrong_answers,
    course.category
   FROM (public.questionnaire
     JOIN public.course ON ((questionnaire.course_id = course.id)));


--
-- TOC entry 234 (class 1259 OID 254445)
-- Name: ranking_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.ranking_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 233 (class 1259 OID 254440)
-- Name: ranking; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.ranking (
    id integer DEFAULT nextval('public.ranking_seq'::regclass) NOT NULL,
    course_id integer NOT NULL,
    points integer NOT NULL,
    percentile integer NOT NULL
);


--
-- TOC entry 215 (class 1259 OID 246793)
-- Name: session; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.session (
    user_id integer NOT NULL,
    "timestamp" date NOT NULL,
    id text NOT NULL
);


--
-- TOC entry 231 (class 1259 OID 254432)
-- Name: text_question_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.text_question_view AS
 SELECT question.id AS question_id,
    text_question.text_id,
    question.title,
    question.key,
    question.correct_answer,
    question.comment_image,
    question.comment_text,
    question.image,
    question.options,
    question.infos
   FROM (public.question
     JOIN public.text_question ON ((text_question.question_id = question.id)))
  ORDER BY text_question.index;


--
-- TOC entry 220 (class 1259 OID 246829)
-- Name: user_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.user_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 221 (class 1259 OID 246831)
-- Name: user; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."user" (
    id integer DEFAULT nextval('public.user_seq'::regclass) NOT NULL,
    name text NOT NULL,
    password text NOT NULL,
    salt bytea NOT NULL,
    role integer NOT NULL,
    questionnaire_id integer,
    email text,
    verification_code text,
    email_confirmed boolean DEFAULT false NOT NULL
);


--
-- TOC entry 3149 (class 0 OID 246668)
-- Dependencies: 203
-- Data for Name: answer; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3151 (class 0 OID 246674)
-- Dependencies: 205
-- Data for Name: course; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3153 (class 0 OID 246689)
-- Dependencies: 207
-- Data for Name: course_question; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3155 (class 0 OID 246695)
-- Dependencies: 209
-- Data for Name: course_text; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3156 (class 0 OID 246732)
-- Dependencies: 210
-- Data for Name: product_key; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3168 (class 0 OID 254375)
-- Dependencies: 226
-- Data for Name: progress; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3158 (class 0 OID 246740)
-- Dependencies: 212
-- Data for Name: question; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3160 (class 0 OID 246782)
-- Dependencies: 214
-- Data for Name: questionnaire; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3170 (class 0 OID 254440)
-- Dependencies: 233
-- Data for Name: ranking; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3161 (class 0 OID 246793)
-- Dependencies: 215
-- Data for Name: session; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3163 (class 0 OID 246816)
-- Dependencies: 217
-- Data for Name: text; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3165 (class 0 OID 246825)
-- Dependencies: 219
-- Data for Name: text_question; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3167 (class 0 OID 246831)
-- Dependencies: 221
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3178 (class 0 OID 0)
-- Dependencies: 202
-- Name: answer_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.answer_seq', 1000, true);


--
-- TOC entry 3179 (class 0 OID 0)
-- Dependencies: 206
-- Name: course_question_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.course_question_seq', 1000, true);


--
-- TOC entry 3180 (class 0 OID 0)
-- Dependencies: 204
-- Name: course_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.course_seq', 1000, true);


--
-- TOC entry 3181 (class 0 OID 0)
-- Dependencies: 208
-- Name: course_text_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.course_text_seq', 1000, false);


--
-- TOC entry 3182 (class 0 OID 0)
-- Dependencies: 227
-- Name: progress_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.progress_seq', 1, false);


--
-- TOC entry 3183 (class 0 OID 0)
-- Dependencies: 211
-- Name: question_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.question_seq', 1000, true);


--
-- TOC entry 3184 (class 0 OID 0)
-- Dependencies: 213
-- Name: questionnaire_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.questionnaire_seq', 1000, true);


--
-- TOC entry 3185 (class 0 OID 0)
-- Dependencies: 234
-- Name: ranking_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.ranking_seq', 1, false);


--
-- TOC entry 3186 (class 0 OID 0)
-- Dependencies: 218
-- Name: text_question_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.text_question_seq', 1000, false);


--
-- TOC entry 3187 (class 0 OID 0)
-- Dependencies: 216
-- Name: text_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.text_seq', 1000, false);


--
-- TOC entry 3188 (class 0 OID 0)
-- Dependencies: 220
-- Name: user_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.user_seq', 1000, true);


--
-- TOC entry 2977 (class 2606 OID 246839)
-- Name: questionnaire ResultSheet_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.questionnaire
    ADD CONSTRAINT "ResultSheet_pkey" PRIMARY KEY (id);


--
-- TOC entry 2956 (class 2606 OID 246841)
-- Name: answer answer_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.answer
    ADD CONSTRAINT answer_pkey PRIMARY KEY (id);


--
-- TOC entry 2960 (class 2606 OID 246845)
-- Name: course course_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course
    ADD CONSTRAINT course_pkey PRIMARY KEY (id);


--
-- TOC entry 2964 (class 2606 OID 246847)
-- Name: course_question course_question_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course_question
    ADD CONSTRAINT course_question_pkey PRIMARY KEY (id);


--
-- TOC entry 2968 (class 2606 OID 246849)
-- Name: course_text course_text_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course_text
    ADD CONSTRAINT course_text_pkey PRIMARY KEY (id);


--
-- TOC entry 2962 (class 2606 OID 247087)
-- Name: course course_un_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course
    ADD CONSTRAINT course_un_name UNIQUE (name);


--
-- TOC entry 2973 (class 2606 OID 246859)
-- Name: product_key product_keys_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.product_key
    ADD CONSTRAINT product_keys_pkey PRIMARY KEY (key);


--
-- TOC entry 2995 (class 2606 OID 254379)
-- Name: progress progress_pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.progress
    ADD CONSTRAINT progress_pk PRIMARY KEY (id);


--
-- TOC entry 2975 (class 2606 OID 246865)
-- Name: question question_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.question
    ADD CONSTRAINT question_pkey PRIMARY KEY (id);


--
-- TOC entry 2997 (class 2606 OID 254444)
-- Name: ranking ranking_pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ranking
    ADD CONSTRAINT ranking_pk PRIMARY KEY (id);


--
-- TOC entry 2982 (class 2606 OID 246875)
-- Name: session session_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.session
    ADD CONSTRAINT session_pkey PRIMARY KEY (id);


--
-- TOC entry 2984 (class 2606 OID 246881)
-- Name: text text_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.text
    ADD CONSTRAINT text_pkey PRIMARY KEY (id);


--
-- TOC entry 2990 (class 2606 OID 246883)
-- Name: text_question text_question_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.text_question
    ADD CONSTRAINT text_question_pkey PRIMARY KEY (id);


--
-- TOC entry 2986 (class 2606 OID 247089)
-- Name: text text_un; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.text
    ADD CONSTRAINT text_un UNIQUE (title);


--
-- TOC entry 2993 (class 2606 OID 246885)
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 2957 (class 1259 OID 246886)
-- Name: fki_fk_answer_question; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_answer_question ON public.answer USING btree (question_id);


--
-- TOC entry 2958 (class 1259 OID 246887)
-- Name: fki_fk_answer_questionnaire; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_answer_questionnaire ON public.answer USING btree (questionnaire_id);


--
-- TOC entry 2965 (class 1259 OID 246892)
-- Name: fki_fk_course_question_course; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_course_question_course ON public.course_question USING btree (course_id);


--
-- TOC entry 2966 (class 1259 OID 246893)
-- Name: fki_fk_course_question_question; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_course_question_question ON public.course_question USING btree (question_id);


--
-- TOC entry 2969 (class 1259 OID 246894)
-- Name: fki_fk_course_text_course; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_course_text_course ON public.course_text USING btree (course_id);


--
-- TOC entry 2970 (class 1259 OID 246895)
-- Name: fki_fk_course_text_text; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_course_text_text ON public.course_text USING btree (text_id);


--
-- TOC entry 2971 (class 1259 OID 246896)
-- Name: fki_fk_key_user; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_key_user ON public.product_key USING btree (user_id);


--
-- TOC entry 2978 (class 1259 OID 246910)
-- Name: fki_fk_questionnaire_course; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_questionnaire_course ON public.questionnaire USING btree (course_id);


--
-- TOC entry 2979 (class 1259 OID 246911)
-- Name: fki_fk_questionnaire_user; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_questionnaire_user ON public.questionnaire USING btree (user_id);


--
-- TOC entry 2980 (class 1259 OID 246912)
-- Name: fki_fk_session_user; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_session_user ON public.session USING btree (user_id);


--
-- TOC entry 2987 (class 1259 OID 246913)
-- Name: fki_fk_text_question_question; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_text_question_question ON public.text_question USING btree (question_id);


--
-- TOC entry 2988 (class 1259 OID 246914)
-- Name: fki_fk_text_question_text; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_text_question_text ON public.text_question USING btree (text_id);


--
-- TOC entry 2991 (class 1259 OID 246915)
-- Name: fki_fk_user_questionnaire; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_fk_user_questionnaire ON public."user" USING btree (questionnaire_id);


--
-- TOC entry 2998 (class 2606 OID 246920)
-- Name: answer fk_answer_question; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.answer
    ADD CONSTRAINT fk_answer_question FOREIGN KEY (question_id) REFERENCES public.question(id) NOT VALID;


--
-- TOC entry 2999 (class 2606 OID 246925)
-- Name: answer fk_answer_questionnaire; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.answer
    ADD CONSTRAINT fk_answer_questionnaire FOREIGN KEY (questionnaire_id) REFERENCES public.questionnaire(id) NOT VALID;


--
-- TOC entry 3000 (class 2606 OID 246950)
-- Name: course_question fk_course_question_course; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course_question
    ADD CONSTRAINT fk_course_question_course FOREIGN KEY (course_id) REFERENCES public.course(id) NOT VALID;


--
-- TOC entry 3001 (class 2606 OID 246955)
-- Name: course_question fk_course_question_question; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course_question
    ADD CONSTRAINT fk_course_question_question FOREIGN KEY (question_id) REFERENCES public.question(id) NOT VALID;


--
-- TOC entry 3002 (class 2606 OID 246960)
-- Name: course_text fk_course_text_course; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course_text
    ADD CONSTRAINT fk_course_text_course FOREIGN KEY (course_id) REFERENCES public.course(id) NOT VALID;


--
-- TOC entry 3003 (class 2606 OID 246965)
-- Name: course_text fk_course_text_text; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.course_text
    ADD CONSTRAINT fk_course_text_text FOREIGN KEY (text_id) REFERENCES public.text(id) NOT VALID;


--
-- TOC entry 3004 (class 2606 OID 246970)
-- Name: product_key fk_key_user; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.product_key
    ADD CONSTRAINT fk_key_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 3005 (class 2606 OID 247045)
-- Name: questionnaire fk_questionnaire_course; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.questionnaire
    ADD CONSTRAINT fk_questionnaire_course FOREIGN KEY (course_id) REFERENCES public.course(id) NOT VALID;


--
-- TOC entry 3006 (class 2606 OID 247050)
-- Name: questionnaire fk_questionnaire_user; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.questionnaire
    ADD CONSTRAINT fk_questionnaire_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 3007 (class 2606 OID 247055)
-- Name: session fk_session_user; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.session
    ADD CONSTRAINT fk_session_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 3008 (class 2606 OID 247060)
-- Name: text_question fk_text_question_question; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.text_question
    ADD CONSTRAINT fk_text_question_question FOREIGN KEY (question_id) REFERENCES public.question(id) NOT VALID;


--
-- TOC entry 3009 (class 2606 OID 247065)
-- Name: text_question fk_text_question_text; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.text_question
    ADD CONSTRAINT fk_text_question_text FOREIGN KEY (text_id) REFERENCES public.text(id) NOT VALID;


--
-- TOC entry 3010 (class 2606 OID 247070)
-- Name: user fk_user_questionnaire; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT fk_user_questionnaire FOREIGN KEY (questionnaire_id) REFERENCES public.questionnaire(id) NOT VALID;


--
-- TOC entry 3011 (class 2606 OID 254380)
-- Name: progress progress_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.progress
    ADD CONSTRAINT progress_fk FOREIGN KEY (user_id) REFERENCES public."user"(id);


--
-- TOC entry 3012 (class 2606 OID 254385)
-- Name: progress progress_fk_1; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.progress
    ADD CONSTRAINT progress_fk_1 FOREIGN KEY (question_id) REFERENCES public.question(id);


-- Completed on 2021-06-06 11:38:20 CEST

--
-- PostgreSQL database dump complete
--

