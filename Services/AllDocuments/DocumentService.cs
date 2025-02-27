using FlightDocs_System.Data;
using FlightDocs_System.ViewModels.AllDocuments;
using FlightDocs_System.ViewModels.AllFights;
using FlightDocs_System.ViewModels.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightDocs_System.Services.AllDocuments
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocumentService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> HasPermission(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return false;
            }

            // Kiểm tra vai trò Owner
            var isOwner = await _userManager.IsInRoleAsync(user, UserClasses.Role_Owner);
            if (isOwner)
            {
                return true;
            }


            var userInGroupPermission = await _context.GroupPermission_User
                 .Where(gpu => gpu.UserId == userId)
                 .Select(gpu => gpu.ID_GroupPermission)
                 .FirstOrDefaultAsync();

            if (userInGroupPermission != 0)
            {
                var typeDocumentGroup = await _context.TypeDocument_Group
                    .FirstOrDefaultAsync(tdg => tdg.ID_GroupPermission == userInGroupPermission);

                if (typeDocumentGroup != null)
                {
                    if (typeDocumentGroup.IsReadModify)
                    {
                        return true;
                    }
                    else if (typeDocumentGroup.IsRead)
                    {
                        return true; 
                    }
                    else if (typeDocumentGroup.NoPermission)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> HasPermission2(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return false;
            }

            // Kiểm tra vai trò Owner
            var isOwner = await _userManager.IsInRoleAsync(user, UserClasses.Role_Owner);
            if (isOwner)
            {
                return true;
            }


            var userInGroupPermission = await _context.GroupPermission_User
                 .Where(gpu => gpu.UserId == userId)
                 .Select(gpu => gpu.ID_GroupPermission)
                 .FirstOrDefaultAsync();

            if (userInGroupPermission != 0)
            {
                var typeDocumentGroup = await _context.TypeDocument_Group
                    .FirstOrDefaultAsync(tdg => tdg.ID_GroupPermission == userInGroupPermission);

                if (typeDocumentGroup != null)
                {
                    if (typeDocumentGroup.IsReadModify)
                    {
                        return true;
                    }
                    else if (typeDocumentGroup.IsRead)
                    {
                        return false;
                    }
                    else if (typeDocumentGroup.NoPermission)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
        public async Task<List<FlightDocumentViewModel>> GetAllFlightDocumentsAsync()
        {
            var flightDocuments = await _context.FlightDocuments
                .Include(fd => fd.Flight)
                .Include(fd => fd.TypeDocument)
                .Include(fd => fd.User)
                .Select(fd => new
                {
                    FlightDocument = fd,
                    DocumentGroups = _context.Document_Group
                        .Where(dg => dg.ID_Document == fd.ID_Document)
                        .Select(dg => new Document_GroupViewModel
                        {
                            ID_Document_Group = dg.ID_Document_Group,
                            ID_GroupPermission = dg.ID_GroupPermission,
                            GroupPermissionName = dg.GroupPermission.GroupPermissionName,
                        })
                        .ToList()
                })
                .ToListAsync();

            var flightDocumentViewModels = flightDocuments.Select(fd => new FlightDocumentViewModel
            {
                ID_Document = fd.FlightDocument.ID_Document,
                DocumentName = fd.FlightDocument.DocumentName,
                FlightDocumentNote = fd.FlightDocument.FlightDocumentNote,
                DateCreated = fd.FlightDocument.DateCreated,
                File = fd.FlightDocument.File,
                Version = fd.FlightDocument.Version,
                Flight = fd.FlightDocument.Flight != null ? new FlightViewModel
                {
                    ID_Flight = fd.FlightDocument.Flight.ID_Flight,
                    FlightNumber = fd.FlightDocument.Flight.FlightNo,
                    Route = $"{fd.FlightDocument.Flight.Point_Of_Loading} - {fd.FlightDocument.Flight.Point_Of_Unloading}",
                    DepartureDate = fd.FlightDocument.Flight.DepartureDate
                } : null,
                TypeDocument = fd.FlightDocument.TypeDocument != null ? new TypeDocumentViewModel
                {
                    TypeDocumentName = fd.FlightDocument.TypeDocument.TypeDocumentName,
                    TypeDocumentNote = fd.FlightDocument.TypeDocument.TypeDocumentNote
                } : null,
                User = fd.FlightDocument.User != null ? new UserViewModel
                {
                    FullName = fd.FlightDocument.User.FullName,
                    Email = fd.FlightDocument.User.Email
                } : null,
                Document_Groups = fd.DocumentGroups,
                TypeDocument_Groups = fd.DocumentGroups.Any() ? _context.TypeDocument_Group
                    .Where(tdg => tdg.ID_TypeDocument == fd.FlightDocument.ID_TypeDocument)
                    .ToList() // Kết thúc truy vấn ở đây
                    .Select(tdg => new TypeDocument_GroupViewModel
                    {
                        ID_GroupPermission = tdg.ID_GroupPermission,
                        IsReadModify = tdg.IsReadModify,
                        IsRead = tdg.IsRead,
                        NoPermission = tdg.NoPermission
                        // Thêm các trường khác của TypeDocument_Group bạn muốn hiển thị
                    })
                    .Where(tdg => fd.DocumentGroups.Any(dg => dg.ID_GroupPermission == tdg.ID_GroupPermission)) // So sánh ID_GroupPermission
                    .ToList() : null

            })
            .ToList();
            return flightDocumentViewModels;
        }
        public async Task<FlightDocumentViewModel> GetFlightDocumentByIdAsync(int documentId)
        {
            var flightDocument = await _context.FlightDocuments
                .Include(fd => fd.Flight)
                .Include(fd => fd.TypeDocument)
                .Include(fd => fd.User)
                .Where(fd => fd.ID_Document == documentId)
                .Select(fd => new
                {
                    FlightDocument = fd,
                    DocumentGroups = _context.Document_Group
                        .Where(dg => dg.ID_Document == fd.ID_Document)
                        .Select(dg => new Document_GroupViewModel
                        {
                            ID_Document_Group = dg.ID_Document_Group,
                            ID_GroupPermission = dg.ID_GroupPermission,
                            GroupPermissionName = dg.GroupPermission.GroupPermissionName,
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (flightDocument == null)
            {
                return null; // Trả về null nếu không tìm thấy FlightDocument với ID tương ứng
            }

            return new FlightDocumentViewModel
            {
                ID_Document = flightDocument.FlightDocument.ID_Document,
                DocumentName = flightDocument.FlightDocument.DocumentName,
                FlightDocumentNote = flightDocument.FlightDocument.FlightDocumentNote,
                DateCreated = flightDocument.FlightDocument.DateCreated,
                File = flightDocument.FlightDocument.File,
                Version = flightDocument.FlightDocument.Version,
                Flight = flightDocument.FlightDocument.Flight != null ? new FlightViewModel
                {
                    ID_Flight = flightDocument.FlightDocument.Flight.ID_Flight,
                    FlightNumber = flightDocument.FlightDocument.Flight.FlightNo,
                    Route = $"{flightDocument.FlightDocument.Flight.Point_Of_Loading} - {flightDocument.FlightDocument.Flight.Point_Of_Unloading}",
                    DepartureDate = flightDocument.FlightDocument.Flight.DepartureDate
                } : null,
                TypeDocument = flightDocument.FlightDocument.TypeDocument != null ? new TypeDocumentViewModel
                {
                    TypeDocumentName = flightDocument.FlightDocument.TypeDocument.TypeDocumentName,
                    TypeDocumentNote = flightDocument.FlightDocument.TypeDocument.TypeDocumentNote
                } : null,
                User = flightDocument.FlightDocument.User != null ? new UserViewModel
                {
                    FullName = flightDocument.FlightDocument.User.FullName,
                    Email = flightDocument.FlightDocument.User.Email
                } : null,
                Document_Groups = flightDocument.DocumentGroups,
                TypeDocument_Groups = flightDocument.DocumentGroups.Any() ? _context.TypeDocument_Group
                    .Where(tdg => tdg.ID_TypeDocument == flightDocument.FlightDocument.ID_TypeDocument)
                    .ToList() // Kết thúc truy vấn ở đây
                    .Select(tdg => new TypeDocument_GroupViewModel
                    {
                        ID_GroupPermission = tdg.ID_GroupPermission,
                        IsReadModify = tdg.IsReadModify,
                        IsRead = tdg.IsRead,
                        NoPermission = tdg.NoPermission
                        // Thêm các trường khác của TypeDocument_Group bạn muốn hiển thị
                    })
                    .Where(tdg => flightDocument.DocumentGroups.Any(dg => dg.ID_GroupPermission == tdg.ID_GroupPermission)) // So sánh ID_GroupPermission
                    .ToList() : null

            };
        }
        public async Task<bool> CreateDocumentAsync(CreateDocumentViewModel model)
        {
            var document = new FlightDocument
            {
                DocumentName = model.DocumentName,
                FlightDocumentNote = model.FlightDocumentNote,
                DateCreated = DateTime.Now,
                File = model.File,
                ID_Flight = model.ID_Flight,
                ID_TypeDocument = model.ID_TypeDocument,
                UserId = model.UserId
            };

            var existingDocuments = await _context.FlightDocuments
                .Where(fd => fd.ID_Flight == model.ID_Flight)
                .OrderBy(fd => fd.DateCreated)
                .ToListAsync();

            if (existingDocuments.Count == 0)
            {
                document.Version = 1.0M;
            }
            else
            {
                var lastVersion = existingDocuments.Last().Version;
                document.Version = lastVersion + 0.1M;
            }

            _context.FlightDocuments.Add(document);
            var result = await _context.SaveChangesAsync();

            if (model.ID_GroupPermissions != null && model.ID_GroupPermissions.Any())
            {
                foreach (var groupPermissionId in model.ID_GroupPermissions)
                {
                    var documentGroup = new Document_Group
                    {
                        ID_Document = document.ID_Document, // Lấy ID của tài liệu mới được tạo
                        ID_GroupPermission = groupPermissionId
                    };
                    _context.Document_Group.Add(documentGroup);
                }
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return result > 0;
        }
        public async Task<bool> UpdateFlightDocumentAsync(int id, UpdateDocumentViewModel model)
        {
            // Tìm tài liệu bay cần cập nhật dựa trên ID
            var document = await _context.FlightDocuments.FindAsync(id);

            if (document == null)
            {
                // Trả về false nếu không tìm thấy tài liệu bay với ID tương ứng
                return false;
            }

            // Cập nhật thông tin tài liệu bay từ dữ liệu đầu vào
            document.DocumentName = model.DocumentName;
            document.FlightDocumentNote = model.FlightDocumentNote;
            document.File = model.File;
            document.ID_Flight = model.ID_Flight;
            document.ID_TypeDocument = model.ID_TypeDocument;
            document.UserId = model.UserId;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.FlightDocuments.Update(document);
            var result = await _context.SaveChangesAsync();

            // Xóa hết các nhóm quyền tài liệu liên quan cũ
            var existingDocumentGroups = await _context.Document_Group
                .Where(dg => dg.ID_Document == document.ID_Document)
                .ToListAsync();

            _context.Document_Group.RemoveRange(existingDocumentGroups);

            // Liên kết các nhóm quyền mới được chỉ định nếu có
            if (model.ID_GroupPermissions != null && model.ID_GroupPermissions.Any())
            {
                foreach (var groupPermissionId in model.ID_GroupPermissions)
                {
                    var documentGroup = new Document_Group
                    {
                        ID_Document = document.ID_Document,
                        ID_GroupPermission = groupPermissionId
                    };
                    _context.Document_Group.Add(documentGroup);
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return result > 0;
        }
        public async Task<bool> DeleteFlightDocumentAsync(int id)
        {
            // Tìm tài liệu bay cần xóa
            var documentToDelete = await _context.FlightDocuments.FindAsync(id);

            // Nếu không tìm thấy, trả về false
            if (documentToDelete == null)
            {
                return false;
            }

            try
            {
                // Xóa tài liệu bay
                _context.FlightDocuments.Remove(documentToDelete);
                await _context.SaveChangesAsync();

                // Xóa các bản ghi liên quan trong bảng Document_Group
                var documentGroups = _context.Document_Group.Where(dg => dg.ID_Document == id);
                _context.Document_Group.RemoveRange(documentGroups);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu cần
                return false;
            }
        }
    }
}
