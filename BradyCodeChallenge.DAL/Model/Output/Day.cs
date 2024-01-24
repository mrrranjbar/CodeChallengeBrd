using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Output
{
    public class Day
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Date")]
        public DateTime Date { get; set; }

        [XmlElement("Emission")]
        public decimal Emission { get; set; }
    }
}
