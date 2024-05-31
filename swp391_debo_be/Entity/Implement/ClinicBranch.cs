using System;
using System.Collections.Generic;

namespace swp391_debo_be.Entity.Implement;

public partial class ClinicBranch
{
    public int Id { get; set; }

    public Guid? MngId { get; set; }

    public Guid? AdminId { get; set; }

    public string? Address { get; set; }

    public virtual User? Admin { get; set; }

    public virtual Manager? Manager { get; set; }
}
