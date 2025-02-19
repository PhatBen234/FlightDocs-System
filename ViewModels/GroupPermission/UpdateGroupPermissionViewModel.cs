using FlightDocs_System.Data;

namespace FlightDocs_System.ViewModels.GroupPermission
{
    public class UpdateGroupPermissionViewModel
    {
        public int ID_GroupPermission { get; set; }
        public string? GroupPermissionName { get; set; }
        public string? GroupPermissionNote { get; set; }
        public string? UserId { get; set; }
        public List<string>? UserIds { get; set; } // Thêm trường này để chứa danh sách UserId

    }


}
