using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Models;

namespace _2020_backend.Pages.Result
{
    public class IndexModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public IndexModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int SelectDepartment { get; set; }
        public IList<Record> Record { get;set; }

        public  async Task<IActionResult> OnGetAsync(int? SelectDepartment)
        {
            
            if (SelectDepartment == null)
            {
                Record = await _context.Record.Where(r => r.status == Status.Pass).Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate)).ToListAsync();
                return Page();
            }
            if (SelectDepartment > 0 && SelectDepartment <= 5)
            {
                Record = await _context.Record.Where(r => r.status == Status.Pass).Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate)).Where(r=>r.FinalResult== SelectDepartment).ToListAsync();
                return Page();
            }
            return NotFound();
        }
    }
}
