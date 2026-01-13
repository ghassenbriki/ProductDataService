using Leoni.Domain.Entities;
using Leoni.Repositories;
using Leoni.Services.Interfaces;
using Leoni.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Leoni.Dtos.AuthDto;

namespace Leoni.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        private readonly IConfiguration _config;
        private readonly IEmployeeSerice _employeeSerice;
        private readonly IGenericRepository<Permission> _permissionRepository;

        public AuthService(IGenericRepository<Employee> employeeRepository,
            IGenericRepository<Role> roleRepository,
            IGenericRepository<RefreshToken> refTokenRepository,
            IConfiguration config,
            IEmployeeSerice employeeSerice
            ,IGenericRepository<Permission> permissionRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _refreshTokenRepository = refTokenRepository;
            _permissionRepository = permissionRepository;
            _employeeSerice = employeeSerice;               
            _config = config;
        }
        public async Task<LoggedInDto> signIN(RegistrationDto input)
        {
            var employee = await _employeeRepository.Filter(
         e => e.FirstName == input.FirstName && e.LastName == input.LastName).FirstOrDefaultAsync();

            if (employee == null)
                throw new Exception("Check your information!");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(input.password, employee.HashedPasword);
            if (!isPasswordValid)
                throw new Exception("Incorrect password!");

            var permissions =  await _employeeSerice.GetAllPermissions(employee.SessionId);
            var token = SecurityConfig.GenerateJwtToken(employee, _config, permissions);

            var refreshToken = SecurityConfig.CreaTeRefreshToken(employee.SessionId);
            await _refreshTokenRepository.Add(refreshToken);
            await _refreshTokenRepository.SaveAsync();

            return new LoggedInDto
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                SessionId = employee.SessionId
            };




        }
        public async Task SignOut(string refreshToken,string? employeeId, bool alldevices = false)
        {

           

            var refTokens = await _refreshTokenRepository.Filter(r => r.Token == refreshToken && r.IsActive || (alldevices && r.UserId == employeeId)).ToListAsync();

            foreach(var token in refTokens)
            {
                token.IsRevoked = true;
                token.ExpiresAt = DateTime.Now;
                token.RevokedAt = DateTime.Now; 
            }

            await _refreshTokenRepository.SaveAsync();




        }
        public async Task<LoggedInDto> SignUP(RegistrationDto input)
        {
            SecurityConfig.Validate(input.password);

            var emps = _employeeRepository.Filter(e => e.FirstName == input.FirstName && e.LastName == input.LastName).Any();

            if (emps)
            {
                throw new Exception("User already exists");
            }
            string hash = BCrypt.Net.BCrypt.HashPassword(input.password, workFactor: 12);

            var emp = new Employee() { SessionId = String.Concat(input.FirstName,input.LastName,DateTime.Now),
                FirstName = input.FirstName, 
                LastName = input.LastName, 
                HashedPasword = hash };
            await _employeeRepository.Add(emp);
            await _employeeRepository.SaveAsync();

            var role = await _roleRepository.Filter(r => r.RoleName == "Regular Employee").FirstOrDefaultAsync();
            var permission = await _permissionRepository.Filter(p => p.PermissionName == "Task.ChangeState").FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception("Role  : Regular Employee not found");
            }

            if (permission == null)
            {
                throw new Exception("permission :  Task.ChangeState not found");

            }
            role.Employees.Add(emp);
            //emp.Roles.Add(role); optional
            role.Permissions.Add(permission);
            await _roleRepository.Add(role);
            await _permissionRepository.Add(permission);

            await _roleRepository.SaveAsync();




          var permissions = await _employeeSerice.GetAllPermissions(emp.SessionId);

            var token = SecurityConfig.GenerateJwtToken(emp,_config, permissions);
            var refreshToken = SecurityConfig.CreaTeRefreshToken(emp.SessionId);
            await _refreshTokenRepository.Add(refreshToken);
            await _refreshTokenRepository.SaveAsync();


            return new LoggedInDto { Token = token,RefreshToken = refreshToken.Token, FirstName = emp.FirstName, LastName = emp.LastName, SessionId = emp.SessionId };    



            }


        public async Task<LoggedInDto> RotateTokens(string refreshToken)
        {
            var tokenEntity = await _refreshTokenRepository
              .Filter(r => r.Token == refreshToken)
              .Include(r => r.Employee)
              .FirstOrDefaultAsync();

            if (tokenEntity == null || !tokenEntity.IsActive)
            {
                throw new Exception("Session expired, you must log in again");
            }

            var permissions = await _employeeSerice.GetAllPermissions(tokenEntity.Employee.SessionId);
            var token = SecurityConfig.GenerateJwtToken(tokenEntity.Employee, _config, permissions);
            var newrRfreshToken = SecurityConfig.CreaTeRefreshToken(tokenEntity.Employee.SessionId);

            tokenEntity.ExpiresAt = DateTime.UtcNow;
            tokenEntity.IsRevoked = true;
            tokenEntity.RevokedAt = DateTime.UtcNow;
            tokenEntity.ReplacedByToken = newrRfreshToken.Token;
            await _refreshTokenRepository.Add(newrRfreshToken);
            await _refreshTokenRepository.SaveAsync();

            return new LoggedInDto { Token = token, RefreshToken = newrRfreshToken.Token, FirstName = tokenEntity.Employee.FirstName, LastName = tokenEntity.Employee.LastName, SessionId = tokenEntity.Employee.SessionId };


        }




    }
}
