﻿// See https://aka.ms/new-console-template for more information
using AI.Classes;
using AI.Enums;

ArtificialNeuralNetwork ann = new ArtificialNeuralNetwork(2, new List<int>() { 5, 5 }, 1, Algorithms.Sigmoid, Algorithms.Sigmoid);
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
List<float> correctValues = new List<float>();
List<List<float>> outputsLists = new List<List<float>>();
List<List<float>> costLists = new List<List<float>>();
for (int i = 0; i < 50000; i++)
{
    Console.WriteLine($"Pass {i + 1}\n");

    Console.WriteLine("True AND True = True");
    inputValues = new List<float>() { 1, 1 };
    correctValues = new List<float>() { 1 };
    Console.WriteLine("Resulting Values:");
    outputs = ann.PropagateNetwork(inputValues);
    foreach (float output in outputs) Console.WriteLine(output.ToString());
    outputsLists.Add(outputs);
    Console.WriteLine("Costs:");
    costs = ann.CalculateOutputCosts(correctValues);
    costLists.Add(costs);
    foreach (float cost in costs) Console.WriteLine(cost.ToString());
    Console.WriteLine();
    ann.BackPropagateNetwork(inputValues, correctValues);
    Console.WriteLine(ann.ToString());

    Console.WriteLine("False AND False = False");
    inputValues = new List<float>() { 0, 0 };
    correctValues = new List<float>() { 0 };
    Console.WriteLine("Resulting Values:");
    outputs = ann.PropagateNetwork(inputValues);
    foreach (float output in outputs) Console.WriteLine(output.ToString());
    outputsLists.Add(outputs);
    Console.WriteLine("Costs:");
    costs = ann.CalculateOutputCosts(correctValues);
    costLists.Add(costs);
    foreach (float cost in costs) Console.WriteLine(cost.ToString());
    Console.WriteLine();
    ann.BackPropagateNetwork(inputValues, correctValues);
    Console.WriteLine(ann.ToString());

    Console.WriteLine("True AND True = True");
    inputValues = new List<float>() { 1, 1 };
    correctValues = new List<float>() { 1 };
    Console.WriteLine("Resulting Values:");
    outputs = ann.PropagateNetwork(inputValues);
    foreach (float output in outputs) Console.WriteLine(output.ToString());
    outputsLists.Add(outputs);
    Console.WriteLine("Costs:");
    costs = ann.CalculateOutputCosts(correctValues);
    costLists.Add(costs);
    foreach (float cost in costs) Console.WriteLine(cost.ToString());
    Console.WriteLine();
    ann.BackPropagateNetwork(inputValues, correctValues);
    Console.WriteLine(ann.ToString());

    Console.WriteLine("True AND False = False");
    inputValues = new List<float>() { 1, 0 };
    correctValues = new List<float>() { 0 };
    Console.WriteLine("Resulting Values:");
    outputs = ann.PropagateNetwork(inputValues);
    foreach (float output in outputs) Console.WriteLine(output.ToString());
    outputsLists.Add(outputs);
    Console.WriteLine("Costs:");
    costs = ann.CalculateOutputCosts(correctValues);
    costLists.Add(costs);
    foreach (float cost in costs) Console.WriteLine(cost.ToString());
    Console.WriteLine();
    ann.BackPropagateNetwork(inputValues, correctValues);
    Console.WriteLine(ann.ToString());

    Console.WriteLine("True AND True = True");
    inputValues = new List<float>() { 1, 1 };
    correctValues = new List<float>() { 1 };
    Console.WriteLine("Resulting Values:");
    outputs = ann.PropagateNetwork(inputValues);
    foreach (float output in outputs) Console.WriteLine(output.ToString());
    outputsLists.Add(outputs);
    Console.WriteLine("Costs:");
    costs = ann.CalculateOutputCosts(correctValues);
    costLists.Add(costs);
    foreach (float cost in costs) Console.WriteLine(cost.ToString());
    Console.WriteLine();
    ann.BackPropagateNetwork(inputValues, correctValues);
    Console.WriteLine(ann.ToString());

    Console.WriteLine("False AND True = False");
    inputValues = new List<float>() { 0, 1 };
    correctValues = new List<float>() { 0 };
    Console.WriteLine("Resulting Values:");
    outputs = ann.PropagateNetwork(inputValues);
    foreach (float output in outputs) Console.WriteLine(output.ToString());
    outputsLists.Add(outputs);
    Console.WriteLine("Costs:");
    costs = ann.CalculateOutputCosts(correctValues);
    costLists.Add(costs);
    foreach (float cost in costs) Console.WriteLine(cost.ToString());
    Console.WriteLine();
    ann.BackPropagateNetwork(inputValues, correctValues);
    Console.WriteLine(ann.ToString());
}

//Console.WriteLine("===== Outputs/Costs ==============================================================");
//string str = "";
//for (int i = 0; i < outputsLists.Count; i++)
//{
//    str = "";
//    foreach (float output in outputsLists[i]) str += output + " ";
//    str += "/ ";
//    foreach (float cost in costLists[i]) str += cost + " ";
//    Console.WriteLine(str);
//}
//Console.WriteLine();

Console.WriteLine("===== Resulting Model ============================================================");
Console.WriteLine(ann.ToString());