using Leoni.Domain.Entities;
using Leoni.Persistence;
using Leoni.Repositories;
using Leoni.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Leoni.Services.Implementations
{
    public class EmployeeSerivce : IEmployeeSerice
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeeSerivce(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public Task<List<Permission>> GetAllPermissions(string sessionId)
        {
            return _employeeRepo.GetDbSet()
                .Where(e => e.SessionId == sessionId)
                .SelectMany(e => e.Roles)
                .SelectMany(r => r.Permissions)
                .AsNoTracking()
                .DistinctBy(p => p.PermissionId)
                .ToListAsync();

            /*SELECT DISTINCT p.*
            FROM Employees e
            JOIN EmployeeRoles er ON er.EmployeeId = e.Id
            JOIN Roles r ON r.Id = er.RoleId
            JOIN RolePermissions rp ON rp.RoleId = r.Id
            JOIN Permissions p ON p.Id = rp.PermissionId
            WHERE e.SessionId = @sessionId*/
        }
    }
}
