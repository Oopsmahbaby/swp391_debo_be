using System.ComponentModel.DataAnnotations;

namespace swp391_debo_be.Dto.Implement
{
    public class AppointmentDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int? TreatId { get; set; }

        [Required]
        public Guid? DentId { get; set; }

        public Guid? TempDentId { get; set; }

        public DateOnly? CreatedDate { get; set; } = new DateOnly();

        public DateOnly? StartDate { get; set; } = new DateOnly();

        [Required]
        public int? TimeSlot { get; set; }

        [Required]
        [RegularExpression("^(pending|future|on-going|done)$", ErrorMessage = "Invalid status value")]
        public string? Status { get; set; }

        public string? Description { get; set; }

        public string? Note { get; set; }

    }
}
