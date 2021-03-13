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
    public class DetailsModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public DetailsModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }

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
    }
}
