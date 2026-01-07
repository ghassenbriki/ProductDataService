using Leoni.Domain.Entities;
using Leoni.Repositories;
using Leoni.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security;

namespace Leoni.Services.Implementations
{
    public class PermissionService : IpermissionService
    {
        private readonly IGenericRepository<Employee> _empRepository;
        public PermissionService(IGenericRepository<Employee>empRepo)
        {
            _empRepository = empRepo;

        }


        public async Task<bool> HasPermissions(string userId, bool requireAll = false, params string[] permissions)
        {

            if (permissions.Length == 0 || permissions == null)
            {
                return true;
            }
            var hasPErmission = false;
            var p = await _empRepository.GetDbSet()
                .Where(e => e.SessionId == userId)
                .SelectMany(e => e.Roles)
                .SelectMany(r => r.Permissions)
                 .DistinctBy(p=>p.PermissionId)
                 .Select(p => p.PermissionName)
                 .ToListAsync();

            hasPErmission = requireAll ? p.All(p => permissions.Contains(p)) : p.Any(p => permissions.Contains(p));
            return hasPErmission;

        }



    }

}
