using FlightDocs_System.Helpers;
using FlightDocs_System.Services.GroupPermission;
using FlightDocs_System.ViewModels.GroupPermission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocs_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserClasses.Role_Owner)]
    public class GroupPermissionController : ControllerBase
    {
        private readonly IGroupPermissionService _groupPermissionService;

        public GroupPermissionController(IGroupPermissionService groupPermissionService)
        {
            _groupPermissionService = groupPermissionService;
        }

        [HttpGet("getAllGroupPermissions")]
        public async Task<IActionResult> GetAllGroupPermissions()
        {
            try
            {
                var groupPermissions = await _groupPermissionService.GetAllGroupPermissionsAsync();
                return Ok(groupPermissions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Đã xảy ra lỗi khi lấy thông tin tất cả các quyền nhóm.", ErrorMessage = ex.Message });
            }
        }
        [HttpPost("addGroupPermission")]
        public async Task<IActionResult> AddGroupPermission([FromBody] AddGroupPermissionViewModel model)
        {
            var result = await _groupPermissionService.AddGroupPermissionAsync(model);

            if (result)
            {
                return Ok(new { Message = "Quyền nhóm đã được thêm thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể thêm quyền nhóm" });
            }
        }
        [HttpPut("updateGroupPermission")]
        public async Task<IActionResult> UpdateGroupPermission([FromBody] UpdateGroupPermissionViewModel model)
        {
            var result = await _groupPermissionService.UpdateGroupPermissionAsync(model);
            if (result)
            {
                return Ok(new { Message = "GroupPermission updated successfully!" });
            }
            else
            {
                return BadRequest(new { Error = "Failed to update GroupPermission." });
            }
        }

        [HttpDelete("deleteGroupPermission/{id}")]
        public async Task<IActionResult> DeleteGroupPermission(int id)
        {
            var result = await _groupPermissionService.DeleteGroupPermissionAsync(id);

            if (result)
            {
                return Ok(new { Message = "Group permission has been deleted successfully!" });
            }
            else
            {
                return BadRequest(new { Error = "Failed to delete group permission" });
            }
        }


    }
}
