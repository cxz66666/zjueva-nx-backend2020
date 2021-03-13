using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2020_backend.Data;
using _2020_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2020_backend.Pages.Records
{
    public class PendingModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;
        public PendingModel(BackendContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Record Record { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Record = await _context.Record.FirstOrDefaultAsync(m => m.rid == id);
            if (Record == null)
            {
                return NotFound();
            }
            IList<Record> records = await _context.Record.Where(r => r.id_student == Record.id_student).ToListAsync();

            foreach (var item in records)
            {
                item.FinalResult = 0;
                item.status = Status.Pending;
                _context.Record.Update(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}