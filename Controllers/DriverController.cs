using VanDriverAPI.App_Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VanDriverAPI.Models;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using static Azure.Core.HttpHeader;
using Microsoft.OpenApi.Writers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace VanDriverAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        //private readonly DataLayer _dataLayer;
        private readonly IConfiguration _configuration;

        public DriverController(IConfiguration configuration)
        {
            //_dataLayer = dataLayer;
            _configuration = configuration;
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        //{
        //string connectionString = _configuration.GetConnectionString("DefaultConnection");

        //using (SqlConnection conn = new SqlConnection(connectionString))
        {
            //string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
            //SqlCommand cmd = new SqlCommand(query, conn);
            //cmd.Parameters.AddWithValue("@Email", user.Email);
            //cmd.Parameters.AddWithValue("@Password", user.Password);

            //conn.Open();
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid login attempt");
            }

            //int count = (int)cmd.ExecuteScalar();
            int count = 1;

            if (count == 1)
            {
                var token = GenerateJwtToken(user.Email);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }
        //}

        private string GenerateJwtToken(string email)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("DriverController is working");
        }
    }
}