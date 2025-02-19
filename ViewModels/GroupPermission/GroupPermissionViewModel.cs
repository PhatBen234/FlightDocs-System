using FlightDocs_System.ViewModels.AllDocuments;

namespace FlightDocs_System.ViewModels.GroupPermission
{
    public class GroupPermissionViewModel
    {
        public int ID_GroupPermission { get; set; }
        public string? GroupPermissionName { get; set; }
        public string? GroupPermissionNote { get; set; }
        public int Member { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserId { get; set; }
        public UserViewModel? User { get; set; }
        public List<UserViewModel>? RelatedUserIds { get; set; } // Thêm trường này
    }

}
