using FlightDocs_System.ViewModels.Decentralizations;

namespace FlightDocs_System.Services.Decentralizations
{
    public interface IDecentralizationService
    {
        Task<List<UserViewModel2>> GetUsersWithRolesAsync();
        Task<List<ClaimModel>> GetClaimsForRoleAsync(string roleName);
        Task<string> AddClaimToRoleAsync(string roleName, string claimType);
        Task<string> UpdateRoleClaimAsync(string roleName, string claimType, bool newClaimValue);
    }
}
