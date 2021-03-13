using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Models;
using _2020_backend.Utils;
using NPOI.SS.Formula.Functions;

namespace _2020_backend.Pages.Records
{
    public class IndexModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public IndexModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }
        const int pageSize = 30;
        public int PageCount { get; set; }
        public int PageId { get; set; }
        [BindProperty(SupportsGet = true, Name = "search")]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet =true)]
        public int EtcId { get; set; }

        public IList<Record> Record { get;set; }

        public async Task OnGetAsync(int?pageId)
        {
            PageId = pageId??1;
            SearchString = SearchString ?? string.Empty;
            SearchString = SearchString.Trim();
            IQueryable<Record> recordQuery;
           

            if (SearchString != string.Empty)
            {
                long _studentId = 0;
                long.TryParse(SearchString, out _studentId);
                if (_studentId > 0)
                {
                    // means searchstring is student_id_number
                    recordQuery = from r in _context.Record
                                  where r.id_student == SearchString
                                  orderby r.addedDate descending
                                  select r;
                }
                else
                {
                    //means searchstring is name
                    recordQuery = from r in _context.Record
                                  where r.name == SearchString
                                  orderby r.addedDate descending
                                  select r;
                }

            }
            else
            {
                recordQuery = from r in _context.Record
                              orderby r.addedDate descending
                              select r;
            }
            //筛选每个人最近的一次提交
            if (SearchString == string.Empty)
                recordQuery = recordQuery
                 .Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate));

            if (EtcId > 0)
            {
                recordQuery = from r in recordQuery
                              where r.InterviewID == EtcId
                              select r;
            }
            (Record, PageCount) = await PaginatedList<Record>.CreateAsync(recordQuery.AsNoTracking(), PageId, pageSize);
           
            foreach(var x in Record)
            {
                if (x.InterviewID>0)
                {
                    InterviewTime interviewTime = await _context.Time.FirstOrDefaultAsync(r => r.ID == x.InterviewID);
                    if (interviewTime != null)
                    {
                        x.InterviewTime = $"{interviewTime.Day} {interviewTime.BeginTime} {interviewTime.Place}";
                    }
                    else
                    {
                        x.InterviewTime = "暂无数据";
                    }
                }
                else
                {
                    x.InterviewTime = "暂无数据";
                }
            }
          
        }
        public string CheckNum(string id_student)
        {
            int num = _context.Record.Where(r => r.id_student == id_student).Count();
            if (num > 1)
                return "👀";
            return "";
        }
    }
}
