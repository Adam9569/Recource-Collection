using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Recource_Collection
{
    public static class QuestionManager
    {


        public static List<Question> LoadQuestions(string path) => JsonSerializer.Deserialize<List<Question>>(File.ReadAllText(path));

        public static void SaveQuestions(List<Question> questions, string path)
        {
            string json = JsonSerializer.Serialize(questions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
