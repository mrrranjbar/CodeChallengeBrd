
namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public interface IGenerationReportRepository
    {
        public GenerationReport? Get(string filePath);
    }
}
