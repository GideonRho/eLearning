drop view course_text_view ;
create view course_text_view as select 
course_id, text_id, title, "content"
from text
join course_text on text.id = course_text.text_id;