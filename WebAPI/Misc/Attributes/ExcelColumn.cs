using System;

namespace WebAPI.Misc.Attributes
{
    public class ExcelColumn : Attribute
    {

        public string Name { private set; get; }

        public ExcelColumn(string name)
        {
            Name = name;
        }
        
    }
}