using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _2020_backend.Data;
using _2020_backend.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace _2020_backend.Pages.Times
{
    public class CreateModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public CreateModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [Display(Name ="同时创建200和204场次")]
        [BindProperty]
        public bool CreateTwo { get; set; }
        [BindProperty]
        public InterviewTime InterviewTime { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            InterviewTime.Students = new List<int>();
            if (CreateTwo)
            {
                string anotherPlace = InterviewTime.Place == "200" ? "204" : "200";
                InterviewTime tmp = await _context.Time.FirstOrDefaultAsync(r => r.Place == anotherPlace && r.Day == InterviewTime.Day && InterviewTime.BeginTime == r.BeginTime);

                if (tmp == null)
                {
                    InterviewTime interviewTimeSecond = new InterviewTime()
                    {
                        Day = InterviewTime.Day,
                        BeginTime = InterviewTime.BeginTime,
                        Place = anotherPlace,
                        TakenNum = InterviewTime.TakenNum,
                        Chief=InterviewTime.Chief,
                        SendSMS=InterviewTime.SendSMS ,
                        NowNum = 0,
                        Students = new List<int>()
                    };
                    _context.Add(interviewTimeSecond);
                    await _context.SaveChangesAsync();
                }

                
            }
            _context.Time.Add(InterviewTime);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }
    }
}
