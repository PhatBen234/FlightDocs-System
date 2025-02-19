using FlightDocs_System.Helpers;
using FlightDocs_System.Services.Decentralizations;
using FlightDocs_System.ViewModels.Decentralizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocs_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DecentralizationController : ControllerBase
    {
        private readonly IDecentralizationService _decentralizationService;

        public DecentralizationController(IDecentralizationService decentralizationService)
        {
            _decentralizationService = decentralizationService;
        }

        [HttpGet("usersWithRoles")]
        public async Task<ActionResult<List<UserViewModel2>>> GetUsersWithRoles()
        {
            try
            {
                var usersWithRoles = await _decentralizationService.GetUsersWithRolesAsync();
                return Ok(usersWithRoles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("claimsForRole")]
        [Authorize(Roles = UserClasses.Role_Owner)]
        public async Task<ActionResult<List<ClaimModel>>> GetClaimsForRole(string roleName)
        {
            try
            {
                var claimsForRole = await _decentralizationService.GetClaimsForRoleAsync(roleName);
                return Ok(claimsForRole);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("addClaimToRole")]
        [Authorize(Roles = UserClasses.Role_Owner)]
        public async Task<ActionResult<string>> AddClaimToRole([FromBody] AddClaimToRoleRequest request)
        {
            try
            {
                var result = await _decentralizationService.AddClaimToRoleAsync(request.RoleName, request.ClaimType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("update-role-claim")]
        [Authorize(Roles = UserClasses.Role_Owner)]
        public async Task<IActionResult> UpdateRoleClaim([FromBody] UpdateRoleClaimModel model)
        {
            try
            {
                var result = await _decentralizationService.UpdateRoleClaimAsync(model.RoleName, model.ClaimType, model.NewClaimValue);

                if (result == "Role claim updated successfully.")
                {
                    return Ok(new { Message = result });
                }
                else
                {
                    return BadRequest(new { Error = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Internal Server Error: {ex.Message}" });
            }
        }
    }
}
