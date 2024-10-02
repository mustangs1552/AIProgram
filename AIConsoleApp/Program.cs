// See https://aka.ms/new-console-template for more information
using AI.Classes;
using AI.Enums;
using AI.Models;

IFileService fileService = new FileService();
ArtificialNueralNetwork ann = fileService.LoadANN("ANN") ?? new ArtificialNueralNetwork(3, new List<int>() { 5, 5 }, 1, Algorithms.Sigmoid, Algorithms.Sigmoid);
//ArtificialNueralNetwork ann = new ArtificialNueralNetwork(new ANNTemplate()
//{
//    NumOfInputNodes = 3,
//    HiddenLayerNodes = new List<List<Algorithms>>()
//    {
//        new List<Algorithms>()
//        {
//            Algorithms.Sigmoid,
//            Algorithms.Sigmoid,
//            Algorithms.Sigmoid
//        },
//        new List<Algorithms>()
//        {
//            Algorithms.Sigmoid,
//            Algorithms.Sigmoid,
//            Algorithms.Sigmoid
//        }
//    },
//    OutputNodes = new List<Algorithms>()
//    {
//        Algorithms.Sigmoid
//    }
//});

string startANN = ann.ToString();
Console.WriteLine(startANN);

DateTime startTime = DateTime.Now;

List<float> outputs = new List<float>();
List<float> costs = new List<float>();
List<float> inputValues = new List<float>();
List<float> correctValues = new List<float>();
List<List<float>> outputsLists = new List<List<float>>();
List<List<float>> costLists = new List<List<float>>();
for (int i = 0; i < 1; i++)
{
    Console.WriteLine($"Pass {i + 1}\n");

    // And Conditions
    Console.WriteLine("True AND True = True");
    inputValues = new List<float>() { 1, 0, 1 };
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
    inputValues = new List<float>() { 0, 0, 0 };
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
    inputValues = new List<float>() { 1, 0, 1 };
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
    inputValues = new List<float>() { 1, 0, 0 };
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
    inputValues = new List<float>() { 1, 0, 1 };
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
    inputValues = new List<float>() { 0, 0, 1 };
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

    // Or Conditions
    Console.WriteLine("True OR True = True");
    inputValues = new List<float>() {1, 1, 1 };
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

    Console.WriteLine("False OR False = False");
    inputValues = new List<float>() { 0, 1, 0 };
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

    Console.WriteLine("True OR False = True");
    inputValues = new List<float>() { 1, 1, 0 };
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

    Console.WriteLine("False OR True = True");
    inputValues = new List<float>() { 0, 1, 1 };
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
}

//Console.WriteLine("==== Outputs/Costs ==============================================================");
//Console.WriteLine();
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

Console.WriteLine("==== Done =======================================================================");
Console.WriteLine();
Console.WriteLine($"Finished in {DateTime.Now - startTime}.");
Console.WriteLine();

Console.WriteLine("==== Model ======================================================================");
Console.WriteLine();
Console.WriteLine("Node Format: \"{Input nodes} ({Input nodes' weights}) -> [{Name} = {Current Value} ({Algorithm})] -> {Output nodes} ({Output nodes' weights})\"");
Console.WriteLine();

Console.WriteLine("-------- Start ------------------------------------------------------------------");
Console.WriteLine();
Console.WriteLine(startANN);

Console.WriteLine("-------- Result -----------------------------------------------------------------");
Console.WriteLine();
Console.WriteLine(ann.ToString());

Console.WriteLine($"Saved: {fileService.SaveANN(ann)}");