namespace swp391_debo_be.Dto.Implement
{
    public class DashboardCustomerDto
    {
        public Guid? CusId { get; set; }
        public string? Status { get; set; }
        public double? TotalPaidAmount { get; set; }
        public int? AppointmentCount { get; set; }
    }
}
