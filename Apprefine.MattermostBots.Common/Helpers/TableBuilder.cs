using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apprefine.MattermostBots.Common.Helpers
{
    public class TableBuilder
    {
        private Dictionary<string, List<string>> _columns 
            = new Dictionary<string, List<string>>();

        public TableBuilder AddColumn(string name, string value)
        {
            if (!_columns.ContainsKey(name))
                _columns[name] = new List<string>();

            //escape illegal characters (those which would break the table)
            _columns[name].Add(value.Replace("|", "\\|"));
            return this;
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            //header
            str.Append("|");
            foreach(var col in _columns.Keys)
            {
                str.Append(col);
                str.Append("|");
            }
            str.AppendLine();

            //header separator (text align config)
            str.Append("|");
            foreach (var col in _columns.Keys)
            {
                //let's align the text to the left
                //while keeping a straight face
                str.Append(":-|");
            }
            str.AppendLine();

            //body
            int maxRow = _columns.Values.Max(x => x.Count) - 1;
            for(int row = 0; row <= maxRow; row++)
            {
                str.Append("|");
                foreach(var col in _columns.Values)
                {
                    //some columns can be shorter
                    if(col.Count -1 <= row) 
                    {
                        str.Append(col[row]);
                    }
                    str.Append("|");
                }
                str.AppendLine();
            }


            return str.ToString();
        }
    }
}
