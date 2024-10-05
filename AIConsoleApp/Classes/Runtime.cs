using AI.Classes;
using AI.Enums;
using AIConsoleApp.Enums;
using AIConsoleApp.Models;
using System.Diagnostics;
using System.Reflection;

namespace AIConsoleApp.Classes
{
    public class Runtime
    {
        protected IFileService fileService = new FileService();
        protected string? cmdInput = "";
        protected CMDs currCMD = CMDs.None;
        protected bool exit = false;
        protected ArtificialNueralNetwork? ann = null;
        protected TrainingData trainingData = new TrainingData();

        public bool Run()
        {
            while (!exit)
            {
                cmdInput = GetCMD();
                currCMD = ProcessCMD(cmdInput);
                Console.WriteLine();

                PerformCMD(currCMD);
                Console.WriteLine();
            }

            return true;
        }

        protected string? GetCMD()
        {
            Console.Write("Enter command: ");
            return Console.ReadLine();
        }

        protected CMDs ProcessCMD(string? cmdStr)
        {
            if (string.IsNullOrWhiteSpace(cmdStr)) return CMDs.None;

            CMDs cmd = CMDs.None;
            if (cmdStr.ToLower() == CMDs.Help.ToString().ToLower()) cmd = CMDs.Help;
            else if (cmdStr.ToLower() == CMDs.Exit.ToString().ToLower()) cmd = CMDs.Exit;
            else if (cmdStr.ToLower() == CMDs.NewANN.ToString().ToLower()) cmd = CMDs.NewANN;
            else if (cmdStr.ToLower() == CMDs.LoadANN.ToString().ToLower()) cmd = CMDs.LoadANN;
            else if (cmdStr.ToLower() == CMDs.SaveANN.ToString().ToLower()) cmd = CMDs.SaveANN;
            else if (cmdStr.ToLower() == CMDs.DisplayANN.ToString().ToLower()) cmd = CMDs.DisplayANN;
            else if (cmdStr.ToLower() == CMDs.LoadData.ToString().ToLower()) cmd = CMDs.LoadData;
            else if (cmdStr.ToLower() == CMDs.RunData.ToString().ToLower()) cmd = CMDs.RunData;
            else if (cmdStr.ToLower() == CMDs.DisplayData.ToString().ToLower()) cmd = CMDs.DisplayData;

            return cmd;
        }

        protected void PerformCMD(CMDs cmd)
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
                case CMDs.LoadData:
                    PerformLoadDataCMD();
                    break;
                case CMDs.RunData:
                    PerformRunDataCMD();
                    break;
                case CMDs.DisplayData:
                    PerformDisplayDataCMD();
                    break;
            }
        }

        protected void PerformHelpCMD()
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

        protected void PerformExitCMD()
        {
            exit = true;
        }

        protected void PerformNewANNCMD()
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
            while (layersAlgorithm == Algorithms.None)
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
            while (outputsAlgorithm == Algorithms.None)
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

        protected void PerformLoadANNCMD()
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

        protected void PerformSaveANNCMD()
        {
            Console.WriteLine("Save Artificial Nueral Network");
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else
            {
                bool success = fileService.SaveANN(ann);
                Console.WriteLine($"Save {(success ? "successful" : "failed")}.");
            }
        }

        protected void PerformDisplayANNCMD()
        {
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else
            {
                Console.WriteLine("Current Artificial Nueral Network");
                Console.WriteLine(ann.ToString());
            }
        }

        protected void PerformLoadDataCMD()
        {
            Console.WriteLine("Load Training Data");
            if (ann == null)
            {
                Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
                return;
            }
            Console.WriteLine("Enter 'Cancel' to return.");

            string? path = "";
            do
            {
                Console.Write("Path (.csv): ");
                path = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(path));
            if (!string.IsNullOrWhiteSpace(path) && path.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;

            IEnumerable<string> trainingDataRows = CSVUtility.ReadLinesNoLocking(path);
            if (!trainingDataRows.Any()) Console.WriteLine("Failed to load training data. Make sure it is a '.csv' file and that the file exists and is not empty.");
            else
            {
                trainingData = new TrainingData(path, trainingDataRows, ann.NumOfInputNodes, ann.NumOfOutputNodes);
                if (trainingData.Inputs.First().Count != ann.NumOfInputNodes || trainingData.CorrectOutputs.First().Count != ann.NumOfOutputNodes) Console.WriteLine("Load failed. Input and/or correct output values don't match the number of inputs and outputs of the curent Artificial Nueral Network.");
                else
                {
                    Console.WriteLine("Load successful.");
                    Console.WriteLine();
                    Console.WriteLine(trainingData.ToString());
                }
            }
        }

        protected void PerformRunDataCMD()
        {
            Console.WriteLine("Run Training Data");
            if (ann == null)
            {
                Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
                return;
            }
            else if (trainingData == null)
            {
                Console.WriteLine("No current loading data. Use 'LoadData' load an existing some.");
                return;
            }
            Console.WriteLine("Enter 'Cancel' to return.");

            string? response = "";
            bool gotResponse = false;
            bool doBackpropagation = true;
            do
            {
                Console.Write("Do backpropagation: ");
                response = Console.ReadLine()?.ToLower();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    gotResponse = response.ToLower() == SubCMDs.Cancel.ToString().ToLower();
                    if (!gotResponse)
                    {
                        if (response == "y" || response == "yes")
                        {
                            doBackpropagation = true;
                            gotResponse = true;
                        }
                        else if (response == "n" || response == "no")
                        {
                            doBackpropagation = false;
                            gotResponse = true;
                        }
                    }
                }
            }
            while (!gotResponse);
            if (!string.IsNullOrWhiteSpace(response) && response == SubCMDs.Cancel.ToString().ToLower()) return;

            int numOfPasses = -1;
            while (numOfPasses <= 0)
            {
                Console.Write("Number of passes: ");
                response = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;
                int.TryParse(response, out numOfPasses);
            }

            List<float> results = new List<float>();
            List<float> costs = new List<float>();
            string testStr = "";
            DateTime startTime = DateTime.Now;
            DateTime startPassTime = DateTime.Now;
            DateTime startTestTime = DateTime.Now;
            for (int passI = 0; passI < numOfPasses; passI++)
            {
                startPassTime = DateTime.Now;
                Console.WriteLine($"\nPass {passI + 1}\n");

                for (int testI = 0; testI < trainingData.Inputs.Count; testI++)
                {
                    startTestTime = DateTime.Now;
                    testStr = $"- Test {testI + 1}: Inputs: ";
                    trainingData.Inputs[testI].ForEach(input => testStr += $"{input} ");

                    testStr += "Correct Outputs: ";
                    trainingData.CorrectOutputs[testI].ForEach(output => testStr += $"{output} ");

                    testStr += "Outputs: ";
                    results = ann.PropagateNetwork(trainingData.Inputs[testI]);
                    costs = ann.CalculateOutputCosts(trainingData.CorrectOutputs[testI]);
                    for (int resultI = 0; resultI < results.Count; resultI++) testStr += $"{results[resultI]} ({costs[resultI]})";

                    if (doBackpropagation) ann.BackPropagateNetwork(trainingData.Inputs[testI], trainingData.CorrectOutputs[testI]);

                    Console.WriteLine($"{testStr} in {DateTime.Now - startTestTime}.");
                }

                Console.WriteLine($"Pass finished in {DateTime.Now - startPassTime}.");
            }

            Console.WriteLine($"\nTraining finished in {DateTime.Now - startTime}.");
        }

        protected void PerformDisplayDataCMD()
        {
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else if (trainingData == null) Console.WriteLine("No current loading data. Use 'LoadData' load an existing some.");
            else
            {
                Console.WriteLine("Current Loading Data");
                Console.WriteLine(trainingData.ToString());
            }
        }

        protected string GetProjVersion()
        {
            string? ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            if (string.IsNullOrEmpty(ver)) return "";

            int lastPlusI = ver.LastIndexOf('+');
            int lastPeriodI = ver.LastIndexOf('.');
            if (lastPeriodI >= 0 && lastPeriodI > lastPlusI) return ver.Substring(0, lastPeriodI);
            else if (lastPlusI >= 0) return ver.Substring(0, lastPlusI);
            else return ver;
        }
    }
}
