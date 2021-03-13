using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _2020_backend.Data;
using _2020_backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _2020_backend.Pages.Records
{
    public class CreateModel : PageModel
    {
        
        private readonly IConfiguration _configuration;
        private readonly _2020_backend.Data.BackendContext _context;

        [BindProperty]
        public List<TimeDto> TimeDtos { get; set; }

        public CreateModel(IConfiguration configuration,_2020_backend.Data.BackendContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string SelectDay)
        {
          
          
           
            TimeDtos = await _context.Time.OrderBy(r => r.Day).ThenBy(r => r.BeginTime).ThenBy(r => r.Place).Select(r => new TimeDto
            {
                ID = r.ID,
                Day = r.Day,
                BeginTime = r.BeginTime,
                Place = r.Place
            }).ToListAsync();
            
            return Page();
        }

      

      /*  public  SelectList GetDayEtc(string day)
        {
           
            IQueryable<string> Etc = from m in _context.Time
                                     where m.Day == day
                                     select m.EtcNumber;
        SelectList EtcList = new SelectList( Etc.Distinct().ToList());
            return  EtcList;              
        }*/

        [BindProperty]
        public Record Record { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Record.addedDate = DateTime.Now;
            Record.status = Status.Pending;
            Record.Times = new List<int>();
            foreach(var x in TimeDtos)
            {
                if (x.IsChoose)
                    Record.Times.Add(x.ID);
            }
            if(Record.strguid==null)
                Record.strguid= Guid.NewGuid().ToString("N").Substring(0,12);

            var ip=Request.Headers["X-Real-IP"].FirstOrDefault();
            if (ip!= null)
            {
                Record.ip = ip;
            }
            else Record.ip = "miss";
            _context.Record.Add(Record);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
