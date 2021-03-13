using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2020_backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using _2020_backend.Models;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace _2020_backend.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly string LoginURL = "https://xms.zjueva.net/api/auth/login";
        private readonly IConfiguration _config;
        private readonly BackendContext _context;
        public LoginModel(IConfiguration configuration,BackendContext context)
        {
            _config = configuration;
            _context = context;
        }
       
        public IActionResult OnGet(string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return Page();
        }
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (username == _config[ "ADMIN_USERNAME"]&& password == _config["ADMIN_PASSWORD"])
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
                identity.AddClaim(new Claim(ClaimTypes.Name, "纳新系统管理员"));
                identity.AddClaim(new Claim(ClaimTypes.Sid, "0"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim(EvaClaimTypes.IsManager, "true"));
                var Iprincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   Iprincipal,
                   new AuthenticationProperties
                   {
                       IsPersistent = true,
                       AllowRefresh =true
                   }
                    );
                return RedirectToPage("/Index");

            }
            var NowSecret = EvaCryptoHelper.Password2Secret(password);
            long stuID = 0;
            if(!long.TryParse(username,out stuID))
            {
                return RedirectToPage("/Account/Denied");

            }
            var query = from _user in _context.User
                        where _user.stuID == stuID
                        select _user;
            var user = await query.AsNoTracking().FirstOrDefaultAsync();

            //need use xms login service
            if (user == null)
            {
                Login login_Stu = new Login(username, password);

                //post login information to xms.zjueva.net and receive the response with string

                string ansString = LoginHelp.PostMoths(LoginURL, login_Stu);
                JObject ansJson = (JObject)JsonConvert.DeserializeObject(ansString);

                //error and Denied
                if (ansJson["status"].ToString() == "error")
                {
                    return RedirectToPage("/Account/Denied");
                }
                else if (ansJson["status"].ToString() == "success")
                {
                    User add_user = new User()
                    {

                        stuID = long.Parse(ansJson["data"]["stuid"].ToString()),
                        Name = ansJson["data"]["name"].ToString(),
                        Secret = login_Stu.GetSHASecret(),
                        isManager = login_Stu.isManager()
                    };
                    _context.User.Add(add_user);
                    await _context.SaveChangesAsync();
                    //add new user to DB and now continue to create cookie;
                    user = add_user;
                    //this user is correct and  don't use the follow "else"
                }
            }
            if (user.Secret == NowSecret)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,ClaimTypes.Name,ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.Uid));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
                if (user.isManager)
                {
                    identity.AddClaim(new Claim(EvaClaimTypes.IsManager, "true"));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "manager"));
                }
                else
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                }
                var Iprinciple = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    Iprinciple,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true
                    }
                    );
                return RedirectToPage("/Records/Index");
            }

            //if user change his secret in XMS
            else
            {
                Login login_Stu = new Login(username, password);

                //post login information to xms.zjueva.net and receive the response with string

                string ansString = LoginHelp.PostMoths(LoginURL, login_Stu);
                JObject ansJson = (JObject)JsonConvert.DeserializeObject(ansString);
              
                //error and Denied
                if (ansJson["status"].ToString() == "error")
                {
                    return RedirectToPage("/Account/Denied");
                }
                else
                {
                    //change the secret in  DB
                    user = await _context.User.FirstOrDefaultAsync(r => r.stuID == stuID);
                    user.Secret = login_Stu.GetSHASecret();
                    _context.Attach(user).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return NotFound();
                    }
                    return RedirectToPage("/Index");
                }
            }
         
        }

    }
}