using FlightDocs_System.Data;
using FlightDocs_System.ViewModels.AllDocuments;
using FlightDocs_System.ViewModels.AllFights;
using FlightDocs_System.ViewModels.Configuration;

namespace FlightDocs_System.Services.Configuration
{
    public interface ITypeDocumentService
    {
        Task<bool> AddTypeDocumentAsync(AddTypeDocumentViewModel model);
        Task<List<TypeDocumentViewModel2>> GetDocumentsByTypeAsync(int idTypeDocument);
        Task<List<TypeDocumentViewModel2>> GetAllTypeDocumentsAsync();
        Task<bool> UpdateTypeDocumentAsync(int typeDocumentId, UpdateTypeDocumentViewModel model);
        Task<bool> DeleteTypeDocumentAsync(int typeDocumentId);
    }
}
