using FlightDocs_System.Data;
using FlightDocs_System.Helpers;
using FlightDocs_System.ViewModels.LoginLogout;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightDocs_System.Services.LoginLogout
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AccountServices> logger;
        private readonly ApplicationDbContext context;

        public AccountServices(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            ILogger<AccountServices> logger,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.logger = logger;
            this.context = context;
        }
        public async Task<IdentityResult> AssignOwnerRoleAsync(string currentUserId, string newOwnerId)
        {
            var currentUser = await userManager.FindByIdAsync(currentUserId);
            var newOwner = await userManager.FindByIdAsync(newOwnerId);

            // Kiểm tra xem tài khoản hiện tại có quyền Owner không
            if (!await userManager.IsInRoleAsync(currentUser, UserClasses.Role_Owner))
            {
                // Trả về kết quả không thành công nếu không có quyền Owner
                return IdentityResult.Failed(new IdentityError { Description = "Bạn không có quyền thực hiện thao tác này." });
            }

            // Gỡ bỏ quyền Owner từ tài khoản hiện tại
            await userManager.RemoveFromRoleAsync(currentUser, UserClasses.Role_Owner);

            // Gán quyền Owner cho tài khoản mới
            await userManager.AddToRoleAsync(newOwner, UserClasses.Role_Owner);

            // Trả về kết quả thành công
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpAdmin model)
        {
            // Tạo một đối tượng ApplicationUser từ dữ liệu đăng ký
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                Phone = model.Phone
            };

            // Thực hiện đăng ký người dùng
            var result = await userManager.CreateAsync(user, model.Password);

            // Nếu đăng ký thành công
            if (result.Succeeded)
            {
                // Kiểm tra và thêm vai trò Admin nếu chưa có
                if (!await roleManager.RoleExistsAsync(UserClasses.Role_Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserClasses.Role_Admin));
                }

                // Thêm người dùng vào vai trò Admin
                await userManager.AddToRoleAsync(user, UserClasses.Role_Admin);
            }

            // Trả về kết quả của quá trình đăng ký
            return result;
        }

        public async Task<IdentityResult> SignUpPilotAsync(SignUpAdmin model)
        {
            // Tạo một đối tượng ApplicationUser từ dữ liệu đăng ký
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                Phone = model.Phone
            };

            // Thực hiện đăng ký người dùng
            var result = await userManager.CreateAsync(user, model.Password);

            // Nếu đăng ký thành công
            if (result.Succeeded)
            {
                // Kiểm tra và thêm vai trò Admin nếu chưa có
                if (!await roleManager.RoleExistsAsync(UserClasses.Role_Pilot))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserClasses.Role_Pilot));
                }

                // Thêm người dùng vào vai trò Admin
                await userManager.AddToRoleAsync(user, UserClasses.Role_Pilot);
            }

            // Trả về kết quả của quá trình đăng ký
            return result;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !passwordValid)
            {
                var errorMessage = "Đăng nhập không thành công. Tên người dùng hoặc mật khẩu không đúng.";
                return $"{{\"Error\": \"{errorMessage}\"}}";
            }


            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id) // Thêm claim ID ở đây
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Trả về kết quả không thành công nếu không tìm thấy người dùng
                return IdentityResult.Failed(new IdentityError { Description = "Người dùng không tồn tại." });
            }

            // Xóa người dùng khỏi hệ thống
            var result = await userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserModel model)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Trả về kết quả không thành công nếu không tìm thấy người dùng
                return IdentityResult.Failed(new IdentityError { Description = "Người dùng không tồn tại." });
            }

            // Cập nhật thông tin người dùng
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;

            var result = await userManager.UpdateAsync(user);

            return result;
        }

    }
}