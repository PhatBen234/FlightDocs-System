namespace FlightDocs_System.ViewModels.AllDocuments
{
    public class UpdateDocumentViewModel
    {
        public string? DocumentName { get; set; }
        public string? FlightDocumentNote { get; set; }
        public string? File { get; set; }
        public int ID_Flight { get; set; }
        public int ID_TypeDocument { get; set; }
        public string? UserId { get; set; }
        public List<int>? ID_GroupPermissions { get; set; }
    }
}
