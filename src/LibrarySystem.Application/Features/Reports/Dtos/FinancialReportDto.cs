namespace LibrarySystem.Application.Features.Reports.Dtos;
public class FinancialReportDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TotalRentalIncome { get; set; }
    public decimal TotalFinesCollected { get; set; }
    public decimal TotalPledgeFunds { get; set; }
    public decimal TotalRefunds { get; set; }
    public decimal NetIncome { get; set; }
}