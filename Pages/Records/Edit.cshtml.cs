using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace _2020_backend.Pages.Records
{
    public class EditModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public EditModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }



        [BindProperty]
        public int Oldid { get; set; }
        public SelectList DayList { get; set; }
        [Display(Name = "面试日期")]
        [BindProperty]
        public string SelectDay { get; set; }

        public string RecommendTime { get; set; }

        [BindProperty]
        [Display(Name = "面试场次")]
        public string SelectTime { get; set; }
        public SelectList EtcList { get; set; }

        [BindProperty]
        public Record Record { get; set; }

        [BindProperty]
        public List<TimeDto> TimeDtos { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id, string SelectDay)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeDtos = await _context.Time.OrderBy(r => r.Day).ThenBy(r => r.BeginTime).ThenBy(r => r.Place).Select(r => new TimeDto
            {
                ID = r.ID,
                Day = r.Day,
                BeginTime = r.BeginTime,
                Place = r.Place
            }).ToListAsync();

            Oldid = (int)id;
            Record = await _context.Record.FirstOrDefaultAsync(m => m.rid == id);
            if (Record == null)
            {
                return NotFound();
            }

            foreach(var x in TimeDtos)
            {
                if (Record.Times == null)
                    break;
                if (Record.Times.Contains(x.ID))
                {
                    x.IsChoose = true;
                }
            }

            if (Record.InterviewID > 0)
            {
                InterviewTime interview =await _context.Time.FirstOrDefaultAsync(r => r.ID == Record.InterviewID);
                this.SelectDay = interview.Day;
                this.SelectTime = interview.BeginTime+" "+interview.Place+" 剩余"+(interview.TakenNum-interview.NowNum).ToString();
            }

            IQueryable<string> Day = from m in _context.Time
                                     select m.Day;

            DayList = new  SelectList(await Day.Distinct().OrderBy(r => r).ToListAsync());
            if (SelectDay != null)
            {
 //if user is not admin only show the user choosed time
                IEnumerable<InterviewTime> contains=_context.Time ;
                if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value!="纳新系统管理员")
                {
                    contains = contains.Where(r => Record.Times.Contains(r.ID));
                }

                IEnumerable<string> onlyShowChoose = contains.Where(m => m.Day == SelectDay).Where(m => m.TakenNum != m.NowNum).Select(m => m.BeginTime + " " + m.Place + " 剩余" + (m.TakenNum - m.NowNum).ToString());
               
                EtcList = new SelectList(onlyShowChoose.Distinct());
                this.SelectDay = SelectDay;
            }
            //get recommendTime to choose
            RecommendTime = GetRecommendTime(Record.rid);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
           //添加日期
            if (SelectDay != null && SelectTime !=null)
            {
                //注意剩余前面的那个空格！
                SelectTime = SelectTime.Substring(0, SelectTime.IndexOf(" 剩余"));
                InterviewTime interviewTime = await _context.Time.FirstOrDefaultAsync(r => r.Day == SelectDay && r.BeginTime+" "+r.Place == SelectTime);
                if (interviewTime!=null)
                {
                    if (interviewTime.NowNum == interviewTime.TakenNum)
                    {
                        return RedirectToPage("../Error");
                    }

                    InterviewTime oldTime=null;
                    foreach(var x in _context.Time)
                    {
                        try
                        {
                            if (x.Students.Contains(Record.rid))
                            {
                                oldTime = x;
                                break;
                            }
                        }
                         catch(NullReferenceException e)
                        {
                            throw e;
                        }
                    }
                    if (oldTime != null)
                    {
                        if(oldTime.NowNum>0)
                        oldTime.NowNum -= 1;
                        oldTime.Students.Remove(Record.rid);
                        await _context.SaveChangesAsync();
                    }
                    interviewTime.NowNum += 1;
                    interviewTime.Students.Add(Record.rid);
                    await _context.SaveChangesAsync();
                    Record.InterviewID = interviewTime.ID;
                    Record.InterviewTime = $"{SelectDay} {interviewTime.BeginTime}";
                   
                }
            }
            //重新赋值
            var record = _context.Record.AsNoTracking().Where(r => r.rid == Record.rid).FirstOrDefault();
            
            Record.status = record.status;
            Record.addedDate = record.addedDate;
            Record.ip = record.ip;
            Record.strguid = record.strguid;
            Record.Times = new List<int>();
            foreach (var x in TimeDtos)
            {
                if (x.IsChoose)
                    Record.Times.Add(x.ID);
            }

            _context.Attach(Record).State = EntityState.Modified;   

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(Record.rid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RecordExists(int id)
        {
            return _context.Record.Any(e => e.rid == id);
        }
       private  string GetRecommendTime(int rid)
        {
            Record record = _context.Record.FirstOrDefault(r => r.rid == rid);
            if (record == null)
            {
                return "NOT FOUND";
            }
            List<int> Times = new List<int>(record.Times);
            int remaining = -1;
            int TimeID = 0;
            foreach(int x in Times)
            {
                InterviewTime interview =  _context.Time.FirstOrDefault(r => r.ID == x);
                if (interview == null)
                    continue;
                if (interview.TakenNum - interview.NowNum > remaining)
                {
                    remaining = interview.TakenNum - interview.NowNum;
                    TimeID = x;
                }
            }
            if (remaining > 0)
            {
                //the most remaining interview place
                InterviewTime interview =  _context.Time.FirstOrDefault(r => r.ID == TimeID);
                return interview.Day + " " + interview.BeginTime + " " + interview.Place + " " + "剩余" + (interview.TakenNum - interview.NowNum).ToString() + "人";
            }
            else return "NOT FOUND";
        }
    }
}
