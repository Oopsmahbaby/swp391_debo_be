namespace swp391_debo_be.Dto.Implement
{
    public class TreatmenBranchReturnDto
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? RuleName { get; set; }
        public int NumOfAppointment { get; set; }
    }
}
