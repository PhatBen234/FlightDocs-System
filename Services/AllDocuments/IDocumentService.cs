using FlightDocs_System.ViewModels.AllDocuments;
using FlightDocs_System.ViewModels.Configuration;

namespace FlightDocs_System.Services.AllDocuments
{
    public interface IDocumentService
    {
        Task<List<FlightDocumentViewModel>> GetAllFlightDocumentsAsync();
        Task<FlightDocumentViewModel> GetFlightDocumentByIdAsync(int documentId);
        Task<bool> CreateDocumentAsync(CreateDocumentViewModel model);
        Task<bool> UpdateFlightDocumentAsync(int id, UpdateDocumentViewModel model);
        Task<bool> DeleteFlightDocumentAsync(int id);
        Task<bool> HasPermission(string userId);
        Task<bool> HasPermission2(string userId);

        

    }
}
