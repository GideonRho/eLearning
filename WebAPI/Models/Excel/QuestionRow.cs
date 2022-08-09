using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebAPI.Misc.Attributes;

namespace WebAPI.Models.Excel
{
    public class QuestionRow : ExcelRow
    {
        [ExcelColumn("frage")] public string title;
        [ExcelColumn("id")] public string key;
        [ExcelColumn("a")] public string answerA;
        [ExcelColumn("b")] public string answerB;
        [ExcelColumn("c")] public string answerC;
        [ExcelColumn("d")] public string answerD;
        [ExcelColumn("e")] public string answerE;
        [ExcelColumn("antwort")] public string correctAnswer;
        [ExcelColumn("kommentar")] public string comment;
        [ExcelColumn("kommentarbild")] public string commentImage;
        [ExcelColumn("bild")] public string image;
        [ExcelColumn("tags")] public string tags;
        
        [ExcelColumn("info1")] public string info1;
        [ExcelColumn("info2")] public string info2;
        [ExcelColumn("info3")] public string info3;
        [ExcelColumn("info4")] public string info4;
        [ExcelColumn("info5")] public string info5;

        public QuestionRow()
        {
        }

        public int CorrectAnswerIndex()
        {
            if (correctAnswer.Length != 1) 
                throw new ValidationException($"QuestionRow CorrectAnswer has invalid length! Length is {correctAnswer.Length}.");
            int i = correctAnswer.ToUpper()[0] - 65;
            if (i < 0 || i >= 5)
                throw new ValidationException($"QuestionRow CorrectAnswer has invalid char! Char is {correctAnswer.ToUpper()[0]}");
            return i;
        }
        
        public IEnumerable<string> Answers => new List<string>{answerA, answerB, answerC, answerD, answerE};
        public IEnumerable<string> Infos => new List<string>{info1, info2, info3, info4, info5}
            .Where(s => !string.IsNullOrEmpty(s));
        public IEnumerable<string> Tags => tags != null ? tags.Split(',').Select(s => s.Trim()) : Enumerable.Empty<string>();

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(title);
        }
        
    }
}