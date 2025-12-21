using System;
using System.Collections.Generic;

namespace Leoni.Domain.Entities;

public partial class Component
{
    public string ComponentName { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime ModificationTime { get; set; }

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();

}
