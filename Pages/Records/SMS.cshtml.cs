using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _2020_backend.Data;
using _2020_backend.Models;
using _2020_backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cms;
using TencentCloud.Clb.V20180317.Models;

namespace _2020_backend.Pages.Records
{
    public class SMSModel : PageModel
    {
        private readonly BackendContext _context;

        public IList<SMS> AboutSMS { get; set; }
        public IList<SMS> ResponseSMS { get; set; }
        [BindProperty]
        public Record Record { get; set; }
        [BindProperty]
        public bool yuyue { get; set; }
        [BindProperty]
        public bool queren { get; set; }
        [BindProperty]
        public bool zuizhong { get; set; }
        [BindProperty]
        public bool PullResponse { get; set; }
        public SMSModel(BackendContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Record = await _context.Record.FirstOrDefaultAsync(r => r.rid == id);

            if (Record == null)
                return NotFound();
           
         
            //这点没设计好，只能通过type区分了,type不为Pullresponse则为发送，反之则为接受,主要不想动数据库了
            AboutSMS = await _context.Sms.Where(r => r.id_student == Record.id_student).Where(r=>r.type!= "PullResponse").ToListAsync();
            //获取所有回执
           ResponseSMS= await _context.Sms.Where(r => r.id_student == Record.id_student).Where(r => r.type == "PullResponse").ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            int rid = Record.rid;
            Record record = await _context.Record.FirstOrDefaultAsync(r => r.rid == rid);
            if (PullResponse)
            {
                try
                {
                    await GetSMSResponse(Record.id_student, Record.phone);
                }
                catch (Exception e)
                {
                    throw (e);
                }
                return RedirectToPage("./SMS",new { id=Record.rid});
            }
            SMS sms = new SMS()
            {
                id_student =record.id_student,
                sendTime = DateTime.Now,
               
                OperatorName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };
            if (yuyue)
            {
               
                int ans = TencentSMS.SendYuYueSMS(record);
                sms.Status = ans;
                sms.type = "预约时间";
                _context.Sms.Add(sms);
                await _context.SaveChangesAsync();
            }
           else  if (queren)
            {
                InterviewTime interview = await _context.Time.FirstOrDefaultAsync(r => r.ID == record.InterviewID);
                if (interview == null)
                {
                    sms.Status = 0;
                    sms.type = "未选择面试时间";
                }
                else
                {
                    int ans = TencentSMS.SendQueRenSms(record, interview);
                    sms.Status = ans;
                    sms.type = "确认时间";
                }
         
                _context.Sms.Add(sms);
                await _context.SaveChangesAsync();
            }
            else if (zuizhong)
            {
                if (record.status == Status.Pending)
                {
                    sms.Status = 0;
                    sms.type = "面试结果还未确认";
                }
                else if (record.status == Status.Fail)
                {
                    int ans = TencentSMS.SendFailSMS(record);
                    sms.Status = ans;
                    sms.type = "发送失败短信";
                }
                _context.Sms.Add(sms);
                await _context.SaveChangesAsync();
            }
            return await OnGetAsync(rid);
        }
        public async Task GetSMSResponse(string idstudent,string phone)
        {
            SMS sms =await _context.Sms.Where(r => r.id_student == idstudent && r.type == "PullResponse").OrderByDescending(r => r.sendTime).FirstOrDefaultAsync();
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
           
            if (sms != null)
            {

                List<SMS>responseSMS=Utils.TencentSMS.PullSMSStatus(idstudent,phone,(ulong)(sms.sendTime.AddSeconds(2).ToUniversalTime().Ticks-startTime.Ticks)/ 10000000,(ulong)(DateTime.UtcNow.Ticks-startTime.Ticks)/10000000);

                try
                {
                    foreach(var x in responseSMS)
                    {
                        _context.Sms.Add(x);
                    }
                  await  _context.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    throw (e);
                }
            }
            else
            {
           
                List<SMS> responseSMS = Utils.TencentSMS.PullSMSStatus(idstudent, phone, (ulong)( DateTime.UtcNow.AddDays(-6).Ticks - startTime.Ticks) / 10000000, (ulong)(DateTime.UtcNow.Ticks - startTime.Ticks) / 10000000);

                try
                {
                    foreach (var x in responseSMS)
                    {
                        _context.Sms.Add(x);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }
            return ;
        }
    }
}