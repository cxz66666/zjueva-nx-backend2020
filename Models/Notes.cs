using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
namespace _2020_backend.Models
{
    public class Notes
    {
        public int ID { get; set; }
        [Required()]
        public string OperatorId { get; set; }
        [Required()]
        public string OperatorName { get; set; }

        [Required()]
        [Display(Name ="评论")]
        public string Content { get; set; }
        public DateTime AddTime { get; set; }
        public int RecordId { get; set; }
        public Notes() { }
        public Notes(string opid,string opname,string content,int recordid)
        {
            OperatorId = opid;
            OperatorName = opname;
            Content = content;
            RecordId = recordid;
            AddTime = DateTime.Now;
        }

    }
}
