using System;
using System.Collections.Generic;

namespace Leoni.Domain.Entities;

public partial class Status
{
    public int Id { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
