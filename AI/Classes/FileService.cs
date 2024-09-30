using AI.Models;
using Newtonsoft.Json;

namespace AI.Classes
{
    public class FileService : IFileService
    {
        public string AppDataFolder
        {
            get
            {
                if (!Directory.Exists(appDataFolder)) Directory.CreateDirectory(appDataFolder);
                return appDataFolder;
            }
        }

        public readonly string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AIApp");
        public readonly string annFolderNameFormat = @"Networks\ANN-{0}";
        public readonly string annModelFileNameFormat = "model-{0}.json";

        public ArtificialNeuralNetwork LoadANN(ArtificialNeuralNetwork ann)
        {
            try
            {
                string annFilePath = GetANNPath(ann.Name);
                if (!File.Exists(annFilePath)) return new ArtificialNeuralNetwork();

                string fileContents = File.ReadAllText(annFilePath);
                SavedArtificialNeuralNetwork? savedANN = JsonConvert.DeserializeObject<SavedArtificialNeuralNetwork>(fileContents);
                if (savedANN == null) return new ArtificialNeuralNetwork();

                return new ArtificialNeuralNetwork(savedANN);
            }
            catch
            {
                return new ArtificialNeuralNetwork();
            }
        }

        public bool SaveANN(ArtificialNeuralNetwork ann)
        {
            if (ann == null) return false;

            try
            {
                string folderName = @$"{AppDataFolder}\{string.Format(annFolderNameFormat, ann.Name)}";
                if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);

                File.WriteAllText(GetANNPath(ann.Name), JsonConvert.SerializeObject(SavedArtificialNeuralNetwork.PopulateSavedANN(ann), Formatting.Indented));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetANNPath(string annName) => Path.Combine(AppDataFolder, string.Format(annFolderNameFormat, annName), string.Format(annModelFileNameFormat, annName));
    }
}
