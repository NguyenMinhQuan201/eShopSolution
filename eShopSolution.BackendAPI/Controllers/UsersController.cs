using eShopSolution.Application.System.User;
using eShopSolution.ViewMoldes.System;
using eShopSolution.ViewMoldes.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService usersService)
        {
            _userService = usersService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultToken = await _userService.Authencate(request);

            if (string.IsNullOrEmpty(resultToken.ResultObj))
            {
                return BadRequest(resultToken);
            }
           
            return Ok(resultToken);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://localhost/api/user/id=
        [HttpPut("{id}")]
        
        public async Task<IActionResult> Update(Guid id ,[FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Update(id,request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://localhost/api/user/paging?pageIndex=1&pageSize=10&keyword=
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            var products = await _userService.GetUsersPaging(request);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var products = await _userService.GetById(Id);
            return Ok(products);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var products = await _userService.Delete(Id);
            return Ok(products);
        }
    }
}
