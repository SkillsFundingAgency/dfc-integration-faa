
namespace DFC.Integration.AVFeed.Data.Models
{
    public class AddressLocation
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public GeoPoint GeoPoint { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }
    }
}
