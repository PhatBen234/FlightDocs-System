using System.ComponentModel.DataAnnotations;

namespace FlightDocs_System.Data
{
    public class GroupPermission_User
    {
        [Key]
        public int ID_GroupPermission_User { get; set; }
        public int ID_GroupPermission { get; set; }
        public GroupPermission? GroupPermission { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
