using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTheare.Shared.Data;
using TestTheatre.BLL.DTO;
using TestTheatre.BLL.Services;

namespace TestTheatre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService; 
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #region AuthRegion
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (ModelState.IsValid)
            { 
                return Ok(await _userService.RegisterAsync(model));
            }
            return BadRequest(ModelState);

        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _userService.LoginAsync(model));
            }
            return BadRequest(ModelState);
        }



        #endregion
    }
}
