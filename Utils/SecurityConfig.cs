using Leoni.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Leoni.Utils
{
    public static class SecurityConfig
    {
        public static string GenerateJwtToken(Employee user , IConfiguration _config , List<Permission> permissions)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.SessionId.ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };

            foreach(var p in permissions)
            {
                claims.Add(new Claim("permission", p.PermissionName));
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static RefreshToken  CreaTeRefreshToken(string userId)
        {

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                UserId = userId,

            };

            return refreshToken;
        }


        public static void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (password.Length < 8)
                throw new Exception("Password must be at least 8 characters long");

            if (!password.Any(char.IsUpper))
                throw new Exception("Password must contain at least one uppercase letter");

            if (!password.Any(char.IsLower))
                throw new Exception("Password must contain at least one lowercase letter");

            if (!password.Any(char.IsDigit))
                throw new Exception("Password must contain at least one number");

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new Exception("Password must contain at least one special character");

            if (password.Any(char.IsWhiteSpace))
                throw new Exception("Password must not contain spaces");
        }


        public static bool IsValidGitHubSignature(string payload, string signatureHeader, string secret)
        {
            if (string.IsNullOrEmpty(signatureHeader) || string.IsNullOrEmpty(secret)) return false;

            var secretBytes = Encoding.UTF8.GetBytes(secret);

            using var hmac = new HMACSHA256(secretBytes);
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));

            var computedSignature = "sha256=" + Convert.ToHexString(hashBytes).ToLower();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(signatureHeader)
            );
        }
    }
}
