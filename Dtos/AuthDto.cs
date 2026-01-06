using Leoni.Domain.Entities;

namespace Leoni.Dtos
{
    public class AuthDto
    {
        public class RegistrationDto
        {
            public string FirstName { get; set; } = null!;   
            public string password { get; set; } = null!;
            public string LastName { get; set; } = null!;


        }

        public class LoggedInDto

        {
            public string Token { get; set; } = null!;

            public string RefreshToken { get; set; } = null!;
            public string SessionId { get; set; } = null!;
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;

        }


    }
}
