using FlightDocs_System.ViewModels.LoginLogout;
using Microsoft.AspNetCore.Identity;

namespace FlightDocs_System.Services.LoginLogout
{
    public interface IAccountServices
    {
        Task<IdentityResult> SignUpAsync(SignUpAdmin model);
        Task<string> SignInAsync(SignInModel model);
        Task<IdentityResult> SignUpPilotAsync(SignUpAdmin model);
        Task SignOutAsync();
        Task<IdentityResult> AssignOwnerRoleAsync(string currentUserId, string newOwnerId);
        Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserModel model);
        Task<IdentityResult> DeleteUserAsync(string userId);
    }
}
