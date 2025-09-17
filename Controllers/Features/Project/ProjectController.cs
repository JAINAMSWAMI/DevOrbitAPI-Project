using DevOrbitAPI.DTO.Kanban_OPS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DevOrbitAPI.Controllers.Features.Project
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        [HttpPost("AddProject")]
        public IActionResult AddProject([FromBody] Project_DTO projectDto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(new Operations().connectionString))
                {
                    conn.Open();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(role))
                    return Unauthorized(new { message = "Role not found something went wrong" });

                Project_OPS project = new Project_OPS
                {
                    Project_Name = projectDto.Project_Name,
                    Project_Description = projectDto.Project_Description,
                    Project_Created_By = role,
                    Project_Status = projectDto.Project_Status,
                    Project_Version = projectDto.Project_Version
                };
                if (!User.IsInRole("Admin"))
                {
                    return Unauthorized("You dont have access to this recoruces");
                }

                int result = project.AddNewProject();
                if (result > 0)
                {
                    return Ok(new { success = true, message = "Project Created Successfully" });

                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to insert Project" });

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });

            }
        }


        [HttpGet("GetProject")]
        public IActionResult GetProject(GetProjectDTO getProjectDTO)
        {

            try
            {
                

                using (SqlConnection conn = new SqlConnection(new Operations().connectionString))
                {
                    conn.Open();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                GetProject project = new GetProject
                {
                    Project_ID = getProjectDTO.Project_ID,
                    Project_Name = getProjectDTO.Project_Name,
                    Project_Description = getProjectDTO.Project_Description,
                    Project_Created_By = getProjectDTO.Project_Created_By,
                    Project_Status = getProjectDTO.Project_Status,
                    Project_Created_At = getProjectDTO.Project_Created_At,
                    Project_Version = getProjectDTO.Project_Version
                };

                int result = project.GetProjectDB();
                if (result > 0)
                {
                    return Ok(new { success = true, message = "Project Fetched Successfully" });

                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to Fetch Project" });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}

