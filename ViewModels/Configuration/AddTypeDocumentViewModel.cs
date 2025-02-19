using FlightDocs_System.ViewModels.AllDocuments;

namespace FlightDocs_System.ViewModels.Configuration
{
    public class AddTypeDocumentViewModel
    {
        public string? TypeDocumentName { get; set; }
        public string? TypeDocumentNote { get; set; }
        /*public int Permission { get; set; }*/
        public string? UserId { get; set; }

        // Bổ sung các trường khác liên quan đến TypeDocument ở đây
    }

}
