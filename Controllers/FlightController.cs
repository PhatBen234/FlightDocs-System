using FlightDocs_System.Helpers;
using FlightDocs_System.Services.AllFights;
using FlightDocs_System.ViewModels.AllFights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocs_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserClasses.Role_Owner)]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("getFlightsWithDocuments")]

        public async Task<IActionResult> GetFlightsWithDocuments()
        {
            var flightsWithDocuments = await _flightService.GetFlightsWithDocumentsAsync();
            return Ok(flightsWithDocuments);
        }

        [HttpGet("getFlightWithDocuments/{flightId}")]
        public async Task<IActionResult> GetFlightWithDocuments(int flightId)
        {
            var flightWithDocuments = await _flightService.GetFlightWithDocumentsAsync(flightId);
            if (flightWithDocuments == null)
            {
                return NotFound();
            }
            return Ok(flightWithDocuments);
        }
        [HttpPost("addFlight")]
        public async Task<IActionResult> AddFlight([FromBody] AddFlightViewModel model)
        {
            var result = await _flightService.AddFlightAsync(model);
            if (result)
            {
                return Ok(new { Message = "Chuyến bay đã được thêm thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể thêm chuyến bay" });
            }
        }

        [HttpPut("updateFlight/{flightId}")]
        public async Task<IActionResult> UpdateFlight(int flightId, [FromBody] UpdateFlightViewModel model)
        {
            var result = await _flightService.UpdateFlightAsync(flightId, model);
            if (result)
            {
                return Ok(new { Message = "Thông tin chuyến bay đã được cập nhật thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể cập nhật thông tin chuyến bay" });
            }
        }
        [HttpDelete("deleteFlight/{flightId}")]
        public async Task<IActionResult> DeleteFlight(int flightId)
        {
            var result = await _flightService.DeleteFlightAsync(flightId);
            if (result)
            {
                return Ok(new { Message = "Thông tin chuyến bay đã được xóa thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể xóa thông tin chuyến bay" });
            }
        }
    }
}
