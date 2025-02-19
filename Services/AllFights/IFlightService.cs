using FlightDocs_System.Data;
using FlightDocs_System.ViewModels.AllFights;

namespace FlightDocs_System.Services.AllFights
{
    public interface IFlightService
    {
        Task<List<FlightViewModel>> GetFlightsWithDocumentsAsync();
        Task<FlightViewModel> GetFlightWithDocumentsAsync(int flightId);
        Task<bool> AddFlightAsync(AddFlightViewModel model);
        Task<bool> UpdateFlightAsync(int flightId, UpdateFlightViewModel model);
        Task<bool> DeleteFlightAsync(int flightId);
    }
}
