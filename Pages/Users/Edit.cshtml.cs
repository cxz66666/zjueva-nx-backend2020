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

namespace _2020_backend.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public EditModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }

        [BindProperty]
        public new User User { get; set; }
        private string _secret { get; set; }

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
            _secret = User.Secret;
            User.Secret = string.Empty;
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
            if (await _context.User.AsNoTracking().Where(u => u.stuID == User.stuID).CountAsync() > 0
                && (await _context.User.AsNoTracking().Where(u => u.stuID == User.stuID).FirstOrDefaultAsync()).Uid != User.Uid
                )
                return new ConflictResult();
            if (User.Secret == null || User.Secret == String.Empty)
            {
                var user = await _context.User.AsNoTracking().Where(u => u.Uid == User.Uid).FirstOrDefaultAsync();
                user.Name = User.Name;
                user.stuID = User.stuID;
                user.isManager = User.isManager;
                _context.Attach(user).State = EntityState.Modified;
            }
            else
            {
                var user = await _context.User.AsNoTracking().Where(u => u.Uid == User.Uid).FirstOrDefaultAsync();
                user.Name = User.Name;
                user.stuID = User.stuID;
                user.isManager = User.isManager;
                user.Secret = Utils.EvaCryptoHelper.Password2Secret(User.Secret);
                _context.Attach(user).State = EntityState.Modified;
            }
         

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Uid))
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

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Uid == id);
        }
    }
}
