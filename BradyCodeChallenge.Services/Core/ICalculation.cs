using BradyCodeChallenge.DAL.Model.Inputs;
using BradyCodeChallenge.DAL.Model.Output;

namespace BradyCodeChallenge.Services.Core
{
    // Interface defining the contract for calculation services
    public interface ICalculation
    {
        // Method to perform calculations based on a GenerationReport and produce a GenerationOutput
        public GenerationOutput? Calculate(GenerationReport generationReport);
    }
}
