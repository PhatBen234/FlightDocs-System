using System.ComponentModel.DataAnnotations;

namespace FlightDocs_System.Data
{
    public class Flight
    {
        [Key]
        public int ID_Flight { get; set; }
        public string? FlightNo { get; set; }
        public string? Rouse { get; set; }
        public int Total_Documents { get; set; }
        public DateTime DepartureDate { get; set; }
        public string? Point_Of_Loading { get; set; }
        public string? Point_Of_Unloading { get; set; }
    }
}
