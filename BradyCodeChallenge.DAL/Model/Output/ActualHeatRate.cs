using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Output
{
    public class ActualHeatRate
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("HeatRate")]
        public decimal HeatRate { get; set; }
    }
}
