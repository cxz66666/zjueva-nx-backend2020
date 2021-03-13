
using _2020_backend.Models;
using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;
namespace _2020_backend.Utils
{
    public class TencentSMS
    {
        public static int SendQueRenSms(Record record,InterviewTime interview)
        {

            Credential cred = new Credential
            {
                SecretId = "*******************",
                SecretKey = "**********************"
            };

            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("sms.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;

            SmsClient client = new SmsClient(cred, "ap-nanjing", clientProfile);
            SendSmsRequest req = new SendSmsRequest();
            req.PhoneNumberSet= new String[] { "+86"+record.phone };
            req.TemplateID = "724175";
            req.SmsSdkAppid = "1400410910";
            req.Sign = "EVA记录";
            req.TemplateParamSet = new String[] { record.name,interview.Day+" "+interview.BeginTime,interview.Place+"室" };

            SendSmsResponse resp = client.SendSmsSync(req);

            if (resp.SendStatusSet[0].Code == "Ok")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int SendYuYueSMS(Record record)
        {
            Credential cred = new Credential
            {
                SecretId = "AKIDqZ1DBslX2NUSJvv0U4RYP0LJ4YEeEnTu",
                SecretKey = "PULL6aoKagW5PSFp0sUqxiDSRGpg6EBu"
            };

            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("sms.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;

            SmsClient client = new SmsClient(cred, "ap-nanjing", clientProfile);
            SendSmsRequest req = new SendSmsRequest();
            req.PhoneNumberSet = new String[] { "+86" + record.phone };
            req.TemplateID = "695460";
            req.SmsSdkAppid = "1400410910";
            req.Sign = "EVA记录";
            req.TemplateParamSet = new String[] { record.name, "面试", record.strguid };

            SendSmsResponse resp = client.SendSmsSync(req);
            Console.WriteLine(resp.SendStatusSet[0]);
            if (resp.SendStatusSet[0].Code == "Ok")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int SendFailSMS(Record record)
        {
            Credential cred = new Credential
            {
                SecretId = "AKIDqZ1DBslX2NUSJvv0U4RYP0LJ4YEeEnTu",
                SecretKey = "PULL6aoKagW5PSFp0sUqxiDSRGpg6EBu"
            };

            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("sms.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;

            SmsClient client = new SmsClient(cred, "ap-nanjing", clientProfile);
            SendSmsRequest req = new SendSmsRequest();
            req.PhoneNumberSet = new String[] { "+86" + record.phone };
            req.TemplateID = "692782";
            req.SmsSdkAppid = "1400410910";
            req.Sign = "EVA记录";
            req.TemplateParamSet = new String[] { record.name };

            SendSmsResponse resp = client.SendSmsSync(req);
            if (resp.SendStatusSet[0].Code == "Ok")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
        public static int SendAcceptSms(Record record)
        {

            Credential cred = new Credential
            {
                SecretId = "AKIDqZ1DBslX2NUSJvv0U4RYP0LJ4YEeEnTu",
                SecretKey = "PULL6aoKagW5PSFp0sUqxiDSRGpg6EBu"
            };

            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("sms.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;

            SmsClient client = new SmsClient(cred, "ap-nanjing", clientProfile);
            SendSmsRequest req = new SendSmsRequest();
            req.PhoneNumberSet = new String[] { "+86" + record.phone };
            req.TemplateID = "716621";
            req.SmsSdkAppid = "1400410910";
            req.Sign = "EVA记录";
            req.TemplateParamSet = new String[] { record.name, record.id_student,Utils.DateHelp.GetDepartment(record.firstWish),Utils.DateHelp.GetDepartment(record.secondWish), Utils.DateHelp.GetDepartment(record.thirdWish) };

            SendSmsResponse resp = client.SendSmsSync(req);

            if (resp.SendStatusSet[0].Code == "Ok")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static List<SMS> PullSMSStatus(string student,string Phone,ulong BeginTime,ulong EndTime)
        {
            Credential cred = new Credential
            {
                SecretId = "AKIDqZ1DBslX2NUSJvv0U4RYP0LJ4YEeEnTu",
                SecretKey = "PULL6aoKagW5PSFp0sUqxiDSRGpg6EBu"
            };

            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("sms.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;

            SmsClient client = new SmsClient(cred,"", clientProfile);
            PullSmsReplyStatusByPhoneNumberRequest req = new PullSmsReplyStatusByPhoneNumberRequest();
            req.PhoneNumber = "+86" + Phone;
            req.SendDateTime = BeginTime;
            req.EndDateTime = EndTime;
            req.Limit = 10;
            req.SmsSdkAppid = "1400410910";
            req.Offset = 0;
            string strParams = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            req = PullSmsReplyStatusByPhoneNumberRequest.FromJsonString<PullSmsReplyStatusByPhoneNumberRequest>(strParams);
            PullSmsReplyStatusByPhoneNumberResponse resp = client.PullSmsReplyStatusByPhoneNumberSync(req);
            Console.WriteLine(AbstractModel.ToJsonString(resp));
            List<SMS> ans = new List<SMS>();
            try
            {
                foreach(var x in resp.PullSmsReplyStatusSet)
                {
                    ans.Add(new SMS()
                    {
                        id_student = student,
                        sendTime = Convert.ToDateTime(x.ReplyTime),
                        type = "PullResponse",
                        OperatorName = x.ReplyContent,
                        Status = 1
                    }) ;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ans;
        }
    }
}
