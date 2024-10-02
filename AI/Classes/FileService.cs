using AI.Models;
using Newtonsoft.Json;

namespace AI.Classes
{
    /// <summary>
    /// The service that handles writing/reading from files.
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// The App Data folder for this app.
        /// </summary>
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

        /// <summary>
        /// Load a saved Artificial Nueral Network from file with the given name.
        /// </summary>
        /// <param name="name">The name of the ANN to load.</param>
        /// <returns>The ANN found or null if none was found or errored.</returns>
        public ArtificialNueralNetwork? LoadANN(string name)
        {
            try
            {
                string annFilePath = GetANNPath(name);
                if (!File.Exists(annFilePath)) return null;

                string fileContents = File.ReadAllText(annFilePath);
                SavedArtificialNueralNetwork? savedANN = JsonConvert.DeserializeObject<SavedArtificialNueralNetwork>(fileContents);
                if (savedANN == null) return null;

                return new ArtificialNueralNetwork(savedANN);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Save an Artificial Nueral Network to file.
        /// </summary>
        /// <param name="ann">The ANN to save.</param>
        /// <returns>True if successful.</returns>
        public bool SaveANN(ArtificialNueralNetwork ann)
        {
            if (ann == null) return false;

            try
            {
                string folderName = @$"{AppDataFolder}\{string.Format(annFolderNameFormat, ann.Name)}";
                if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);

                File.WriteAllText(GetANNPath(ann.Name), JsonConvert.SerializeObject(SavedArtificialNueralNetwork.PopulateSavedANN(ann), Formatting.Indented));

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get the save path of a Artificial Nueral Network (does not mean file exists).
        /// </summary>
        /// <param name="annName">The name of the ANN.</param>
        /// <returns>The path produced.</returns>
        public string GetANNPath(string annName) => Path.Combine(AppDataFolder, string.Format(annFolderNameFormat, annName), string.Format(annModelFileNameFormat, annName));
    }
}
