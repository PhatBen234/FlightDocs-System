namespace FlightDocs_System.ViewModels.AllDocuments
{
    public class CreateDocumentViewModel
    {
        public string? DocumentName { get; set; }
        public string? FlightDocumentNote { get; set; }
        public int ID_Flight { get; set; }
        public int ID_TypeDocument { get; set; }
        public string? UserId { get; set; }
        public string? File { get; set;}
        public List<int>? ID_GroupPermissions { get; set; }
        // Các trường khác liên quan đến tài liệu có thể được thêm vào ở đây
    }

}
