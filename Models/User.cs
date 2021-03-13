using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Internal;

namespace _2020_backend.Models
{
    
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="ID")]
        public string Uid { get; set; }
        [Display(Name = "学号")]
        public long stuID { get; set; }
        [Display(Name ="姓名")]
        public string Name { get; set; }
        [Display(Name ="密码")]
        public string Secret { get; set; }
        [Display(Name ="管理员")]
        public bool isManager { get; set; }

    }
    public class Login
    {
        private readonly string[]  Manager = { "3190102826", "3190104611", "3190104698", "3190103719", "3190104143", "3190100494", "3190103301", "3190102034", "3190100151", "3190103577", "3190105399", "3190100133" };
        [JsonProperty("username")]
        public string id_student { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        public Login(string name,string pwd)
        {
            id_student = name;
            Password = pwd;
        }
        public string GetSHASecret() => _2020_backend.Utils.EvaCryptoHelper.Password2Secret(Password);
        public bool isManager()
        {
            if (Array.IndexOf(Manager,id_student) != -1)
                return true;
            return false;
        }
    }
}
