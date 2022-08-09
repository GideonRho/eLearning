drop view text_question_view ;
create view text_question_view as select 
question.id as "question_id", text_question.text_id, title, question."key", correct_answer, comment_image , comment_text, image , "options" , infos 
from question 
join text_question on text_question.question_id = question.id 
order by text_question ."index" ;