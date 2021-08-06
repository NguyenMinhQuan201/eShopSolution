using eShopSolution.AdminApp.sevice;
using eShopSolution.Data.Entities;
using eShopSolution.Date.EF;
using eShopSolution.ViewMoldes.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    
    public class UserController : BaseController
    {
        private readonly EShopDbContext _context;
        
        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;

        public UserController(EShopDbContext context,IUserAPIClient userAPIClient,IConfiguration configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
            _context = context;
            
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var session = HttpContext.Session.GetString("Token");
            var request = new GetUserPagingRequest()
            {
                BearerToken = session,
                KeyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userAPIClient.GetUsersPagings(request);
            return View(data);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
             
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            var a = new IdentityUser<Guid>()
            {
                UserName = request.UserName

            };
            var query = from p in _context.Users where request.UserName == p.UserName select p;
            var check = _context.Users.FirstOrDefault(x=>x.UserName==request.UserName);
           
            if (check == null)
            {
                if (!ModelState.IsValid)
            {
                return View();
            }
            
                var result = await _userAPIClient.RegisterUser(request);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View(request);
            }
            else
            {
                ViewBag.error = "UserName already exists";
                return View();
            }
        }
        
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }
        private ClaimsPrincipal ValidateToken (string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}
