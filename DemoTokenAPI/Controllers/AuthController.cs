using DemoTokenAPI.Models.DTOs;
using DemoTokenAPI.Models;
using DemoTokenAPI.Services;
using DemoTokenAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoTokenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(TokenManager _tokenManager, UserService _userService) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginForm form) 
        { 
            if(!ModelState.IsValid) return BadRequest(ModelState);

            User currentUser;
            try
            {
                 currentUser = _userService.Login(form.Email, form.Password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            string token = _tokenManager.GenerateToken(currentUser);
            return Ok(token);
        }

        [Authorize("adminRequired")]
        [HttpGet("getUsers")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAllUsers());
        }

        [Authorize("connected")]
        [HttpGet("getProfil")]
        public IActionResult Get() 
        {
            string tokenFromRequest = HttpContext.Request.Headers["Authorization"];
            string OkToken = tokenFromRequest.Substring(7, tokenFromRequest.Length - 7);

            //Transforme la chaine de caractère Token en objet c#
            JwtSecurityToken jwt = new JwtSecurityToken(OkToken);
            int id = int.Parse(jwt.Claims.First(x => x.Type == "UserId").Value);

            return Ok(_userService.GetById(id));
        }

    }
}
