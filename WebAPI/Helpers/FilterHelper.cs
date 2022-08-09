using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.API.Enums;
using WebAPI.Models.API.Requests;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Helpers
{
    public static class FilterHelper
    {
        
        public static IQueryable<Question> Filter(this IQueryable<Question> self, QuestionFilter filter)
            => self.FilterCategory(filter)
                .FilterKeywords(filter);
        
        public static IQueryable<Course> Filter(this IQueryable<Course> self, CourseFilter filter)
            => self.FilterCategory(filter)
                .FilterType(filter)
                .FilterIsActive(filter)
                .FilterTags(filter)
                .FilterKeywords(filter);

        public static IQueryable<Text> Filter(this IQueryable<Text> self, TextFilter filter)
            => self.FilterKeywords(filter);
        
        public static IQueryable<Questionnaire> FilterCourseType(this IQueryable<Questionnaire> query, CourseTypeFilter filter)
        {
            if (filter != CourseTypeFilter.Any)
                return query.Where(q => q.Course.Type == (ECourseType) filter);
            return query;
        }
        
        private static IQueryable<Question> FilterCategory(this IQueryable<Question> self, QuestionFilter filter)
        {
            if (filter.Category.HasValue) 
                return self.Where(q => q.Category == filter.Category);
            return self;
        }
        
        public static IEnumerable<Question> FilterTags(this IEnumerable<Question> self, QuestionFilter filter)
        {
            if (filter.Tags != null)
                return self.Where(q => filter.Tags.All(tag => q.Tags.Contains(tag)));
            return self;
        }

        private static IQueryable<Question> FilterKeywords(this IQueryable<Question> self, QuestionFilter filter)
        {
            if (filter.TitleKeywords == null) return self;
            return filter.TitleKeywords
                .Aggregate(self, (current, s) => 
                    current.Where(q => 
                        EF.Functions.ILike(q.Title, $"%{s}%")));
        }
        
        private static IQueryable<Course> FilterCategory(this IQueryable<Course> self, CourseFilter filter)
        {
            if (filter.Category.HasValue) 
                return self.Where(c => c.Category == filter.Category);
            return self;
        }
        
        private static IQueryable<Course> FilterType(this IQueryable<Course> self, CourseFilter filter)
        {
            if (filter.Type.HasValue) 
                return self.Where(c => c.Type == filter.Type);
            return self;
        }
        
        private static IQueryable<Course> FilterTags(this IQueryable<Course> self, CourseFilter filter)
        {
            if (filter.Tags != null)
                return self.Where(c => filter.Tags.All(tag => c.Tags.Contains(tag)));
            return self;
        }
        
        private static IQueryable<Course> FilterIsActive(this IQueryable<Course> self, CourseFilter filter)
        {
            if (filter.IsActive.HasValue)
                return self.Where(c => c.IsActive == filter.IsActive);
            return self;
        }
        
        private static IQueryable<Course> FilterKeywords(this IQueryable<Course> self, CourseFilter filter)
        {
            if (filter.TitleKeywords == null) return self;
            return filter.TitleKeywords
                .Aggregate(self, (current, s) => 
                    current.Where(c => 
                        EF.Functions.ILike(c.Name, $"%{s}%")));
        }
        
        private static IQueryable<Text> FilterKeywords(this IQueryable<Text> self, TextFilter filter)
        {
            if (filter.TitleKeywords == null) return self;
            return filter.TitleKeywords
                .Aggregate(self, (current, s) => 
                    current.Where(t => 
                        EF.Functions.ILike(t.Title, $"%{s}%")));
        }

    }
}