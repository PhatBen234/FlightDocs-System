using FlightDocs_System.Data;
using FlightDocs_System.ViewModels.AllFights;
using Microsoft.EntityFrameworkCore;

namespace FlightDocs_System.Services.AllFights
{
    public class FlightService : IFlightService
    {
        private readonly ApplicationDbContext _context;

        public FlightService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FlightViewModel>> GetFlightsWithDocumentsAsync()
        {
            var flightsWithDocuments = await _context.Flights
                .Select(f => new FlightViewModel
                {
                    ID_Flight = f.ID_Flight,
                    FlightNumber = f.FlightNo,
                    Route = f.Rouse,
                    DepartureDate = f.DepartureDate,
                    DocumentCount = _context.FlightDocuments.Count(fd => fd.ID_Flight == f.ID_Flight),
                    Documents = _context.FlightDocuments
                        .Where(fd => fd.ID_Flight == f.ID_Flight)
                        .Select(fd => new DocumentViewModel
                        {
                            DocumentName = fd.DocumentName,
                            DocumentType = fd.TypeDocument.TypeDocumentName,
                            DateCreated = fd.DateCreated,
                            Creator = fd.User.FullName,
                            LatestVersion = fd.Version,
                        })
                        .ToList()
                })
                .ToListAsync();

            return flightsWithDocuments;
        }

        public async Task<FlightViewModel> GetFlightWithDocumentsAsync(int flightId)
        {
            var flightWithDocuments = await _context.Flights
                .Where(f => f.ID_Flight == flightId)
                .Select(f => new FlightViewModel
                {
                    ID_Flight = f.ID_Flight,
                    FlightNumber = f.FlightNo,
                    Route = f.Rouse,
                    DepartureDate = f.DepartureDate,
                    DocumentCount = _context.FlightDocuments.Count(fd => fd.ID_Flight == f.ID_Flight),
                    Documents = _context.FlightDocuments
                        .Where(fd => fd.ID_Flight == f.ID_Flight)
                        .Select(fd => new DocumentViewModel
                        {
                            DocumentName = fd.DocumentName,
                            DocumentType = fd.TypeDocument.TypeDocumentName,
                            DateCreated = fd.DateCreated,
                            Creator = fd.User.FullName,
                            LatestVersion = fd.Version,
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return flightWithDocuments;
        }
        public async Task<bool> AddFlightAsync(AddFlightViewModel model)
        {
            var newFlight = new Data.Flight
            {
                FlightNo = model.FlightNo,
                Rouse = model.Route,
                DepartureDate = model.DepartureDate,
                Point_Of_Loading = model.PointOfLoading,
                Point_Of_Unloading = model.PointOfUnloading,
                Total_Documents = 0,

            };

            _context.Flights.Add(newFlight);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateFlightAsync(int flightId, UpdateFlightViewModel model)
        {
            // Tìm kiếm máy bay cần cập nhật thông tin dựa vào flightId
            var flightToUpdate = await _context.Flights.FindAsync(flightId);

            // Kiểm tra nếu không tìm thấy máy bay
            if (flightToUpdate == null)
            {
                return false; // Hoặc thực hiện xử lý khác tùy thuộc vào yêu cầu của ứng dụng
            }

            // Cập nhật thông tin của máy bay
            flightToUpdate.FlightNo = model.FlightNo;
            flightToUpdate.Rouse = model.Route;
            flightToUpdate.DepartureDate = model.DepartureDate;
            flightToUpdate.Point_Of_Loading = model.PointOfLoading;
            flightToUpdate.Point_Of_Unloading = model.PointOfUnloading;

            // Lưu các thay đổi vào cơ sở dữ liệu
            var result = await _context.SaveChangesAsync();

            return result > 0; // Trả về true nếu có ít nhất một bản ghi được cập nhật thành công
        }
        public async Task<bool> DeleteFlightAsync(int flightId)
        {
            var flight = await _context.Flights.FindAsync(flightId);
            if (flight == null)
            {
                return false;
            }

            _context.Flights.Remove(flight);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }


    }
}
