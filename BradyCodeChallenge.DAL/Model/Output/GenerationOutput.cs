using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Output
{
    [XmlRoot("GenerationOutput")]
    public class GenerationOutput
    {
        [XmlElement("Totals")]
        public Totals Totals { get; set; }

        [XmlElement("MaxEmissionGenerators")]
        public MaxEmissionGenerators MaxEmissionGenerators { get; set; }

        [XmlElement("ActualHeatRates")]
        public ActualHeatRates ActualHeatRates { get; set; }

        public static GenerationOutput GetInstance() 
        {
            GenerationOutput generationOutput = new GenerationOutput();
            generationOutput.Totals = new Totals();
            generationOutput.Totals.Generator = new List<Generator>();
            generationOutput.MaxEmissionGenerators = new MaxEmissionGenerators();
            generationOutput.MaxEmissionGenerators.Days = new List<Day>();
            generationOutput.ActualHeatRates = new ActualHeatRates();
            generationOutput.ActualHeatRates.ActualHeatRateList = new List<ActualHeatRate>();

            return generationOutput;
        }
    }
}
