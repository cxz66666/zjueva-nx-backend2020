using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using System.Collections.Generic;

namespace _2020_backend.Utils
{
    public class FailDto
    {
        public string name { get; set; }
       public FailDto(string name)
        {
            this.name = name;
        }
    }
    public class PassDto
    {
        //ToDO
   
    }
    public class AliyunSMS
    {
        public static string  GetSignNameString(int Count)
        {
            string sign = "[";
            for (int x = 0; x < Count; x++)
            {


                sign = sign + "\"EVA记录\"";
                if (x != Count - 1)
                {
                    sign += ",";
                }

            }
            sign += "]";
            return sign;
        }
        //拿到类似手机号码的string
        public static string ListToStringList(List<string> Target)
        {
            string ans = String.Join<string>(",", Target);
            ans = "[" + ans + "]";
            return ans;
        }

        //拿到json格式的数组  为TemplateParamJson
        public static string GetJsonTemplateParamJson<T>(List<T> Target)
        {
            string Params = JsonHelper.json(Target);
            return Params;
        }
        public static  void SendFailSms<T>(List<string>PhoneNum, List<T>failDto)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "______", "________");
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendBatchSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumberJson",ListToStringList(PhoneNum));
            request.AddQueryParameters("SignNameJson", GetSignNameString(failDto.Count));
            request.AddQueryParameters("TemplateCode", "SMS_199760197");
            request.AddQueryParameters("TemplateParamJson", GetJsonTemplateParamJson(failDto));
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
