using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Apps.Model
{
    public class MD5Encode
    {
        public static string Encode16(string content)
        {
            byte[] result = Encoding.Default.GetBytes(content);    //tbPass为输入密码的文本框
            MD5 md5 = MD5.Create();
            byte[] output = md5.ComputeHash(result);
            var resultStr = BitConverter.ToString(output).Replace("-", "");

            return resultStr;
        }
    }
}