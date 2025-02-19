using System.ComponentModel.DataAnnotations;

namespace FlightDocs_System.Data
{
    public class Document_Group
    {
        [Key]
        public int ID_Document_Group { get; set; }
        public int ID_Document { get; set; }
        public FlightDocument? FlightDocument { get; set; }
        public int ID_GroupPermission { get; set; }
        public GroupPermission? GroupPermission { get; set; }
    }
}
