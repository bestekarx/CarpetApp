namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Dashboard statistics
/// </summary>
public class DashboardStatsDto
{
    // Daily Statistics
    public int TodayReceivedCount { get; set; }
    public int TodayOrdersCount { get; set; }
    public int TodayDeliveriesCount { get; set; }
    public decimal TodayRevenue { get; set; }

    // Order Status Summary
    public int PendingOrdersCount { get; set; }
    public int InProgressOrdersCount { get; set; }
    public int ReadyForDeliveryCount { get; set; }
    public int OutForDeliveryCount { get; set; }

    // Financial Summary
    public decimal TotalDebt { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal YearlyRevenue { get; set; }

    // Customer Summary
    public int TotalCustomers { get; set; }
    public int NewCustomersThisMonth { get; set; }

    // Performance Metrics
    public double AverageDeliveryTime { get; set; } // in hours
    public double CustomerSatisfactionRate { get; set; } // percentage
}
