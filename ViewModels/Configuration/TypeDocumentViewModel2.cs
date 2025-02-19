using FlightDocs_System.ViewModels.AllDocuments;

namespace FlightDocs_System.ViewModels.Configuration
{
    public class TypeDocumentViewModel2
    {
        public int ID_TypeDocument { get; set; }
        public string? TypeDocumentName { get; set; }
        public string? TypeDocumentNote { get; set; }
        public DateTime DateCreated { get; set; }
        public int Permission { get; set; }
        public string? UserId { get; set; }
        public UserViewModel? User { get; set; }
        public List<TypeDocument_GroupViewModel>? TypeDocument_Groups { get; set; } 
    }

    public class TypeDocument_GroupViewModel
    {
        public int ID_GroupPermission { get; set; }
        public string? GroupPermissionName { get; set; }
        public bool IsReadModify { get; set; }
        public bool IsRead { get; set; }
        public bool NoPermission { get; set; }
    }




}
