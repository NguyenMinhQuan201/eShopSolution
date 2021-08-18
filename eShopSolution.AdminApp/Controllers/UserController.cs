using eShopSolution.AdminApp.sevice;
using eShopSolution.Data.Entities;
using eShopSolution.Date.EF;
using eShopSolution.ViewMoldes.System;
using eShopSolution.ViewMoldes.System.User;
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
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 2)
        {
            var request = new GetUserPagingRequest()
            {
                
                KeyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.Keyword = keyword;
            var data = await _userAPIClient.GetUsersPagings(request);
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var result = await _userAPIClient.GetById(Id);
            return View(result.ResultObj);
        }
        [HttpGet]
        public IActionResult Create()
        {
             
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            
                if (!ModelState.IsValid)
                {
                return View();
                }
            
                var result = await _userAPIClient.RegisterUser(request);
                if (result.IsSuccessed)
                {
                    TempData["result"] = "Tao thanh cong";
                    return RedirectToAction("Index");
                }
            ModelState.AddModelError("", result.Message);
            return View(request);
            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userAPIClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userAPIClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Edit thanh cong";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
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
        [HttpGet]
        public IActionResult Delete(Guid id)
        {

            return View(new UserDeleteRequest()
            {
                Id=id 
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userAPIClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Delete thanh cong";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);

        }
    }
}
