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
        private readonly DataLayer _dataLayer;
        private readonly IConfiguration _configuration;

        public DriverController(IConfiguration configuration,DataLayer dataLayer)
        {
            _dataLayer = dataLayer;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] JsonElement json)
        {
            try
            {
                if (!json.TryGetProperty("userId", out JsonElement userIdElement) ||
                    !json.TryGetProperty("password", out JsonElement passwordElement))
                {
                    return BadRequest(new { status = false, message = "Invalid JSON Format" });
                }

                string userId = userIdElement.GetString();
                string password = passwordElement.GetString();

                var ds = _dataLayer.LoginCredentials(userId, password);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return Unauthorized(new { status = false, message = "Invalid Credentials" });
                }

                string status = ds.Tables[0].Rows[0]["status"].ToString();

                if (status.Equals("Successs", StringComparison.OrdinalIgnoreCase))  // Case-insensitive check
                {
                    var token = GenerateJwtToken(userId);

                    return Ok(new
                    {
                        status = true,
                        message = "Login Successful",
                        data = JsonConvert.SerializeObject(ds.Tables[0]),
                        token = token
                    });
                }
                else
                {
                    return Unauthorized(new { status = false, message = "Invalid Credentials" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = "Error: " + ex.Message });
            }
        }



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