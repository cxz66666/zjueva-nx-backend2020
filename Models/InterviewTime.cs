using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Threading.Tasks;

namespace _2020_backend.Models
{
    public class InterviewTime
    {
        [Display(Name ="场次ID")]
        [Required()]
        public int ID { get; set; }
        [Display (Name="面试日期")]
        [Required()]
        public string Day { get; set; }
       

      
        [RegularExpression(@"^[0-9]{2}:[0-9]{2}$",ErrorMessage ="请输入类似09:00或23:00的数据")]
        [Display(Name ="开始时间")]
        [Required()]
        public string BeginTime { get; set; }

        [RegularExpression(@"^20[04]$", ErrorMessage = "200或204")]
        [Display(Name = "地点")]
        [Required()]
        public string Place { get; set; }

        [Display(Name ="主面试官")]
        public string Chief { get; set; }
        [Display(Name ="可容纳人数")]
        public int TakenNum { get; set; }

        [Display(Name = "当前人数")]
        public int NowNum { get; set; }

         [Display(Name = "短信通知")]
        public bool SendSMS { get; set; }
        [Display(Name ="面试的人")]
        public List<int> Students { get; set; }
   
    }
    public class TimeApiUsed
    {
        public int ID { get; set; }
        public string Day { get; set; }
        public string BeginTime { get; set; }
        public string Place { get; set; }
        public int TakenNum { get; set; }
        public int NowNum { get; set; }
    }
    public class TimeDto
    {
        [Display(Name = "场次ID")]
        [Required()]
        public int ID { get; set; }

        [Display(Name = "面试日期")]
      
        public string Day { get; set; }
        [Display(Name = "开始时间")]
        
        public string BeginTime { get; set; }
        [Display(Name = "地点")]
      
        public string Place { get; set; }

        [Display(Name ="是否选择")]
        [Required()]
        public bool IsChoose { get; set; }
    }
}
