namespace swp391_debo_be.Dto.Implement
{
    public class AppointmentDto
    {
        public int? TreatId { get; set; }

        public Guid? PaymentId { get; set; }

        public Guid? DentId { get; set; }

        public Guid? TempDentId { get; set; }

        public Guid? CusId { get; set; }

        public Guid? CreatorId { get; set; }

        public bool? IsCreatedByStaff { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public DateOnly? StartDate { get; set; }

        public int? TimeSlot { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public string? Note { get; set; }
    }
}
