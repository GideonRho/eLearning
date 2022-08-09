drop view course_question_bytag_view ;
create view course_question_bytag_view as select
c.id as "course_id", question.id as "question_id", title , question."key", correct_answer, comment_image , comment_text, image , "options" , infos 
from question
join course as "c" on c.tags && question.tags;