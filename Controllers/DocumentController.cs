using FlightDocs_System.Services.AllDocuments;
using FlightDocs_System.ViewModels.AllDocuments;
using FlightDocs_System.ViewModels.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightDocs_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("getAllFlightDocuments")]
        public async Task<IActionResult> GetAllFlightDocuments()
        {
            try
            {
                // Kiểm tra quyền của người dùng
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null || !await _documentService.HasPermission(userId))
                {
                    return Forbid();
                }

                var flightDocuments = await _documentService.GetAllFlightDocumentsAsync();
                return Ok(flightDocuments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Đã xảy ra lỗi khi lấy danh sách tất cả các tài liệu chuyến bay.", ErrorMessage = ex.Message });
            }
        }

        [HttpGet("getFlightDocumentById")]
        public async Task<IActionResult> GetFlightDocumentById(int documentId)
        {
            // Kiểm tra quyền của người dùng
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !await _documentService.HasPermission(userId))
            {
                return Forbid();
            }

            var flightDocument = await _documentService.GetFlightDocumentByIdAsync(documentId);
            if (flightDocument == null)
            {
                return NotFound(new { Error = $"Không tìm thấy tài liệu với ID {documentId}" });
            }
            return Ok(flightDocument);
        }

        [HttpPost("createDocument")]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentViewModel model)
        {
            // Kiểm tra quyền của người dùng
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !await _documentService.HasPermission2(userId))
            {
                return Forbid();
            }

            var result = await _documentService.CreateDocumentAsync(model);
            if (result)
            {
                return Ok(new { Message = "Tài liệu đã được tạo thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể tạo tài liệu" });
            }
        }

        [HttpPut("UpdateFlightDocument/{id}")]
        public async Task<IActionResult> UpdateFlightDocument(int id, [FromBody] UpdateDocumentViewModel model)
        {

            // Kiểm tra quyền của người dùng
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !await _documentService.HasPermission2(userId))
            {
                return Forbid();
            }
            var result = await _documentService.UpdateFlightDocumentAsync(id, model);

            if (result)
            {
                return Ok(new { Message = "Cập nhật thông tin tài liệu chuyến bay thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể cập nhật tài liệu chuyến bay" });
            }
        }

        [HttpDelete("DeleteFlightDocument/{id}")]
        public async Task<IActionResult> DeleteFlightDocument(int id)
        {
            // Kiểm tra quyền của người dùng
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !await _documentService.HasPermission2(userId))
            {
                return Forbid();
            }

            var result = await _documentService.DeleteFlightDocumentAsync(id);

            if (result)
            {
                return Ok(new { Message = "Xóa tài liệu chuyến bay thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể xóa tài liệu chuyến bay" });
            }
        }


    }
}
