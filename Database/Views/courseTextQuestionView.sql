drop view course_text_question_view ;
create view course_text_question_view as select 
course_id, question.id as "question_id", text_question.text_id, title, question."key", correct_answer, comment_image , comment_text, image , "options" , infos 
from question 
join text_question on text_question.question_id = question.id 
join course_text on course_text.text_id = text_question.text_id
order by course_text ."index" , text_question ."index" ;