
namespace BradyCodeChallenge.DAL.Model.Output
{
    public interface IGenerationOutputRepository
    {
        public void Create(GenerationOutput entity, string filePath, string preFileNameWithoutExtension);
    }
}
