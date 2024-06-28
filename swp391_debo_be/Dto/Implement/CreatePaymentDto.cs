namespace swp391_debo_be.Dto.Implement
{
    public class CreatePaymentDto
    {
        public List<string> ListAppointmentId { get; set; } = new List<string>();
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        public string? PaymentLanguage { get; set; } = string.Empty;
    }
}
