using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Recource_Collection
{
    public class QuestionManager
    {
        public List<Question> LoadQuestions(string path)
        {
            string jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Question>>(jsonString);
        }
    }
}
