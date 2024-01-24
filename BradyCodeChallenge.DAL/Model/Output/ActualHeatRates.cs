using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Output
{
    public class ActualHeatRates
    {
        [XmlElement("ActualHeatRate")]
        public List<ActualHeatRate> ActualHeatRateList { get; set; }
    }
}
