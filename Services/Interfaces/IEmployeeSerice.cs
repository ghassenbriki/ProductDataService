using Leoni.Domain.Entities;

namespace Leoni.Services.Interfaces
{
    public interface IEmployeeSerice
    {
        Task <List<Permission>> GetAllPermissions(string empId);
    }
}
