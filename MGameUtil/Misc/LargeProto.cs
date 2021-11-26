using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameUtil
{
    public class LargeProto
    {
        public static string ToString(string data)
        {
            StringBuilder sb = new();
            string[] fields = data.Trim().Split(",");

            List<string> fieldArr = new();
            for (int i = 0; i < fields.Length; i++)
            {
                string field = fields[i].Trim();
                if (field.Length == 0) continue;
                if (field.Contains("="))
                {
                    fieldArr.Add(field);
                }
                else
                {
                    fieldArr[^1] = $"{fieldArr[^1]},{field}";
                }
            }

            for (int i = 0; i < fieldArr.Count; i++)
            {
                string line = fieldArr[i];
                string[] tmp1 = line.Split("=");
                string field = tmp1[0].Trim();
                string value = tmp1[1].Replace("[", "").Replace("]", "").Trim();
                sb.Append($"data.{field} = ");

                string[] values = value.Split(",");
                if (values.Length > 1)
                {
                    sb.Append("{ ");
                }
                for (int iValue = 0; iValue < values.Length; iValue++)
                {
                    string v = values[iValue].Trim();
                    if (v.Contains("."))
                    {
                        sb.Append("osle:unmarshal_float()");
                    }
                    else
                    {
                        sb.Append("osle:unmarshal_int()");
                    }
                    if (iValue < values.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                if (values.Length > 1)
                {
                    sb.Append("}");
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
