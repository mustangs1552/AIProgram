// See https://aka.ms/new-console-template for more information
using AI.Classes;
using AI.Enums;
using AIConsoleApp.Enums;
using System.Diagnostics;
using System.Reflection;

IFileService fileService = new FileService();
string? cmdInput = "";
CMDs currCMD = CMDs.None;
bool exit = false;
ArtificialNueralNetwork? ann = null;

while (!exit)
{
    cmdInput = GetCMD();
    currCMD = ProcessCMD(cmdInput);
    Console.WriteLine();

    PerformCMD(currCMD);
    Console.WriteLine();
}

string? GetCMD()
{
    Console.Write("Enter command: ");
    return Console.ReadLine();
}

CMDs ProcessCMD(string? cmdStr)
{
    if (string.IsNullOrWhiteSpace(cmdStr)) return CMDs.None;

    CMDs cmd = CMDs.None;    
    if (cmdStr.ToLower() == CMDs.Help.ToString().ToLower()) cmd = CMDs.Help;
    else if (cmdStr.ToLower() == CMDs.Exit.ToString().ToLower()) cmd = CMDs.Exit;
    else if (cmdStr.ToLower() == CMDs.NewANN.ToString().ToLower()) cmd = CMDs.NewANN;
    else if (cmdStr.ToLower() == CMDs.LoadANN.ToString().ToLower()) cmd = CMDs.LoadANN;
    else if (cmdStr.ToLower() == CMDs.SaveANN.ToString().ToLower()) cmd = CMDs.SaveANN;
    else if (cmdStr.ToLower() == CMDs.DisplayANN.ToString().ToLower()) cmd = CMDs.DisplayANN;

    return cmd;
}

void PerformCMD(CMDs cmd)
{
    if (cmd == CMDs.None) return;

    switch (cmd)
    {
        case CMDs.Help:
            PerformHelpCMD();
            break;
        case CMDs.Exit:
            PerformExitCMD();
            break;
        case CMDs.NewANN:
            PerformNewANNCMD();
            break;
        case CMDs.LoadANN:
            PerformLoadANNCMD();
            break;
        case CMDs.SaveANN:
            PerformSaveANNCMD();
            break;
        case CMDs.DisplayANN:
            PerformDisplayANNCMD();
            break;
    }
}

void PerformHelpCMD()
{
    Console.WriteLine($"AI App v{GetProjVersion()}");
    Console.WriteLine("Use this app to manage Artificial Nueral Networks, run training data using them, and give them data to apply results for them after being trained.");
    Console.WriteLine();

    Console.WriteLine("Commands:");
    Console.WriteLine("- Help: Displays these usage intructions and commands.");
    Console.WriteLine("- Exit: Exit app.");
    Console.WriteLine("- NewANN: Setup a new Artificial Nueral Network as the current selected one.");
    Console.WriteLine("- LoadANN: Load an existing Artificial Nueral Network from file.");
    Console.WriteLine("- SaveANN: Save the current Artificial Nueral Network to file.");
    Console.WriteLine("- DisplayANN: Display the current Artificial Nueral Network.");
}

void PerformExitCMD()
{
    exit = true;
}

void PerformNewANNCMD()
{
    Console.WriteLine("Create new Artificial Nueral Network");
    Console.WriteLine("Enter 'Cancel' to return.");

    string? response = "";

    int inputNodeCount = -1;
    while (inputNodeCount <= 0)
    {
        Console.Write("Number of inputs: ");
        response = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;
        int.TryParse(response, out inputNodeCount);
    }
    
    int layerCount = -1;
    while (layerCount <= 0)
    {
        Console.Write("Number of hidden layers: ");
        response = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;
        int.TryParse(response, out layerCount);
    }

    int layerNodeCount = -1;
    while (layerNodeCount <= 0)
    {
        Console.Write("Number of nodes in hidden layers: ");
        response = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;
        int.TryParse(response, out layerNodeCount);
    }

    Algorithms layersAlgorithm = Algorithms.None;
    while (layersAlgorithm != Algorithms.None)
    {
        foreach (string algorithm in Enum.GetNames(typeof(Algorithms)))
        {
            if (algorithm == "None") continue;
            Console.WriteLine($"- {algorithm}");
        }
        Console.Write("Hidden layers' algorithm: ");
        response = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(response)) continue;
        else if (response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;

        if (response.ToLower() == Algorithms.Threshold.ToString().ToLower()) layersAlgorithm = Algorithms.Threshold;
        else if (response.ToLower() == Algorithms.Sigmoid.ToString().ToLower()) layersAlgorithm = Algorithms.Sigmoid;
        else if (response.ToLower() == Algorithms.Rectifier.ToString().ToLower()) layersAlgorithm = Algorithms.Rectifier;
        else if (response.ToLower() == Algorithms.HyperbolicTangent.ToString().ToLower()) layersAlgorithm = Algorithms.HyperbolicTangent;
        else layersAlgorithm = Algorithms.None;
    }

    int outputNodeCount = -1;
    while (outputNodeCount <= 0)
    {
        Console.Write("Number of outputs: ");
        response = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;
        int.TryParse(response, out outputNodeCount);
    }

    Algorithms outputsAlgorithm = Algorithms.None;
    while (outputsAlgorithm != Algorithms.None)
    {
        foreach (string algorithm in Enum.GetNames(typeof(Algorithms)))
        {
            if (algorithm == "None") continue;
            Console.WriteLine($"- {algorithm}");
        }
        Console.Write("Output nodes' algorithm: ");
        response = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(response)) continue;
        else if (response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;

        if (response.ToLower() == Algorithms.Threshold.ToString().ToLower()) outputsAlgorithm = Algorithms.Threshold;
        else if (response.ToLower() == Algorithms.Sigmoid.ToString().ToLower()) outputsAlgorithm = Algorithms.Sigmoid;
        else if (response.ToLower() == Algorithms.Rectifier.ToString().ToLower()) outputsAlgorithm = Algorithms.Rectifier;
        else if (response.ToLower() == Algorithms.HyperbolicTangent.ToString().ToLower()) outputsAlgorithm = Algorithms.HyperbolicTangent;
        else outputsAlgorithm = Algorithms.None;
    }

    string? name = "";
    do
    {
        Console.Write("Name: ");
        name = Console.ReadLine();
    }
    while (string.IsNullOrWhiteSpace(name));
    if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;

    List<int> layers = new List<int>();
    for (int layerI = 0; layerI < layerCount; layerI++) layers.Add(layerNodeCount);

    ann = new ArtificialNueralNetwork(name, inputNodeCount, layers, outputNodeCount, layersAlgorithm, outputsAlgorithm);
    Console.WriteLine();
    Console.WriteLine(ann.ToString());
}

void PerformLoadANNCMD()
{
    Console.WriteLine("Load Existing Artificial Nueral Network");
    Console.WriteLine("Enter 'Cancel' to return.");

    string? name = "";
    do
    {
        Console.Write("Name: ");
        name = Console.ReadLine();
    }
    while (string.IsNullOrWhiteSpace(name));
    if (!string.IsNullOrWhiteSpace(name) && name.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;

    ArtificialNueralNetwork? loadedANN = fileService.LoadANN(name);
    if (loadedANN == null) Console.WriteLine("Load failed.");
    else
    {
        ann = loadedANN;
        Console.WriteLine("Load successful.");
        Console.WriteLine();
        Console.WriteLine(ann.ToString());
    }
}

void PerformSaveANNCMD()
{

}

void PerformDisplayANNCMD()
{
    if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
    else
    {
        Console.WriteLine("Current Artificial Nueral Network");
        Console.WriteLine(ann.ToString());
    }
}

string GetProjVersion()
{
    string? ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
    if (string.IsNullOrEmpty(ver)) return "";

    int lastPlusI = ver.LastIndexOf('+');
    int lastPeriodI = ver.LastIndexOf('.');
    if (lastPeriodI >= 0 && lastPeriodI > lastPlusI) return ver.Substring(0, lastPeriodI);
    else if (lastPlusI >= 0) return ver.Substring(0, lastPlusI);
    else return ver;
}

return 0;

//ArtificialNueralNetwork ann = fileService.LoadANN("ANN") ?? new ArtificialNueralNetwork(3, new List<int>() { 5, 5 }, 1, Algorithms.Sigmoid, Algorithms.Sigmoid);
////ArtificialNueralNetwork ann = new ArtificialNueralNetwork(new ANNTemplate()
////{
////    NumOfInputNodes = 3,
////    HiddenLayerNodes = new List<List<Algorithms>>()
////    {
////        new List<Algorithms>()
////        {
////            Algorithms.Sigmoid,
////            Algorithms.Sigmoid,
////            Algorithms.Sigmoid
////        },
////        new List<Algorithms>()
////        {
////            Algorithms.Sigmoid,
////            Algorithms.Sigmoid,
////            Algorithms.Sigmoid
////        }
////    },
////    OutputNodes = new List<Algorithms>()
////    {
////        Algorithms.Sigmoid
////    }
////});

//string startANN = ann.ToString();
//Console.WriteLine(startANN);

//DateTime startTime = DateTime.Now;

//List<float> outputs = new List<float>();
//List<float> costs = new List<float>();
//List<float> inputValues = new List<float>();
//List<float> correctValues = new List<float>();
//List<List<float>> outputsLists = new List<List<float>>();
//List<List<float>> costLists = new List<List<float>>();
//for (int i = 0; i < 1; i++)
//{
//    Console.WriteLine($"Pass {i + 1}\n");

//    // And Conditions
//    Console.WriteLine("True AND True = True");
//    inputValues = new List<float>() { 1, 0, 1 };
//    correctValues = new List<float>() { 1 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("False AND False = False");
//    inputValues = new List<float>() { 0, 0, 0 };
//    correctValues = new List<float>() { 0 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("True AND True = True");
//    inputValues = new List<float>() { 1, 0, 1 };
//    correctValues = new List<float>() { 1 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("True AND False = False");
//    inputValues = new List<float>() { 1, 0, 0 };
//    correctValues = new List<float>() { 0 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("True AND True = True");
//    inputValues = new List<float>() { 1, 0, 1 };
//    correctValues = new List<float>() { 1 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("False AND True = False");
//    inputValues = new List<float>() { 0, 0, 1 };
//    correctValues = new List<float>() { 0 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    // Or Conditions
//    Console.WriteLine("True OR True = True");
//    inputValues = new List<float>() {1, 1, 1 };
//    correctValues = new List<float>() { 1 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("False OR False = False");
//    inputValues = new List<float>() { 0, 1, 0 };
//    correctValues = new List<float>() { 0 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("True OR False = True");
//    inputValues = new List<float>() { 1, 1, 0 };
//    correctValues = new List<float>() { 1 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());

//    Console.WriteLine("False OR True = True");
//    inputValues = new List<float>() { 0, 1, 1 };
//    correctValues = new List<float>() { 1 };
//    Console.WriteLine("Resulting Values:");
//    outputs = ann.PropagateNetwork(inputValues);
//    foreach (float output in outputs) Console.WriteLine(output.ToString());
//    outputsLists.Add(outputs);
//    Console.WriteLine("Costs:");
//    costs = ann.CalculateOutputCosts(correctValues);
//    costLists.Add(costs);
//    foreach (float cost in costs) Console.WriteLine(cost.ToString());
//    Console.WriteLine();
//    ann.BackPropagateNetwork(inputValues, correctValues);
//    Console.WriteLine(ann.ToString());
//}

////Console.WriteLine("==== Outputs/Costs ==============================================================");
////Console.WriteLine();
////string str = "";
////for (int i = 0; i < outputsLists.Count; i++)
////{
////    str = "";
////    foreach (float output in outputsLists[i]) str += output + " ";
////    str += "/ ";
////    foreach (float cost in costLists[i]) str += cost + " ";
////    Console.WriteLine(str);
////}
////Console.WriteLine();

//Console.WriteLine("==== Done =======================================================================");
//Console.WriteLine();
//Console.WriteLine($"Finished in {DateTime.Now - startTime}.");
//Console.WriteLine();

//Console.WriteLine("==== Model ======================================================================");
//Console.WriteLine();
//Console.WriteLine("Node Format: \"{Input nodes} ({Input nodes' weights}) -> [{Name} = {Current Value} ({Algorithm})] -> {Output nodes} ({Output nodes' weights})\"");
//Console.WriteLine();

//Console.WriteLine("-------- Start ------------------------------------------------------------------");
//Console.WriteLine();
//Console.WriteLine(startANN);

//Console.WriteLine("-------- Result -----------------------------------------------------------------");
//Console.WriteLine();
//Console.WriteLine(ann.ToString());

//Console.WriteLine($"Saved: {fileService.SaveANN(ann)}");