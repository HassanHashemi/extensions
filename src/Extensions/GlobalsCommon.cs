using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Base
{
    public class GlobalsCommon
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        static public string AS(object text1, object text2)
        {
            return new StringBuilder().Append(text1).Append(text2).ToString();
        }

        static public string AS(object text1, object text2, object text3)
        {
            return new StringBuilder().AppendFormat("{0}{1}{2}", text1, text2, text3).ToString();
        }

        static public string AS(object text1, object text2, object text3, object text4)
        {
            return new StringBuilder().AppendFormat("{0}{1}{2}{3}", text1, text2, text3, text4).ToString();
        }

        static public string AS(object text1, object text2, object text3, object text4, object text5)
        {
            return new StringBuilder().AppendFormat("{0}{1}{2}{3}{4}", text1, text2, text3, text4, text5).ToString();
        }

        static public string AS(object text1, object text2, object text3, object text4, object text5, object text6 = null, object text7 = null, object text8 = null, object text9 = null, object text10 = null)
        {
            return AS(AS(text1, text2, text3, text4, text5), AS(text6, text7, text8, text9, text10));
        }

        static public string AS(object text1, object text2, object text3, object text4, object text5, object text6, object text7, object text8, object text9, object text10, object text11 = null, object text12 = null, object text13 = null, object text14 = null, object text15 = null, object text16 = null, object text17 = null, object text18 = null, object text19 = null, object text20 = null)
        {
            return AS(AS(text1, text2, text3, text4, text5, text6, text7, text8, text9, text10), AS(text11, text12, text13, text14, text15, text16, text17, text18, text19, text20));
        }

        static public string AS(object text1, object text2, object text3, object text4, object text5, object text6, object text7, object text8, object text9, object text10,
            object text11, object text12, object text13, object text14, object text15, object text16, object text17, object text18, object text19, object text20,
            object text21 = null, object text22 = null, object text23 = null, object text24 = null, object text25 = null, object text26 = null, object text27 = null, object text28 = null, object text29 = null, object text30 = null)
        {
            return AS(AS(text1, text2, text3, text4, text5, text6, text7, text8, text9, text10), AS(text11, text12, text13, text14, text15, text16, text17, text18, text19, text20),
                AS(text21, text22, text23, text24, text25, text26, text27, text28, text29, text30));
        }
    }
}
