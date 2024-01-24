using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class GasGenerator : Generator
    {
        [XmlElement("EmissionsRating")]
        public decimal EmissionsRating { get; set; }
    }
}
