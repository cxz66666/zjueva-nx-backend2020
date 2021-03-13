using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using _2020_backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2020_backend.Pages.Times
{
    public class ResetModel : PageModel
    {
        private readonly BackendContext _context;
        public ResetModel(BackendContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var Timelists =await _context.Time.ToListAsync();
            foreach(var x in Timelists )
            {
                try
                {
                    if (x.Students == null)
                    {
                        x.Students = new List<int>();
                        x.NowNum = 0;
                    }
                    else
                    {
                        x.NowNum =  x.Students.Count();
                    }
                }   
             catch(NullReferenceException e)
                {
                    throw e;
                }
             
            }  
            await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
        }
       
    }
}