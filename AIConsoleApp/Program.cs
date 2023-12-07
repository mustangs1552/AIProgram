// See https://aka.ms/new-console-template for more information
using AI.Classes;
using AI.Models;
using AI.Enums;

ArtificialNeuralNetwork ann = new ArtificialNeuralNetwork(2, new List<int>() { 2 }, 1, Algorithms.Rectifier, Algorithms.Sigmoid);
//ArtificialNeuralNetwork ann = new ArtificialNeuralNetwork(new ANNTemplate()
//{
//    NumOfInputNodes = 2,
//    HiddenLayerNodes = new List<List<AlgorithmType>>()
//    {
//        new List<AlgorithmType>()
//        {
//            AlgorithmType.Rectifier,
//            AlgorithmType.Threshold
//        },
//        new List<AlgorithmType>()
//        {
//            AlgorithmType.Sigmoid,
//            AlgorithmType.HyperbolicTangent
//        }
//    },
//    OutputNodes = new List<AlgorithmType>()
//    {
//        AlgorithmType.Sigmoid,
//        AlgorithmType.Threshold
//    }
//});

Console.WriteLine(ann.ToString());

// And boolean
List<float> outputs = new List<float>();
List<float> costs = new List<float>();
List<float> inputValues = new List<float>();
List<float> actualValues = new List<float>();
List<List<float>> costLists = new List<List<float>>();
for (int i = 0; i < 100; i++)
{
    Console.WriteLine($"Pass {i + 1}\n");

    //Console.WriteLine("False AND False = False");
    //inputValues = new List<float>() { 0, 0 };
    //actualValues = new List<float>() { 0 };
    //Console.WriteLine("Resulting Values:");
    //outputs = ann.PropagateNetwork(inputValues);
    //foreach (float output in outputs) Console.WriteLine(output.ToString());
    //Console.WriteLine("Costs:");
    //costs = ann.CalculateOutputCosts(actualValues);
    //costLists.Add(costs);
    //foreach (float cost in costs) Console.WriteLine(cost.ToString());
    //Console.WriteLine();
    //ann.BackPropagateNetwork(actualValues);
    //Console.WriteLine(ann.ToString());

    Console.WriteLine("True AND False = False");
    inputValues = new List<float>() { 1, 0 };
    actualValues = new List<float>() { 0 };
    Console.WriteLine("Resulting Values:");
    outputs = ann.PropagateNetwork(inputValues);
    foreach (float output in outputs) Console.WriteLine(output.ToString());
    Console.WriteLine("Costs:");
    costs = ann.CalculateOutputCosts(actualValues);
    costLists.Add(costs);
    foreach (float cost in costs) Console.WriteLine(cost.ToString());
    Console.WriteLine();
    ann.BackPropagateNetwork(actualValues);
    Console.WriteLine(ann.ToString());

    //Console.WriteLine("True AND True = True");
    //inputValues = new List<float>() { 1, 1 };
    //actualValues = new List<float>() { 1 };
    //Console.WriteLine("Resulting Values:");
    //outputs = ann.PropagateNetwork(inputValues);
    //foreach (float output in outputs) Console.WriteLine(output.ToString());
    //Console.WriteLine("Costs:");
    //costs = ann.CalculateOutputCosts(actualValues);
    //costLists.Add(costs);
    //foreach (float cost in costs) Console.WriteLine(cost.ToString());
    //Console.WriteLine();
    //ann.BackPropagateNetwork(actualValues);
    //Console.WriteLine(ann.ToString());

    //Console.WriteLine("False AND True = False");
    //inputValues = new List<float>() { 0, 1 };
    //actualValues = new List<float>() { 0 };
    //Console.WriteLine("Resulting Values:");
    //outputs = ann.PropagateNetwork(inputValues);
    //foreach (float output in outputs) Console.WriteLine(output.ToString());
    //Console.WriteLine("Costs:");
    //costs = ann.CalculateOutputCosts(actualValues);
    //costLists.Add(costs);
    //foreach (float cost in costs) Console.WriteLine(cost.ToString());
    //Console.WriteLine();
    //ann.BackPropagateNetwork(actualValues);
    //Console.WriteLine(ann.ToString());
}

Console.WriteLine("Costs:");
string str = "";
foreach (List<float> costList in costLists)
{
    str = "";
    foreach (float cost in costList) str += cost + " ";
    Console.WriteLine(str);
}
Console.WriteLine();

Console.WriteLine(ann.ToString());