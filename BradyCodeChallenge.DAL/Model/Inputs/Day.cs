using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class Day
    {
        [XmlElement("Date")]
        public DateTime Date { get; set; }

        [XmlElement("Energy")]
        public decimal Energy { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
