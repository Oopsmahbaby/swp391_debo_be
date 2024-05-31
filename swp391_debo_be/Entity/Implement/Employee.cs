using System;
using System.Collections.Generic;

namespace swp391_debo_be.Entity.Implement;

public partial class Employee
{
    public Guid Id { get; set; }

    public Guid? MngId { get; set; }

    public int? Type { get; set; }

    public double? Salary { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CustomerRecord> CustomerRecords { get; set; } = new List<CustomerRecord>();

    public virtual Manager? Mng { get; set; }

    public virtual ICollection<ClinicTreatment> Treats { get; set; } = new List<ClinicTreatment>();
}
