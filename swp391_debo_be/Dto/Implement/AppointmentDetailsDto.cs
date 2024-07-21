using System.ComponentModel.DataAnnotations;

namespace swp391_debo_be.Dto.Implement
{
    public class AppointmentDetailsDto
    {
        public Guid Id { get; set; }
        public int? TreatId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int? TimeSlot { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public string? CategoryName { get; set; }
        public string? TreatmentName { get; set;}
        public double? Price { get; set; }
        public int? RescheduleCount { get; set; }
        public Guid? Cus_Id { get; set; }
        public Guid? Dent_Id { get; set; }
        public Guid? Temp_Dent_Id { get; set; }
        public string? DentistName { get; set; }
        public string? CustomerName { get; set; }
        public string? CreatorName { get; set; }
        public string? Dent_Avt {  get; set; }
        public string? RescheduleToken { get; set; }
        public bool? IsRequestedDentReschedule { get; set; }
    }
}
