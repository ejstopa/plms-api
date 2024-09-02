
namespace Domain.Services
{
    public class ModelRevisionService : IModelRevisionService
    {
        public string IncrementRevision(string currentRevision)
        {
            List<char> revisionChars = [.. currentRevision.ToCharArray()];
            char lastChar = revisionChars[revisionChars.Count - 1];

            if (lastChar == '-')
            {
                revisionChars[revisionChars.Count - 1] = 'A';
            }
            else if (lastChar != 'Z')
            {
                revisionChars[revisionChars.Count - 1] = ++lastChar;
            }
            else
            {
                revisionChars[revisionChars.Count - 1] = 'A';
                revisionChars.Add('A');
            }

            return string.Join("", revisionChars.ToArray());
        }
    }
}