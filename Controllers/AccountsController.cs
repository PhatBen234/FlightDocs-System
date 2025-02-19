using FlightDocs_System.Helpers;
using FlightDocs_System.Services.LoginLogout;
using FlightDocs_System.ViewModels.LoginLogout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocs_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AccountsController : ControllerBase
    {
        private readonly IAccountServices accountRepository;
        private readonly ILogger<AccountsController> logger;

        public AccountsController(IAccountServices accountRepository, ILogger<AccountsController> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpAdmin model)
        {
            if (ModelState.IsValid)
            {
                var result = await accountRepository.SignUpAsync(model);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Đăng ký thành công!" });
                }
                else
                {
                    return BadRequest(new { Error = "Đăng ký không thành công", Errors = result.Errors });
                }
            }
            return BadRequest(new { Error = "Dữ liệu không hợp lệ" });
        }
        [HttpPost("signuppilot")]
        [Authorize(Roles = UserClasses.Role_Pilot)]
        public async Task<IActionResult> SignUpPilot([FromBody] SignUpAdmin model)
        {
            if (ModelState.IsValid)
            {
                var result = await accountRepository.SignUpPilotAsync(model);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Đăng ký thành công!" });
                }
                else
                {
                    return BadRequest(new { Error = "Đăng ký không thành công", Errors = result.Errors });
                }
            }
            return BadRequest(new { Error = "Dữ liệu không hợp lệ" });
        }
       

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var token = await accountRepository.SignInAsync(signInModel);
                if (!string.IsNullOrEmpty(token))
                {
                    return Ok(new { Token = token });
                }
                else
                {
                    return Unauthorized(new { Error = "Đăng nhập không thành công" });
                }
            }
            return BadRequest(new { Error = "Dữ liệu không hợp lệ" });
        }

        [HttpPost("assignOwnerRole")]
        [Authorize(Roles = UserClasses.Role_Owner)]
        public async Task<IActionResult> AssignOwnerRole(string currentUserId, string newOwnerId)
        {
            try
            {
                var result = await accountRepository.AssignOwnerRoleAsync(currentUserId, newOwnerId);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Gán quyền Owner thành công!" });
                }
                else
                {
                    return BadRequest(new { Error = "Không thể gán quyền Owner", Errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { Error = "Đã xảy ra lỗi khi gán quyền Owner" });
            }
        }

        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> SignOutAsync()
        {
            try
            {
                await accountRepository.SignOutAsync();
                return Ok(new { Message = "Đăng xuất thành công!" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { Error = "Đăng xuất mém thành công" });
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = UserClasses.Role_Owner)]

        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await accountRepository.UpdateUserAsync(userId, model);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Cập nhật thông tin người dùng thành công!" });
                }
                else
                {
                    return BadRequest(new { Error = "Cập nhật thông tin người dùng không thành công", Errors = result.Errors });
                }
            }
            return BadRequest(new { Error = "Dữ liệu không hợp lệ" });
        }

        [HttpDelete("delete")]
        [Authorize(Roles = UserClasses.Role_Owner)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var result = await accountRepository.DeleteUserAsync(userId);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Xóa người dùng thành công!" });
                }
                else
                {
                    return BadRequest(new { Error = "Xóa người dùng không thành công", Errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { Error = "Đã xảy ra lỗi khi xóa người dùng" });
            }
        }

    }
}
