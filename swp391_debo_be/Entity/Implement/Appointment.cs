using System;
using System.Collections.Generic;

namespace swp391_debo_be.Entity.Implement;

public partial class Appointment
{
    public Guid Id { get; set; }

    public int? TreatId { get; set; }

    public Guid? TempDent { get; set; }

    public Guid? BookId { get; set; }

    public int? CusRecId { get; set; }

    public int? EstDuration { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Booking? Book { get; set; }

    public virtual CustomerRecord? CusRec { get; set; }

    public virtual User? TempDentNavigation { get; set; }

    public virtual ClinicTreatment? Treat { get; set; }
}
