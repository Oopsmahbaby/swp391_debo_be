namespace swp391_debo_be.Dto.Implement
{
    public class PaymentLinkDto
    {
        public bool IsGeneralCheckup { get; set; }
        public Guid? PaymentId { get; set; }
        public string PaymentUrl { get; set; } = string.Empty;
    }
}
