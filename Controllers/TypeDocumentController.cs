using FlightDocs_System.Data;
using FlightDocs_System.Services.Configuration;
using FlightDocs_System.ViewModels.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocs_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserClasses.Role_Owner)]
    public class TypeDocumentController : ControllerBase
    {
        private readonly ITypeDocumentService _typeDocumentService;

        public TypeDocumentController(ITypeDocumentService typeDocumentService)
        {
            _typeDocumentService = typeDocumentService;
        }
        [HttpGet("getAllTypeDocuments")]
        public async Task<IActionResult> GetAllTypeDocuments()
        {
            try
            {
                var typeDocuments = await _typeDocumentService.GetAllTypeDocumentsAsync();
                return Ok(typeDocuments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Đã xảy ra lỗi khi lấy danh sách tất cả các loại tài liệu.", ErrorMessage = ex.Message });
            }
        }
        [HttpGet("getTypeDocumentsByType")]
        public async Task<IActionResult> GetDocumentsByType([FromQuery] int idTypeDocument)
        {
            var documents = await _typeDocumentService.GetDocumentsByTypeAsync(idTypeDocument);
            return Ok(documents);
        }
        [HttpPost("addTypeDocument")]
        public async Task<IActionResult> AddTypeDocument([FromBody] AddTypeDocumentViewModel model)
        {
            var result = await _typeDocumentService.AddTypeDocumentAsync(model);

            if (result)
            {
                return Ok(new { Message = "Loại tài liệu đã được thêm thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể thêm loại tài liệu" });
            }
        }
        [HttpPut("updateTypeDocument/{id}")]
        public async Task<IActionResult> UpdateTypeDocument(int id, [FromBody] UpdateTypeDocumentViewModel model)
        {
            var result = await _typeDocumentService.UpdateTypeDocumentAsync(id, model);

            if (result)
            {
                return Ok(new { Message = "Loại tài liệu đã được cập nhật thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể cập nhật loại tài liệu" });
            }
        }

        [HttpDelete("deleteTypeDocument/{id}")]
        public async Task<IActionResult> DeleteTypeDocument(int id)
        {
            var result = await _typeDocumentService.DeleteTypeDocumentAsync(id);

            if (result)
            {
                return Ok(new { Message = "Loại tài liệu đã được xóa thành công!" });
            }
            else
            {
                return BadRequest(new { Error = "Không thể xóa loại tài liệu" });
            }
        }

    }
}
