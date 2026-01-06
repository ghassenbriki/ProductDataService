using System;
using System.Collections.Generic;

namespace Leoni.Domain.Entities;

public partial class Employee
{
    public string SessionId { get; set; } = null!;

    public string HashedPasword { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();

    public virtual ICollection<Tasks> TasksAssigned { get; set; } = new List<Tasks>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

}
