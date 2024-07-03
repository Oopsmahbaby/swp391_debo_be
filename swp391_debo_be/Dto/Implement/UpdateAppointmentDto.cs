namespace swp391_debo_be.Dto.Implement
{
    public class UpdateAppointmentDto
    {
        public string? DentId { get; set; }

        public int? TreateId { get; set; }

        public string? Date { get; set; }

        public int? TimeSlot { get; set; }

        public string? Note { get; set; }
    }
}
