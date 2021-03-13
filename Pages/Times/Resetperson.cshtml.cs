using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2020_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;

namespace _2020_backend.Pages.Times
{
    public class ResetpersonModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public ResetpersonModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? rid)
        {
            if (rid == null)
            {
                return NotFound();
            }
            Record record = await _context.Record.FirstOrDefaultAsync(m => m.rid == rid);
            if (record == null)
            {
                return NotFound();
            }


            if (record.InterviewID != 0)
            {
                InterviewTime interview = await _context.Time.FirstOrDefaultAsync(r => r.ID == record.InterviewID);
                if (interview != null)
                {
                    interview.Students.Remove(record.rid);
                    interview.NowNum--;

                    await _context.SaveChangesAsync();
                }
                record.InterviewID = 0;
                record.InterviewTime = null;
                await _context.SaveChangesAsync();
               

            }

            return RedirectToPage("../Records/index");
        }
    }
}