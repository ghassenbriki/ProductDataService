namespace Leoni.Services.Interfaces
{
    public interface IpermissionService
    {
        Task<bool> HasPermissions(string userId, bool requireAll = false, params string[] permissions); 
    }
}
