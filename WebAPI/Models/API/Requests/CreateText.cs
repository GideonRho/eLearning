using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebAPI.Models.API.Requests
{
    public class CreateText
    {
        
        [JsonRequired]
        public string Title { get; set; }
        [JsonRequired]
        public string Content { get; set; }
        [JsonRequired]
        public List<int> QuestionIds { get; set; }
        public string ReferenceKey { get; set; }
        
        public CreateText()
        {
        }

        public CreateText(string title, string content, List<int> questionIds, string referenceKey)
        {
            Title = title;
            Content = content;
            QuestionIds = questionIds;
            ReferenceKey = referenceKey;
        }
    }
}