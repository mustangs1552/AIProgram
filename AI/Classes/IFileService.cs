using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Classes
{
    public interface IFileService
    {
        ArtificialNeuralNetwork LoadANN();
        bool SaveANN(ArtificialNeuralNetwork ann);
    }
}
