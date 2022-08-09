using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using WebAPI.Misc.Attributes;
using WebAPI.Models.Excel;

namespace WebAPI.Misc
{
    public class ExcelWrapper<T> where T : ExcelRow, new()
    {

        private class Entry
        {
            public readonly FieldInfo fieldInfo;
            public readonly ExcelColumn attribute;
            public int? index;

            public Entry(FieldInfo fieldInfo, ExcelColumn attribute)
            {
                this.fieldInfo = fieldInfo;
                this.attribute = attribute;
            }

            public void Check(string name, int pIndex)
            {
                if (attribute.Name.ToLower().Trim().Replace(" ", "") == name.ToLower().Trim().Replace(" ", ""))
                {
                    index = pIndex;
                }
            }
            
        }

        private readonly List<Entry> _entries = new List<Entry>();
        
        public ExcelWrapper(IDataRecord headerRecord)
        {
            InitEntries();

            for (int i = 0; i < headerRecord.FieldCount; i++)
            {
                int index = i;
                string s = headerRecord.GetString(i);
                if (string.IsNullOrEmpty(s)) continue;
                _entries.ForEach(e => e.Check(s, index));
            }
            
        }

        public T Read(IDataRecord row)
        {
            T result = new T();

            foreach (var entry in _entries)
            {
                if (!entry.index.HasValue) continue;
                var t = row.GetFieldType(entry.index.Value);
                string s = null;
                if (t == typeof(string)) s = row.GetString(entry.index.Value);
                if (t == typeof(double)) s = row.GetDouble(entry.index.Value).ToString(CultureInfo.InvariantCulture);
                entry.fieldInfo.SetValue(result, s);
            }
            
            return result;
        }
        
        private void InitEntries()
        {
            Type type = typeof(T);
            
            foreach (var fieldInfo in type.GetFields())
            {
                ExcelColumn attribute = fieldInfo.GetCustomAttribute<ExcelColumn>();
                if (attribute == null) continue;
                _entries.Add(new Entry(fieldInfo, attribute));
            }
            
        }

    }
}