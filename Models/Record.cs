using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2020_backend.Models
{
    public enum Status
    {
        Pending,
        Pass,
        Fail
    }
    public enum Department
    {
        电脑部=1,
        电器部=2,
        文宣部,
        人资部,
         财外部
    }
    public enum Grade
    {
        大一=1,
        大二=2,
        大三,
        大四
    }
  /*  public enum FINALDAY
    {
        [Display(Name ="24日")]
     first=1,
        [Display(Name = "25日")]
        second =2,
        [Display(Name = "26日")]
        third,
        [Display(Name = "27日")]
        fourth,
        [Display(Name = "28日")]
        fifth,
        [Display(Name = "29日")]
        sixth,
        [Display(Name = "30日")]
        seventh
       
          
    }
    public enum FINALETC
    {
        first=1,
        second=2,
        third,
        fourth,
        fifth
    }*/
    public class RecordTime
    {
        public List<int> Times { get; set; }
    }
    public class Record
    {

        public Record() { }
        public Record(RecordDto Dto, string ip)
        {
            adjustment = Dto.adjustment;
            email = Dto.email;
            firstReason = Dto.firstReason;
            firstWish = Dto.firstWish;
            secondReason = Dto.secondReason;
            secondWish = Dto.secondWish;
            thirdWish = Dto.thirdWish;
            thirdReason = Dto.thirdReason;
            grade = Dto.grade;
            id_student = Dto.id_student;
            major = Dto.major;
            name = Dto.name;
            phone = Dto.phone;
            Times = new List<int>();
            question1 = Dto.question1;
            question2 = Dto.question2;
            strguid = Guid.NewGuid().ToString("N").Substring(0,12);
            if (Dto.sex == 1)
                sex = true;
            else sex = false;
        
            addedDate = DateTime.Now;
            status = Status.Pending;
            if (ip != null)
                this.ip = ip;
            else this.ip = "miss";

        }

        [Display(Name = "ID")]
        public int rid { get; set; }
        [Display(Name = "姓名")]
        public string name { get; set; }
        [Display(Name = "学号")]
        public string id_student { get; set; }
        [Display(Name = "性别")]
        public bool sex { get; set; }
        [Display(Name = "年级")]
        public int grade { get; set; }
        [Display(Name = "专业")]
        public string major { get; set; }
        [Display(Name = "电子邮件")]
        public string email { get; set; }
        [Display(Name = "电话")]
        public string phone { get; set; }
        [Display(Name = "一志愿")]
        public int firstWish { get; set; }
        [Display(Name = "二志愿")]
        public int secondWish { get; set; }
        [Display(Name = "三志愿")]
        public int thirdWish { get; set; }
        [Display(Name = "调剂")]
        public bool adjustment { get; set; }
        [Display(Name = "一志愿原因")]
        public string firstReason { get; set; }
        [Display(Name = "二志愿原因")]
        public string secondReason { get; set; }
        [Display(Name = "三志愿原因")]
        public string thirdReason { get; set; }
        [Display(Name = "特长")]
        public string question1 { get; set; }
        [Display(Name = "得到&付出")]
        public string question2 { get; set; }
        [Display(Name ="GUID")]
        public string strguid { get; set; }

        [Display(Name = "时间列表")]
        public List<int> Times { get; set; }


        [Display(Name = "添加时间")]
        public DateTime addedDate { get; set; }
        [Display(Name = "状态")]
        public Status status { get; set; }
        [Display(Name = "IP")]
        public string ip { get; set; }
  //面试场次ID
        public int InterviewID { get; set; }
        [Display(Name = "面试时间")]
        public string InterviewTime { get; set; }
        
        [Display(Name ="面试结果")]
        public int FinalResult { get; set; }
    }
    
    public class RecordDto
    {
        [Required()]
        public string name { get; set; }
        [Required()]
        public string id_student { get; set; }
        [Required()]
        public int sex { get; set; }
        [Required()]
        public int grade { get; set; }
        [Required()]
        public string major { get; set; }
        [Required()]
        public string email { get; set; }
        [Required()]
        public string phone { get; set; }
        [Required()]
        public int firstWish { get; set; }
        [Required()]
        public int secondWish { get; set; }
        [Required()]
        public int thirdWish { get; set; }
        [Required()]
        public bool adjustment { get; set; }
        [Required()]
        public string firstReason { get; set; }
        [Required()]
        public string secondReason { get; set; }
        [Required()]
        public string thirdReason { get; set; }
        [Required()]
        public string question1 { get; set; }
        [Required()]
        public string question2 { get; set; }
 
       
        public Regex r = new Regex("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
        public Regex phone_regex = new Regex("^1\\d{10}$");

        public bool IsFull() => name != null && id_student != null && (sex == 0 || sex == 1) && grade != 0 && major != null && email != null && phone != null &&
            firstWish != 0 && secondWish != 0&&thirdWish!=0 && firstReason != null && secondReason != null &&thirdReason!=null&& question1 != null && question2 != null;
        public bool Check()
        {
            if (name.Length > 15)
                return false;
            if (Regex.Matches(name, @"\d").Count>0)
                return false;
            if (id_student.Length > 10||id_student.Length<7)
                return false;
            if (sex > 1 || sex < 0)
                return false;
            if (grade < 1 || grade > 4)
                return false;
            if (major.Length > 20)
                return false;
            if (!r.IsMatch(email))
            {
                return false;
            }
            if (!phone_regex.IsMatch(phone))
            {
                return false;
            }
            if (firstWish < 1 || firstWish > 5 || secondWish < 1 || secondWish > 5|| thirdWish < 1 || thirdWish > 5)
            {
                return false;
            }
         
            return true;
        }
    }
    public class MiNiStu
    {
        public MiNiStu(int r,string name,string id_stu,int first,int second,int third)
        {
            rid = r;
            Name = name;
            id_student = id_stu;
            firstWish = first;
            secondWish = second;
            thirdWish = third;
            Times = new List<string>();
        }
        public int rid { get; set; }
        public string Name { get; set; }
        public string id_student { get; set; }
        public int firstWish { get; set; }
        public int secondWish { get; set; }
        public int thirdWish { get; set; }
        public List<string> Times { get; set; }

    }
}
