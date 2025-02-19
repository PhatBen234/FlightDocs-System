namespace FlightDocs_System.ViewModels.AllFights
{
    public class FlightViewModel
    {
        public int ID_Flight { get; set; }
        public string? FlightNumber { get; set; }
        public string? Route { get; set; }
        public DateTime DepartureDate { get; set; }
        public int DocumentCount { get; set; }
        public List<DocumentViewModel>? Documents { get; set; }
    }
    public class DocumentViewModel
    {
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Creator { get; set; }
        public decimal LatestVersion { get; set; }
    }
}
