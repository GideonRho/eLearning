using WebAPI.Misc.Attributes;

namespace WebAPI.Models.Excel
{
    public class TextRow : QuestionRow
    {

        [ExcelColumn("titel")] public string textTitle;
        [ExcelColumn("inhalt")] public string textContent;

        public TextRow()
        {
        }

        public override bool IsValid()
        {
            if (!string.IsNullOrEmpty(title)) return true;
            if (!string.IsNullOrEmpty(textTitle)) return true;
            return false;
        }
        
    }
}