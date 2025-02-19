using FlightDocs_System.ViewModels.AllFights;
using FlightDocs_System.ViewModels.Configuration;

namespace FlightDocs_System.ViewModels.AllDocuments
{
    public class FlightDocumentViewModel
    {
        public int ID_Document { get; set; }
        public string? DocumentName { get; set; }
        public string? FlightDocumentNote { get; set; }
        public DateTime DateCreated { get; set; }
        public string? File { get; set; }
        public decimal Version { get; set; }
        public FlightViewModel? Flight { get; set; }
        public TypeDocumentViewModel? TypeDocument { get; set; }
        public UserViewModel? User { get; set; }
        public List<Document_GroupViewModel>? Document_Groups { get; set; }
        public List<TypeDocument_GroupViewModel>? TypeDocument_Groups { get; set; }
    }
    public class TypeDocumentViewModel
    {
        public string? TypeDocumentName { get; set; }
        public string? TypeDocumentNote { get; set; }
    }
    public class UserViewModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
    
    public class TypeDocument_GroupViewModel2
    {
        public int ID_GroupPermission { get; set; }
        public bool IsReadModify { get; set; }
        public bool IsRead { get; set; }
        public bool NoPermission { get; set; }
    }



}
