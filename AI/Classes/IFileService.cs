namespace AI.Classes
{
    public interface IFileService
    {
        ArtificialNueralNetwork? LoadANN(string name);
        bool SaveANN(ArtificialNueralNetwork ann);
    }
}
