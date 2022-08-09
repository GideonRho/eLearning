drop view questionnaire_view ;
create view questionnaire_view as select 
questionnaire.id, questionnaire.user_id, questionnaire.course_id, "timestamp", status, state_timestamp, completion_time, correct_answers, wrong_answers, course.category
from questionnaire
join course on questionnaire.course_id = course.id 