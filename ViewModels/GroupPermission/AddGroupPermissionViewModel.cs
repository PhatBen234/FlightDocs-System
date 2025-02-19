namespace FlightDocs_System.ViewModels.GroupPermission
{
    public class AddGroupPermissionViewModel
    {
        public string? GroupPermissionName { get; set; }
        public string? GroupPermissionNote { get; set; }
        public int Member { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserId { get; set; }
    }

}
