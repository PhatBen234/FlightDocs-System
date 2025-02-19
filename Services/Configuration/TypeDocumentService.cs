using FlightDocs_System.Data;
using FlightDocs_System.ViewModels.AllDocuments;
using FlightDocs_System.ViewModels.AllFights;
using FlightDocs_System.ViewModels.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Security;
using System.Threading;

namespace FlightDocs_System.Services.Configuration
{
    public class TypeDocumentService : ITypeDocumentService
    {
        private readonly ApplicationDbContext _context;

        public TypeDocumentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TypeDocumentViewModel2>> GetAllTypeDocumentsAsync()
        {
            var typeDocuments = await _context.TypeDocument
                .Include(td => td.User)
                .Select(td => new TypeDocumentViewModel2
                {
                    ID_TypeDocument = td.ID_TypeDocument,
                    TypeDocumentName = td.TypeDocumentName,
                    TypeDocumentNote = td.TypeDocumentNote,
                    DateCreated = td.DateCreated,
                    UserId = td.UserId,
                    User = new UserViewModel
                    {
                        FullName = td.User.FullName,
                        Email = td.User.Phone
                    },
                    Permission = _context.TypeDocument_Group
                        .Count(tdg => tdg.ID_TypeDocument == td.ID_TypeDocument), // Số lượng ID_TypeDocument trong TypeDocument_Group
                    TypeDocument_Groups = _context.TypeDocument_Group
                        .Where(tdg => tdg.ID_TypeDocument == td.ID_TypeDocument) // Lọc theo ID_TypeDocument
                        .Select(tdg => new TypeDocument_GroupViewModel
                        {
                            ID_GroupPermission = tdg.ID_GroupPermission,
                            GroupPermissionName = tdg.GroupPermission.GroupPermissionName,
                            IsReadModify = tdg.IsReadModify,
                            IsRead = tdg.IsRead,
                            NoPermission = tdg.NoPermission
                        })
                        .ToList() // Chuyển sang danh sách
                })
                .ToListAsync();

            return typeDocuments;
        }
        public async Task<List<TypeDocumentViewModel2>> GetDocumentsByTypeAsync(int idTypeDocument)
        {
            var typeDocuments = await _context.TypeDocument
                .Where(td => td.ID_TypeDocument == idTypeDocument)
                .Select(td => new TypeDocumentViewModel2
                {
                    ID_TypeDocument = td.ID_TypeDocument,
                    TypeDocumentName = td.TypeDocumentName,
                    TypeDocumentNote = td.TypeDocumentNote,
                    DateCreated = td.DateCreated,
                    Permission = td.Permission,
                    UserId = td.UserId,
                    User = new UserViewModel
                    {
                        FullName = td.User.FullName,
                        Email = td.User.Email
                    },
                    TypeDocument_Groups = _context.TypeDocument_Group
                        .Where(tdg => tdg.ID_TypeDocument == idTypeDocument) // Lọc theo ID_TypeDocument
                        .Select(tdg => new TypeDocument_GroupViewModel
                        {
                            ID_GroupPermission = tdg.ID_GroupPermission,
                            GroupPermissionName = tdg.GroupPermission.GroupPermissionName,
                            IsReadModify = tdg.IsReadModify,
                            IsRead = tdg.IsRead,
                            NoPermission = tdg.NoPermission
                        })
                        .ToList() // Chuyển sang danh sách
                })
                .ToListAsync();

            return typeDocuments;
        }

        public async Task<bool> AddTypeDocumentAsync(AddTypeDocumentViewModel model)
        {
            var typeDocument = new Data.TypeDocument
            {
                TypeDocumentName = model.TypeDocumentName,
                TypeDocumentNote = model.TypeDocumentNote,
                DateCreated = DateTime.Now,
                Permission = 0,
                UserId = model.UserId,
                // Các trường khác liên quan đến TypeDocument có thể được thêm vào ở đây
            };

            _context.TypeDocument.Add(typeDocument);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        public async Task<bool> UpdateTypeDocumentAsync(int typeDocumentId, UpdateTypeDocumentViewModel model)
        {
            var typeDocument = await _context.TypeDocument.FindAsync(typeDocumentId);
            if (typeDocument == null)
            {
                return false; // Không tìm thấy TypeDocument
            }

            // Cập nhật thông tin từ model vào typeDocument
            typeDocument.TypeDocumentName = model.TypeDocumentName;
            typeDocument.TypeDocumentNote = model.TypeDocumentNote;
            typeDocument.UserId = model.UserId;
            // Cập nhật các trường khác nếu cần

            _context.TypeDocument.Update(typeDocument);
            var result = await _context.SaveChangesAsync();


            // Add data to TypeDocument_Group table
            if (model.Permissions != null && model.Permissions.Any())
            {
                foreach (var permission in model.Permissions)
                {
                    var typeDocumentGroup = new TypeDocument_Group
                    {
                        ID_TypeDocument = typeDocumentId,
                        ID_GroupPermission = permission.ID_GroupPermission,
                        IsReadModify = permission.IsReadModify,
                        IsRead = permission.IsRead,
                        NoPermission = permission.NoPermission
                    };

                    _context.TypeDocument_Group.Add(typeDocumentGroup);
                }

                var typeDocumentGroupResult = await _context.SaveChangesAsync();

                return typeDocumentGroupResult > 0;
            }
            return result > 0;
        }

        public async Task<bool> DeleteTypeDocumentAsync(int typeDocumentId)
        {
            // Tìm loại tài liệu cần xóa
            var typeDocument = await _context.TypeDocument.FindAsync(typeDocumentId);

            // Nếu không tìm thấy, trả về false
            if (typeDocument == null)
            {
                return false;
            }

            try
            {
                // Xóa tất cả các bản ghi liên quan trong bảng TypeDocument_Group
                var typeDocumentGroups = _context.TypeDocument_Group.Where(tdg => tdg.ID_TypeDocument == typeDocumentId);
                _context.TypeDocument_Group.RemoveRange(typeDocumentGroups);

                // Xóa loại tài liệu
                _context.TypeDocument.Remove(typeDocument);

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Trả về true nếu xóa thành công
                return true;
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi xảy ra trong quá trình xóa
                // Có thể ghi log, thông báo lỗi, hoặc xử lý theo cách khác tùy thuộc vào yêu cầu của bạn
                Console.WriteLine($"Error deleting TypeDocument: {ex.Message}");
                return false;
            }
        }

    }
}
