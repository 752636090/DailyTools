using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public static class StringUtil
    {
        #region IsNullOrEmpty 验证值是否为null

        /// <summary>
        /// 判断对象是否为Null、DBNull、Empty或空白字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string value)
        {
            bool retVal = false;
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                retVal = true;
            }
            return retVal;
        }

        #endregion

        /// <summary>
        /// 把string类型转换成int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(string str)
        {
            _ = int.TryParse(str, out int temp);
            return temp;
        }

        /// <summary>
        /// 把string类型转换成long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToLong(string str)
        {
            _ = long.TryParse(str, out long temp);
            return temp;
        }

        /// <summary>
        /// 把string类型转换成float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(string str)
        {
            _ = float.TryParse(str, out float temp);
            return temp;
        }

        public static void DeepReplace<T>(T obj, string srcStr, string targetStr)
        {
            //如果是值类型则直接返回
            if (obj.GetType().IsValueType) return;

            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    string oldStr = (string)field.GetValue(obj);
                    field.SetValue(obj, oldStr.Replace(srcStr, targetStr));
                }
                else
                {
                    DeepReplace(field.GetValue(obj), srcStr, targetStr);
                }
            }
        }

        public static string ReplaceFirst(this string s, string oldValue, string newValue)
        {
            int index = s.IndexOf(oldValue);
            return s.Remove(index, oldValue.Length).Insert(index, newValue);
        }
    }
}
