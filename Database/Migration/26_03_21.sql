 DROP TABLE public.product_key;
 
 CREATE TABLE public.product_key (
    key text NOT NULL,
    type integer NOT NULL,
    user_id integer,
    activation_date timestamp(0) without time zone,
    expiration_date timestamp(0) without time zone,
    duration integer
);