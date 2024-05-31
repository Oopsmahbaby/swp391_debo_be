using System;
using System.Collections.Generic;

namespace swp391_debo_be.Entity.Implement;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid? CusId { get; set; }

    public int? MethodId { get; set; }

    public double? Amount { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User? Cus { get; set; }

    public virtual PaymentMethod? Method { get; set; }
}
