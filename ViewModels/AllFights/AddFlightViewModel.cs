namespace FlightDocs_System.ViewModels.AllFights
{
    public class AddFlightViewModel
    {
        public string? FlightNo { get; set; }
        public string? Route { get; set; }
        public DateTime DepartureDate { get; set; }
        public string? PointOfLoading { get; set; }
        public string? PointOfUnloading { get; set; }
        public int TotalDocuments { get; set; }
        // Bổ sung các trường khác liên quan đến chuyến bay ở đây
    }
}
