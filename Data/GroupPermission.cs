using System.ComponentModel.DataAnnotations;

namespace FlightDocs_System.Data
{
    public class GroupPermission
    {
        [Key]
        public int ID_GroupPermission { get; set; }
        public string? GroupPermissionName { get; set; }
        public string? GroupPermissionNote { get; set; }
        public int Member { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
