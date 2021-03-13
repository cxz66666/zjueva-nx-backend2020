using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _2020_backend.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Utils;

namespace _2020_backend.Controllers

{
    

    [Route("api/submit")]
    public class RecordController : Controller
    {
        private readonly BackendContext _context;

        public RecordController(BackendContext context)
        {
            _context = context;
        }
        public void SendAccpetSMS(Record record)
        {
            int num = _context.Sms.Where(r => r.id_student == record.id_student).Where(r => r.type == "收到报名").Count();
            if(num<3)
            {
                int ans = Utils.TencentSMS.SendAcceptSms(record);
                SMS sms = new SMS()
                {
                    id_student = record.id_student,
                    sendTime = DateTime.Now,
                    OperatorName = "傻傻的bot",
                    type="收到报名",
                    Status=ans
                };
                _context.Sms.Add(sms);
                _context.SaveChanges();     
            }    
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]string dto)
        {
            string ip = Request.Headers["X-Real-IP"].FirstOrDefault();
            RecordDto aDto = JsonConvert.DeserializeObject<RecordDto>(dto);
            if (aDto.IsFull() == false)
                return Ok(ApiResponse.Error("TICKET_INFO_INCOMPLETE"));
            if (aDto.Check() == false)
                return Ok(ApiResponse.Error("TICKET_NOT_LEGEAL"));

            Record OldRecord = await _context.Record.Where(r => r.id_student == aDto.id_student).OrderByDescending(r=>r.addedDate).FirstOrDefaultAsync();
            if (OldRecord!=null)
            {
                if(OldRecord.InterviewID > 0)
                return Ok(ApiResponse.Error("TICKET_ALREADY_OK"));
            }
            Record newRecord = new Record(aDto, ip);

            _context.Record.Add(newRecord);

          


            //send yuyueTime sms
            int ans = Utils.TencentSMS.SendYuYueSMS(newRecord);
            SMS sms = new SMS()
            {
                id_student = newRecord.id_student,
                sendTime = DateTime.Now,
                OperatorName = "傻傻的bot",
                Status = ans,
                type = "预约时间"
            };
            _context.Sms.Add(sms);

            await _context.SaveChangesAsync();


            // SendAccpetSMS(newRecord);
            return Ok(ApiResponse.Success("success"));

        }
    }
    [Authorize]
    [ApiController]
    [Route("api/ticketlist")]
    public class TicketList: Controller
    {
        private readonly BackendContext _context;
        public TicketList(BackendContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTicketList([FromQuery]int? pageId, [FromQuery]int? Pass)
        {
            int pageIndex = pageId ?? 0;
            if (pageIndex < 0)
              return Ok(ApiResponse.Error("TICKET_LIST_INVALID_INDEX"));

            int PageCount;
            IList<Record> records;
            if (pageIndex > 0)
            {
                (records, PageCount) = await PaginatedList<Record>.CreateAsync(
                    _context.Record.AsNoTracking().OrderByDescending(t => t.addedDate), pageIndex, 30);
            }
            else
            {
                records = await _context.Record.AsNoTracking().OrderByDescending(t => t.addedDate).ToListAsync();

            }
            if (Pass == 1)
            {
                //TODO! 灵活的表达
                records = await _context.Record.AsNoTracking().Where(r => r.status == Status.Pass).OrderByDescending(t => t.addedDate).ToListAsync();


            }
            return Ok(ApiResponse.Success(records));
        }
    }
    [Authorize]
    [ApiController]
    [Route("api/info")]
    public class InfoController : Controller
    {
        private readonly BackendContext _context;
        public InfoController(BackendContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int:min(0)}")]
        public async Task<IActionResult> GetInfo([FromRoute]int id)
        {
            var query = _context.Record.AsNoTracking().Where(r => r.rid == id);
            Record result = await query.FirstOrDefaultAsync();
            if (result == null)
                return NotFound();
            return Ok(ApiResponse.Success(result));
            
        }
    }

    [ApiController]
    [Route("api/pic")]
    public class PicController : Controller
    {
        private readonly BackendContext _context;

        public PicController(BackendContext context) => _context = context;

        [HttpGet("{id:int:min(0)}")]
        public async Task<IActionResult> GetInfo([FromRoute]int id)
        {
            var query = _context.Record.AsNoTracking().Where(r => r.rid == id);
            var result = await query.FirstOrDefaultAsync();
            if (result == null)
                return NotFound();
            string id_student = result.id_student;

            string src = "https://1299271970796699.cn-hangzhou.fc.aliyuncs.com/2016-08-15/proxy/oss_upload/download/?stuId=" + Encode(id_student);
            return Ok(ApiResponse.Success(src));
        }
        public string Encode(string stuId)
        {
            long result = 0;
            try
            {
                result = long.Parse(stuId);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (FormatException e)
            {
                throw e;
            }
            catch (OverflowException e)
            {
                throw e;
            }
            finally
            {
                result = ((result ^ 1242458739) + 1984) ^ 4281719956;
            }
            return result.ToString();
        }
    }
    [ApiController]
    [Route("api/gettime")]
    public class TimeList : Controller
    {
        private readonly BackendContext _context;
        public TimeList(BackendContext context)
        {
            _context = context;
        }
        [HttpGet]

        public IActionResult GetTimeList()
        {
            var query = _context.Time.Where(r=>r.SendSMS==true).OrderBy(r => r.Day).ThenBy(r => r.BeginTime).ThenBy(r => r.Place).Select(r => new TimeApiUsed
            {
                ID = r.ID,
                Day = r.Day,
                BeginTime = r.BeginTime,
                Place = r.Place,
                TakenNum = r.TakenNum,
                NowNum = r.NowNum

            });
            return Ok(ApiResponse.Success(query));
        }
    }
    [ApiController]
    [Route("api/posttime")]
    public class Posttime : Controller
    {
        private readonly BackendContext _context;
        public Posttime(BackendContext context)
        {
            _context = context;
        }
        [HttpPost]

        public async Task<IActionResult> Post([FromForm]string guid ,[FromForm]string dto)
        {
            Record record = await _context.Record.FirstOrDefaultAsync(r => r.strguid == guid);
            if (record == null)
            {
                return Ok(ApiResponse.Error("GUID_NOT_VAILD"));
            }
            if (record.InterviewID > 0)
            {
                return Ok(ApiResponse.Error("TIME_ALREAY_CONFIRM"));
            }
            RecordTime recordTime = JsonConvert.DeserializeObject<RecordTime>(dto);
            record.Times = new List<int>(recordTime.Times);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse.Success("success"));
        }
    }
    [ApiController]
    [Route("api/getinfo")]
    public class Getinfo : Controller
    {
        private readonly BackendContext _context;
        public Getinfo(BackendContext context)
        {
            _context = context;
        }
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetAsync([FromRoute]string guid)
        {
            
            Record record = await _context.Record.AsNoTracking().FirstOrDefaultAsync(r => r.strguid == guid);
            if(record==null)
            {
                return Ok(ApiResponse.Error("NOT_VAILD_GUID"));
            }
             MiNiStu miNiStu=    new MiNiStu(record.rid, record.name, record.id_student, record.firstWish, record.secondWish, record.thirdWish);
            IEnumerable<InterviewTime> contains = _context.Time.Where(r => record.Times.Contains(r.ID));
            List<string> TimeStrings = contains.Select(r => $"{r.Day} {r.BeginTime}").Distinct().OrderBy(r=>r).ToList();
            miNiStu.Times = new List<string>(TimeStrings);

            return Ok(ApiResponse.Success(miNiStu));
        }
    }
}