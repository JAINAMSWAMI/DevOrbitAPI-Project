using Microsoft.AspNetCore.Mvc;
using DataLayer;
using Microsoft.Data.SqlClient;
using DevOrbitAPI.DTO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace DevOrbitAPI.Controllers.Features.Users
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AddUserController : ControllerBase
    {
        [HttpPost("AddMember")]
        public IActionResult InsertUser([FromBody] AddUserDTO userDto)
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(new Operations().connectionString))
                {
                    conn.Open();
                }
                AddUser user = new AddUser
                {
                    User_FullName = userDto.User_FullName,
                    UserName = userDto.UserName,
                    User_Email = userDto.User_Email,
                    User_Assigned_Pass = EncodePassword(userDto.User_Assigned_Pass),
                    User_Role = userDto.User_Role,
                    User_Phone_No = userDto.User_Phone_No,
                    User_Designation = userDto.User_Designation,
                    User_Department = userDto.User_Department,
                    User_LastLogin = DateTime.Now
                };
                if (!User.IsInRole("Admin"))
                {
                    return StatusCode(403, new {message =  "You dont have access to this recoruces" });
                }
                int result = user.NewUser();

                if (result > 0)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest("User insertion failed. Stored Procedure returned 0.");
                }
            }
            catch (Exception ex)
            {
                
                return Ok("Exception: " + ex.Message);
            }
        }

        private string EncodePassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
