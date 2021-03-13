using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _2020_backend.Data;
using _2020_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace _2020_backend.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;
        public IList<Record> Record { get; set; }
        public int PageCount { get; set; }
        public int PageId { get; set; }
        [BindProperty(SupportsGet =true,Name ="search")]
        public string SearchString { get; set; }

        public CreateModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User NowUser { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (await _context.User.AsNoTracking().Where(u => u.stuID == NowUser.stuID).FirstOrDefaultAsync() != null)
                return new ConflictResult();
            NowUser.Secret = Utils.EvaCryptoHelper.Password2Secret(NowUser.Secret);
            _context.User.Add(NowUser);
           
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
