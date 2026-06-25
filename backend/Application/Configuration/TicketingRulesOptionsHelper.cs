namespace TP_PROYECTO_SOFTWARE.Application.Configuration
{
    public static class TicketingRulesOptionsHelper
    {
        public static List<string> GetValidRowLabels(TicketingRulesOptions rules) => rules.RowLabels
            .Take(rules.MaxRowsPerBulkCreate)
            .Select(row => row.Trim().ToUpperInvariant())
            .ToList();
    }
}
