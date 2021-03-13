using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _2020_backend.Data;
using _2020_backend.Models;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Utils;

namespace _2020_backend.Pages
{
    public class MigrateModel : PageModel
    {
        [BindProperty(SupportsGet = true,Name ="action")]
        public string action { get; set; }
        private readonly BackendContext _context;
        public MigrateModel(BackendContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (action == "fail")
            {
                List<string> Phones=await _context.Record.Where(r=>r.status==Status.Fail).Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate)).Select(r=> "\"" + r.phone + "\"").ToListAsync();
                IList<string> Names= await _context.Record.Where(r => r.status == Status.Fail).Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate)).Select(r => r.name).ToListAsync();
                List<FailDto> failDtos = new List<FailDto>();
                foreach(string x in Names)
                {
                    failDtos.Add(new FailDto(x));
                }
                Utils.AliyunSMS.SendFailSms<FailDto>(Phones, failDtos);
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            _context.Database.Migrate();
            return RedirectToPage("./Index");
        }
    }
}