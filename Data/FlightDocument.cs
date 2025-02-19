using System.ComponentModel.DataAnnotations;

namespace FlightDocs_System.Data
{
    public class FlightDocument
    {
        [Key]
        public int ID_Document { get; set; }
        public string? DocumentName { get; set; }
        public string? FlightDocumentNote { get; set; }
        public DateTime DateCreated { get; set; }
        public string? File { get; set; }
        public decimal Version { get; set; }
        public int ID_Flight { get; set; }
        public Flight? Flight { get; set; }
        public int ID_TypeDocument { get; set; }
        public TypeDocument? TypeDocument { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }

}
