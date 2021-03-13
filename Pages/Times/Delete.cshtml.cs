using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Models;

namespace _2020_backend.Pages.Times
{
    public class DeleteModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public DeleteModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InterviewTime InterviewTime { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InterviewTime = await _context.Time.FirstOrDefaultAsync(m => m.ID == id);

            if (InterviewTime == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InterviewTime = await _context.Time.FindAsync(id);

            if (InterviewTime != null)
            {
                if (InterviewTime.Students.Count() > 0)
                {
                    foreach(var x in InterviewTime.Students)
                    {
                        Record record = await _context.Record.FirstOrDefaultAsync(r => r.rid == x);
                        record.InterviewID = 0;
                        record.InterviewTime = null;
                        await _context.SaveChangesAsync();
                            
                    }
                }
                _context.Time.Remove(InterviewTime);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
