using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Output
{
    public class Totals
    {
        [XmlElement("Generator")]
        public List<Generator> Generator { get; set; }
    }
}
