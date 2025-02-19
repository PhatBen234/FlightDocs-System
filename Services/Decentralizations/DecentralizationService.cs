using FlightDocs_System.Data;
using FlightDocs_System.ViewModels.AllDocuments;
using FlightDocs_System.ViewModels.Decentralizations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlightDocs_System.Services.Decentralizations
{
    public class DecentralizationService : IDecentralizationService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DecentralizationService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        //lay
        public async Task<List<UserViewModel2>> GetUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel2>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var userViewModel = new UserViewModel2
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.PasswordHash,
                    Roles = userRoles.ToList()
                };

                userViewModels.Add(userViewModel);
            }

            return userViewModels;
        }
        //lay
        public async Task<List<ClaimModel>> GetClaimsForRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _context.RoleClaims
                    .Where(rc => rc.RoleId == role.Id)
                    .Select(rc => new ClaimModel { ClaimType = rc.ClaimType, ClaimValue = rc.ClaimValue })
                    .ToListAsync();

                return roleClaims;
            }

            return new List<ClaimModel>();
        }
        //lay
        public async Task<string> AddClaimToRoleAsync(string roleName, string claimType)
        {
            try
            {
                // Kiểm tra và tạo Role nếu cần
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var newRole = new IdentityRole(roleName);
                    var createRoleResult = await _roleManager.CreateAsync(newRole);
                    if (!createRoleResult.Succeeded)
                    {
                        // Xử lý khi tạo Role thất bại
                        return "Failed to create Role.";
                    }
                }

                // Kiểm tra và thêm Claim
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var existingClaims = await _roleManager.GetClaimsAsync(role);
                    if (existingClaims.Any(c => c.Type == claimType))
                    {
                        // Xử lý khi Claim đã tồn tại
                        return "Claim already exists for the Role.";
                    }

                    var claim = new Claim(claimType, "true");
                    var result = await _roleManager.AddClaimAsync(role, claim);

                    if (result.Succeeded)
                    {
                        // Xử lý khi thêm Claim thành công
                        return "Claim added successfully.";
                    }
                }

                // Xử lý khi thêm Claim thất bại hoặc Role không tồn tại
                return "Failed to add Claim to Role.";
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return $"Error: {ex.Message}";
            }
        }
        public async Task<string> UpdateRoleClaimAsync(string roleName, string claimType, bool newClaimValue)
        {
            // Kiểm tra xem vai trò có tồn tại không
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                // Lấy ID của vai trò
                var roleId = role.Id;

                // Lấy danh sách claims của vai trò từ bảng AspNetRoleClaims
                var roleClaims = await _context.RoleClaims
                    .Where(rc => rc.RoleId == roleId && rc.ClaimType == claimType)
                    .ToListAsync();

                // Kiểm tra xem có quyền cần sửa không
                if (roleClaims.Any())
                {
                    // Lấy RoleClaim cần sửa
                    var roleClaim = roleClaims.FirstOrDefault();

                    // Cập nhật giá trị ClaimValue
                    roleClaim.ClaimValue = newClaimValue.ToString();

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    return "Role claim updated successfully.";
                }
                else
                {
                    return "Role claim not found for the specified role.";
                }
            }
            else
            {
                return "Role not found.";
            }
        }
    }
}
