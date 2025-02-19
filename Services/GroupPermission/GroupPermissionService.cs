using FlightDocs_System.Data;
using FlightDocs_System.ViewModels.AllDocuments;
using FlightDocs_System.ViewModels.GroupPermission;

using Microsoft.EntityFrameworkCore;

namespace FlightDocs_System.Services.GroupPermission
{
    public class GroupPermissionService : IGroupPermissionService
    {
        private readonly ApplicationDbContext _context;

        public GroupPermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GroupPermissionViewModel>> GetAllGroupPermissionsAsync()
        {
            var groupPermissions = await _context.GroupPermissions
                .Include(gp => gp.User)
                .Select(gp => new GroupPermissionViewModel
                {
                    ID_GroupPermission = gp.ID_GroupPermission,
                    GroupPermissionName = gp.GroupPermissionName,
                    GroupPermissionNote = gp.GroupPermissionNote,
                    Member = _context.GroupPermission_User.Count(gpu => gpu.ID_GroupPermission == gp.ID_GroupPermission), 
                    DateCreated = gp.DateCreated,
                    UserId = gp.UserId,
                    User = new UserViewModel
                    {
                        FullName = gp.User.FullName,
                        Email = gp.User.Email
                    },
                    RelatedUserIds = _context.GroupPermission_User
                                .Where(gpu => gpu.ID_GroupPermission == gp.ID_GroupPermission)
                                .Select(gpu => new UserViewModel
                                {
                                    FullName = gpu.User.FullName,
                                    Email = gpu.User.Email
                                })
                                .ToList()
                })
                .ToListAsync();

            return groupPermissions;
        }
        public async Task<bool> AddGroupPermissionAsync(AddGroupPermissionViewModel model)
        {
            var groupPermission = new Data.GroupPermission
            {
                GroupPermissionName = model.GroupPermissionName,
                GroupPermissionNote = model.GroupPermissionNote,
                Member = model.Member,
                DateCreated = DateTime.Now,
                UserId = model.UserId
            };

            _context.GroupPermissions.Add(groupPermission);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }



        public async Task<bool> UpdateGroupPermissionAsync(UpdateGroupPermissionViewModel model)
        {
            var groupPermission = await _context.GroupPermissions.FindAsync(model.ID_GroupPermission);
            if (groupPermission == null)
            {
                return false; // GroupPermission not found
            }

            // Update groupPermission properties
            groupPermission.GroupPermissionName = model.GroupPermissionName;
            groupPermission.GroupPermissionNote = model.GroupPermissionNote;
            groupPermission.UserId = model.UserId;

            // Save changes to the database
            var result = await _context.SaveChangesAsync();

            // Add data to GroupPermission_User table
            if (model.UserIds != null && model.UserIds.Any())
            {
                foreach (var userId in model.UserIds)
                {
                    var groupPermissionUser = new GroupPermission_User
                    {
                        ID_GroupPermission = model.ID_GroupPermission,
                        UserId = userId
                    };

                    _context.GroupPermission_User.Add(groupPermissionUser);
                }

                var groupPermissionUserResult = await _context.SaveChangesAsync();

                return groupPermissionUserResult > 0;
            }

            return result>0;
        }

        public async Task<bool> DeleteGroupPermissionAsync(int id)
        {
            // Tìm quyền nhóm dựa trên id
            var groupPermission = await _context.GroupPermissions.FindAsync(id);

            // Nếu không tìm thấy quyền nhóm, trả về false
            if (groupPermission == null)
            {
                return false;
            }

            try
            {
                // Xóa quyền nhóm
                _context.GroupPermissions.Remove(groupPermission);

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return true; // Xóa thành công
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                // Trong trường hợp này, bạn có thể ghi nhật ký hoặc thực hiện các hành động phù hợp khác
                return false;
            }
        }



    }
}
