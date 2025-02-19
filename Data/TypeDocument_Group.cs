using System.ComponentModel.DataAnnotations;

namespace FlightDocs_System.Data
{
    public class TypeDocument_Group
    {
        [Key]
        public int ID_TypeDocument_Group { get; set; }
        public int ID_TypeDocument { get; set; }
        public TypeDocument? TypeDocument { get; set; }
        public int ID_GroupPermission { get; set; }
        public GroupPermission? GroupPermission { get; set; }
        public bool IsReadModify { get; set; }
        public bool IsRead { get; set; }
        public bool NoPermission { get; set; }
    }
}
