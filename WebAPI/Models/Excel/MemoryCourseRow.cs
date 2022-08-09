using System.Collections.Generic;
using System.Linq;
using WebAPI.Misc.Attributes;
using WebAPI.Models.Database;
using WebAPI.Services;

namespace WebAPI.Models.Excel
{
    public class MemoryCourseRow : QuestionRow
    {

        [ExcelColumn("name")] public string name;
        [ExcelColumn("dauer")] public string duration;
        [ExcelColumn("zwischenzeit")] public string delay;
        [ExcelColumn("merkzeit")] public string memory_duration;

        public MemoryCourseRow()
        {
        }

        public override bool IsValid()
        {
            if (!string.IsNullOrEmpty(title)) return true;
            if (!string.IsNullOrEmpty(name)) return true;
            return false;
        }

        public int Delay => int.Parse(delay);
        public int Duration => int.Parse(duration);
        public int MemoryDuration => int.Parse(memory_duration);

    }
}