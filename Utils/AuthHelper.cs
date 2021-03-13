using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace _2020_backend.Utils
{
    public static class EvaClaimTypes
    {
        public const string IsManager = "IsManager";
    }
    public static class LoginHelp
    {
        public static string PostMoths(string url, object obj_model)
        {
            string param = JsonConvert.SerializeObject(obj_model);
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            /*   if (dic != null && dic.Count != 0)
               {
                   foreach (var item in dic)
                   {
                       request.Headers.Add(item.Key, item.Value);
                   }
               }*/
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(param);
            request.ContentLength = payload.Length;
            string strValue = "";
            try
            {
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate;
                }
            }
            catch (Exception e)
            {
                strValue = e.Message;
            }
            return strValue;
        }
    }
    public static class EvaCryptoHelper
    {
        public static string SHA1(string content)
        {
            try
            {
                using (var sha1=new SHA1CryptoServiceProvider())
                {
                    byte[] bytes_in = Encoding.UTF8.GetBytes(content);
                    byte[] bytes_out = sha1.ComputeHash(bytes_in);
                    string result = BitConverter.ToString(bytes_out);
                    result = result.Replace("-", "");
                    return result;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string Password2Secret(string password)
        {
            return EvaCryptoHelper.SHA1("W5D1" + EvaCryptoHelper.SHA1("EVa" + password + "n13@!@") + "D1X1AnsH3ng");
        }
    }
}
