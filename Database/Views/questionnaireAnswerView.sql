drop view questionnaire_answer_view ;
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