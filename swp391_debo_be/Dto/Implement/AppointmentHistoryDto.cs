namespace swp391_debo_be.Dto.Implement
{
    public class AppointmentHistoryDto
    {
        public string? TreatmentName { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public DateOnly? StartDate { get; set; }
    }
}
