using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Models;

namespace _2020_backend.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public DetailsModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }
        public string SECRET = "EVa N13 @!@";
        public new User User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _context.User.FirstOrDefaultAsync(m => m.Uid == id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
