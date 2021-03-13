using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Cms;

namespace _2020_backend.Pages.Times
{
    public class IndexModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public IndexModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }
        public string StuName { get; set; }
        public IList<InterviewTime> InterviewTime { get;set; }

        public SelectList DayList { get; set; }
        [BindProperty(SupportsGet =true)]
        public string Day { get; set; }
        public async Task OnGetAsync(int?rid)
        {
            IQueryable<InterviewTime> Tmp = from r in _context.Time
                                          
                                            orderby r.Day ,r.BeginTime,r.Place
                                            
                                            select r;
            IQueryable<string> Days = from r in _context.Time
                                      select r.Day;
          DayList=   new SelectList(await Days.Distinct().ToListAsync());

            if (rid > 0)
            {
                StuName = (await _context.Record.FirstOrDefaultAsync(r => r.rid == rid)).name;
                List<int> times =(await _context.Record.FirstOrDefaultAsync(r => r.rid == rid)).Times;
                if (times == null)
                    Tmp = null;
                else 
                Tmp = Tmp.Where(r => times.Contains(r.ID));

            }
            if (!string.IsNullOrEmpty(Day))
            {
                Tmp = from r in Tmp
                      where r.Day == Day
                      select r;
            }

            InterviewTime = await Tmp.ToListAsync();
        }
    }
}
