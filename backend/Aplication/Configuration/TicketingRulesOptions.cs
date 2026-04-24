namespace TP_PROYECTO_SOFTWARE.Aplication.Configuration
{
    public class TicketingRulesOptions
    {
        public const string SectionName = "TicketingRules";

        public int MaxSectorsPerEvent { get; set; }
        public int MaxSectorCapacity { get; set; }
        public int MaxRowsPerBulkCreate { get; set; }
        public int MaxSeatsPerRow { get; set; }
        public List<string> RowLabels { get; set; } = new();
    }
}
