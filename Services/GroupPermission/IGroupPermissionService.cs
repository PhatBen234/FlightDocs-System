using FlightDocs_System.ViewModels.GroupPermission;

namespace FlightDocs_System.Services.GroupPermission
{
    public interface IGroupPermissionService
    {
        Task<List<GroupPermissionViewModel>> GetAllGroupPermissionsAsync();
        Task<bool> AddGroupPermissionAsync(AddGroupPermissionViewModel model);
        Task<bool> UpdateGroupPermissionAsync(UpdateGroupPermissionViewModel model);
        Task<bool> DeleteGroupPermissionAsync(int id);

    }
}
