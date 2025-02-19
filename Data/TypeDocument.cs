using System.ComponentModel.DataAnnotations;

namespace FlightDocs_System.Data
{
    public class TypeDocument
    {
        [Key]
        public int ID_TypeDocument { get; set; }
        public string? TypeDocumentName { get; set; }
        public string? TypeDocumentNote { get; set; }
        public DateTime DateCreated { get; set; }
        public int Permission { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
