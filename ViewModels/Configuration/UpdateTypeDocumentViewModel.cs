using FlightDocs_System.ViewModels.AllDocuments;

namespace FlightDocs_System.ViewModels.Configuration
{
    public class UpdateTypeDocumentViewModel
    {
        public string? TypeDocumentName { get; set; }
        public string? TypeDocumentNote { get; set; }
        public string? UserId { get; set; }
        public List<TypeDocumentPermissionViewModel>? Permissions { get; set; }
    }
    public class TypeDocumentPermissionViewModel
    {
        public int ID_GroupPermission { get; set; }
        public bool IsReadModify { get; set; }
        public bool IsRead { get; set; }
        public bool NoPermission { get; set; }
    }

}
