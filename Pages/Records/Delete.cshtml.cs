using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Data;
using _2020_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace _2020_backend.Pages.Records
{
    public class DeleteModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public DeleteModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }   

        [BindProperty]
        public Record Record { get; set; }
        [BindProperty]
        [Display(Name ="删除改人全部的记录")]
        public bool DeleteAll { get; set; }


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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Record = await _context.Record.FindAsync(id);

            
            if (Record != null)
            {
                if (!DeleteAll)
                {
                    if (Record.InterviewID != 0)
                    {
                        InterviewTime interviewTime = await _context.Time.FirstOrDefaultAsync(r => r.ID == Record.InterviewID);
                        if (interviewTime.NowNum > 0)
                            interviewTime.NowNum -= 1;
                        interviewTime.Students.Remove(Record.rid);
                        await _context.SaveChangesAsync();
                    }
                    _context.Record.Remove(Record);
                    await _context.SaveChangesAsync();
                }
               else
                {
                    foreach(var x in _context.Record)
                    {
                        if(x.name==Record.name&&x.id_student==Record.id_student&&x.sex==Record.sex&&x.phone==Record.phone)
                        {
                            _context.Record.Remove(x);
                            
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
