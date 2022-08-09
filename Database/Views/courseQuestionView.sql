drop view course_question_view ;
create view course_question_view as select 
course_id, question.id as "question_id", title, question."key", correct_answer, comment_image , comment_text, image , "options", infos 
from question join course_question on question_id = question.id
order by "index" ;