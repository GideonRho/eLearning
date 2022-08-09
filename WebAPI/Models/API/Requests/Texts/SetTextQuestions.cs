using System;
using System.Collections.Generic;

namespace WebAPI.Models.API.Requests.Texts
{
    public class SetTextQuestions
    {
        
        public List<int> QuestionIds { get; set; }

        public int OrderIndex(int questionId)
        {
            for (int i = 0; i < QuestionIds.Count; i++)
                if (QuestionIds[i] == questionId) return i;
            
            throw new IndexOutOfRangeException();
        }
        
    }
}