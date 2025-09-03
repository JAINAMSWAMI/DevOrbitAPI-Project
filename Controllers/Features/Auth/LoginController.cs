using Microsoft.AspNetCore.Mvc;
using DataLayer;
using DevOrbitAPI.DTO;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace DevOrbitAPI.Controllers.Features.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)  
        {
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDTO loginDto )
        {
            try
            {
                Login login = new Login
                {
                    User_Email = loginDto.User_Email,
                    User_Assigned_Pass = EncodePassword(loginDto.User_Assigned_Pass)
                };
                if (login.UserLogin())
                { 
                    var user = login.FetchUserData(loginDto.User_Email);
                    if (user != null)
                    {
                        var token = GenerateJWTToken(user.User_Email, user.User_Role);
                        return Ok(new { success = true, token = token, role = user.User_Role
                        });
                    }
                    
                }
               return Unauthorized(new { success = false, message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string GenerateJWTToken(string email, string role)
        {
            var jwtsettings = _configuration.GetSection("JwtSetting");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(
                issuer: jwtsettings["Issuer"],
                audience: jwtsettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtsettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

            private string EncodePassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create()) {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }

        }
    }
