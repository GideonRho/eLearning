using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAPI.Misc.Attributes;

namespace WebAPI.Models.Excel
{
    public class FullCourseRow : ExcelRow
    {

        [ExcelColumn("Name")] public string name;
        [ExcelColumn("merkzeit")] public string memoryDuration;
        [ExcelColumn("zwischenzeit")] public string memoryDelay;
        [ExcelColumn("dauer")] public string duration;
        [ExcelColumn("frage")] public string question;
        [ExcelColumn("a")] public string answerA;
        [ExcelColumn("b")] public string answerB;
        [ExcelColumn("c")] public string answerC;
        [ExcelColumn("d")] public string answerD;
        [ExcelColumn("e")] public string answerE;
        [ExcelColumn("antwort")] public string correctAnswer;
        [ExcelColumn("kommentar")] public string comment;
        [ExcelColumn("tags")] public string tags;
        [ExcelColumn("id")] public string referenceKey;

        public FullCourseRow()
        {
        }

        public int CorrectAnswerIndex()
        {
            if (correctAnswer.Length != 1) 
                throw new ValidationException($"CorrectAnswer has invalid length! Length is {correctAnswer.Length}.");
            int i = correctAnswer.ToUpper()[0] - 65;
            if (i < 0 || i >= 5)
                throw new ValidationException($"CorrectAnswer has invalid char! Char is {correctAnswer.ToUpper()[0]}");
            return i;
        }
        
        public IEnumerable<string> Answers => new List<string>{answerA, answerB, answerC, answerD, answerE};
        public int MemoryDuration => int.Parse(memoryDuration);
        public int MemoryDelay => int.Parse(memoryDelay);

        public int? Duration()
        {
            if (string.IsNullOrEmpty(duration)) return null;
            return int.Parse(duration);
        }
        
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(name);
        }
        
    }
}