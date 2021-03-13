using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _2020_backend.Data;
using _2020_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;


namespace _2020_backend.Pages.Records
{
    public class CommentModel : PageModel
    {
       
        public  string UID { get; set; }
     
        public  string NAME { get; set; }
        public IList<Notes> Comments { get; set; }
        public List<string> Notes { get; set; }
        [BindProperty]
        public string NewNotes { get; set; }
        [BindProperty]
        public Record Record { get; set; }

        private readonly BackendContext _context;
        public CommentModel(_2020_backend.Data.BackendContext context)
        {
            Notes = new List<string>();
            _context = context;
        }
      
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            UID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            NAME = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            if (id == null)
            {
                return NotFound();
            }
            IQueryable<Notes> AllNotes;
            AllNotes = from r in _context.Note
                       where r.RecordId == id
                       orderby r.AddTime descending
                       select r;

            Comments =await AllNotes.ToListAsync();

            Record = await _context.Record.FirstOrDefaultAsync(u => u.rid == id);
            
            foreach(var n in Comments)
            {
                Notes.Add($"{n.OperatorName} 评论:  {n.Content}     --{n.AddTime.ToString("MM-dd HH:mm")}");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            string uid= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value ;
            string name= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            int rid = Record.rid;
            var note = new Notes(uid, name, NewNotes, rid);

            _context.Note.Add(note);
            await _context.SaveChangesAsync();
            NewNotes = "";
            return await OnGetAsync(rid);
        }
    }
}