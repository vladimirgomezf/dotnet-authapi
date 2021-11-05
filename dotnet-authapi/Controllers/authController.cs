using dotnet_authapi.Dtos;
using dotnet_authapi.Helpers;
using dotnet_authapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_authapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly Models.IUserRepository _respository;
        private readonly JwtService _jwtservice;

        public authController(Models.IUserRepository repository, JwtService jwtService)
        {
            _respository = repository;
            _jwtservice = jwtService;
        }

        [HttpPost]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Nombre = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _respository.Create(user);

            return Created("success", _respository.Create(user));
        }

        [HttpGet]
        public IActionResult Login (LoginDto dto)
        {
            var user = _respository.GetByEmail(dto.Email);

            if (user == null) return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new { message = "Invalid Password" });

            var jwt = _jwtservice.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "success"
            });
        }

        [HttpGet]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtservice.Verify(jwt);

                int userId= int.Parse(token.Issuer);

                var user = _respository.GetById(userId);

                return Ok(user);
            } catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
    }
}
