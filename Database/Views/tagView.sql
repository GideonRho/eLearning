drop view course_tag_view;
drop view question_tag_view;

create view course_tag_view as select
distinct tag
from course, unnest(tags) tag

create view question_tag_view as select
distinct tag
from question, unnest(tags) tag