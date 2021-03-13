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
    public class DetailsModel : PageModel
    {
        private readonly _2020_backend.Data.BackendContext _context;

        public DetailsModel(_2020_backend.Data.BackendContext context)
        {
            _context = context;
        }

        public Record Record { get; set; }
       public List<Record> Records { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Record = await _context.Record.FirstOrDefaultAsync(r => r.rid == id);

            IQueryable<Record> tmp = from r in _context.Record
                                     where r.id_student==Record.id_student
                                     orderby r.addedDate descending
                                     select r;
            Records =await tmp.ToListAsync();
            

            if (Record.InterviewID > 0)
            {
                InterviewTime interview = await _context.Time.FirstOrDefaultAsync(r => r.ID == Record.InterviewID);
                Record.InterviewTime = $"{interview.Day} {interview.BeginTime}";
            }
            else
            {
                Record.InterviewTime = $"暂无数据";
            }
            if (Record == null)
            {
                return NotFound();
            }
            return Page();
        }
        public string Encode(string stuId)
        {
            long result = 0;
            try
            {
                result = long.Parse(stuId);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (FormatException e)
            {
                throw e;
            }
            catch (OverflowException e)
            {
                throw e;
            }
            finally
            {
                result = ((result ^ 1242458739) + 1984) ^ 4281719956;
            }
            return result.ToString();
        }

    }
}
