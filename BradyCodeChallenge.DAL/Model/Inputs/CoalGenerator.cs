using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class CoalGenerator : Generator
    {
        [XmlElement("TotalHeatInput")]
        public decimal TotalHeatInput { get; set; }

        [XmlElement("ActualNetGeneration")]
        public decimal ActualNetGeneration { get; set; }

        [XmlElement("EmissionsRating")]
        public decimal EmissionsRating { get; set; }
    }
}
