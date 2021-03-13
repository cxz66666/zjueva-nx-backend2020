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

namespace _2020_backend.Pages.Times
{
    public class EditModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public EditModel(_2020_backend.Data.BackendContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            InterviewTime interview =await _context.Time.AsNoTracking().FirstOrDefaultAsync(r => r.ID == InterviewTime.ID);

            InterviewTime.Students = new List<int>();
            foreach(var x in interview.Students)
            {
                InterviewTime.Students.Add(x);
            }

            _context.Attach(InterviewTime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterviewTimeExists(InterviewTime.ID))
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

        private bool InterviewTimeExists(int id)
        {
            return _context.Time.Any(e => e.ID == id);
        }
    }
}
