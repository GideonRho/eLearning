using System.Collections.Generic;
using Newtonsoft.Json;
using WebAPI.Models.API.Responses.Questions;
using WebAPI.Models.API.Responses.Texts;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Responses
{
    public class  CourseApi
    {
        
        /// <example>13</example>
        public int Id { get; set; }
        /// <example>true</example>
        public bool IsActive { get; set; }
        /// <example>Implikationen Erkennen</example>
        public string Name { set; get; }
        public ECategory Category { get; set; }
        public ECourseType Type { get; set; }

        /// <summary>
        /// Duration in minutes, that should be available when taking the course. 
        /// </summary>
        public int Duration { get; set; }
        public List<string> Images { get; set; }

        public ECourseMode Mode { get; set; }
        /// <summary>
        /// Only available if mode is randomized.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
        /// <summary>
        /// Only available if mode is randomized.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? QuestionAmount { get; set; }

        /// <example>8</example>
        /// <summary>
        /// Represents the duration one has to remember the given images in the MemoryPerformance category.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? MemoryDuration { set; get; }
        
        /// <example>30</example>
        /// <summary>
        /// Represents they delay one has between remembering the images and answering the questionnaire in the MemoryPerformance category.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? MemoryDelay { set; get; }
        
        public CourseApi()
        {
        }

        public CourseApi(Course dbCourse)
        {
            Id = dbCourse.Id;
            IsActive = dbCourse.IsActive;
            Name = dbCourse.Name;
            Category = dbCourse.Category;
            Type = dbCourse.Type;
            Mode = dbCourse.Mode;
            Images = dbCourse.Images ?? new List<string>();

            Duration = dbCourse.Duration;
            
            MemoryDuration = dbCourse.MemoryDuration;
            MemoryDelay = dbCourse.MemoryDelay;

            QuestionAmount = dbCourse.QuestionAmount;
            Tags = dbCourse.Tags ?? new List<string>();
        }
        
    }
}