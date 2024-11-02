using Business;
using Core;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using REST;
using REST.Controllers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using static Data.AppException;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace REST.Controllers
{
    [ApiController]
    [Route("users")]

    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager _mgr;
        private readonly string _secretKey;


        public UserController(UnitOfWork unitOfWork, ILogger<UserController> logger, IOptions<JwtSettings> appSettings) : base(unitOfWork)
        {
            _mgr = new UserManager(unitOfWork);
            _logger = logger;
            _secretKey = appSettings.Value.SecretKey;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserInput login)
        {
            var result = await _mgr.Authenticate(login, _secretKey);
            return Ok(result);
        }

    }
}
