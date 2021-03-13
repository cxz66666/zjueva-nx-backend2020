using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2020_backend.Models
{
    public class SMS
    {
        [Key]
        public int ID { get; set; }

        public string id_student { get; set; }

        public DateTime sendTime { get; set; }

        public string type { get; set; }
        public string OperatorName{get;set;}
        public int Status { get; set; }

        public SMS() { }
    }
}
