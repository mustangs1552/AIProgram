using AI.Classes;
using AI.Enums;
using AIConsoleApp.Enums;
using AIConsoleApp.Models;
using System.Diagnostics;
using System.Reflection;

namespace AIConsoleApp.Classes
{
    public class GuidedRuntime: Runtime
    {
        protected string? cmdInput = "";
        protected Commands currCMD = Commands.None;

        public bool RunGuided()
        {
            while (!exit)
            {
                cmdInput = GetCMD();
                currCMD = ProcessCMD(cmdInput);
                Console.WriteLine();

                PerformGuidedCMD(currCMD);
                Console.WriteLine();
            }

            return true;
        }

        protected string? GetCMD()
        {
            Console.Write("Enter command: ");
            return Console.ReadLine();
        }

        protected void PerformGuidedCMD(Commands cmd)
        {
            if (cmd == Commands.None) return;

            switch (cmd)
            {
                case Commands.Help:
                    PerformHelpCMD();
                    break;
                case Commands.Exit:
                    PerformExitCMD();
                    break;
                case Commands.NewANN:
                    PerformGuidedNewANNCMD();
                    break;
                case Commands.LoadANN:
                    PerformGuidedLoadANNCMD();
                    break;
                case Commands.SaveANN:
                    PerformGuidedSaveANNCMD();
                    break;
                case Commands.DisplayANN:
                    PerformGuidedDisplayANNCMD();
                    break;
                case Commands.LoadData:
                    PerformGuidedLoadDataCMD();
                    break;
                case Commands.RunData:
                    PerformGuidedRunDataCMD();
                    break;
                case Commands.DisplayData:
                    PerformGuidedDisplayDataCMD();
                    break;
                case Commands.Calculate:
                    PerformGuidedCalculateCMD();
                    break;
            }
        }

        protected void PerformGuidedNewANNCMD()
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

        protected void PerformGuidedLoadANNCMD()
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

        protected void PerformGuidedSaveANNCMD()
        {
            Console.WriteLine("Save Artificial Nueral Network");
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else
            {
                bool success = fileService.SaveANN(ann);
                Console.WriteLine($"Save {(success ? "successful" : "failed")}.");
            }
        }

        protected void PerformGuidedDisplayANNCMD()
        {
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else
            {
                Console.WriteLine("Current Artificial Nueral Network");
                Console.WriteLine(ann.ToString());
            }
        }

        protected void PerformGuidedLoadDataCMD()
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

        protected void PerformGuidedRunDataCMD()
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
                Console.Write("Do backpropagation? ");
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

        protected void PerformGuidedDisplayDataCMD()
        {
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else if (trainingData == null) Console.WriteLine("No current loading data. Use 'LoadData' load an existing some.");
            else
            {
                Console.WriteLine("Current Loading Data");
                Console.WriteLine(trainingData.ToString());
            }
        }

        protected void PerformGuidedCalculateCMD()
        {
            Console.WriteLine("Calculate Inputs");
            if (ann == null)
            {
                Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
                return;
            }
            Console.WriteLine("Enter 'Cancel' to return.");

            string? response = "";
            List<float> inputs = new List<float>();
            string[] splitResponse = new string[] { };
            float currInput = -1;
            do
            {
                Console.Write("Inputs (seperated by spaces, musta match current Artificial Nueral Network's number of inputs): ");
                response = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    if (response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) break;

                    splitResponse = response.Split(' ');
                    if (splitResponse != null)
                    {
                        foreach (string resp in splitResponse)
                        {
                            if (float.TryParse(resp, out currInput)) inputs.Add(currInput);
                        }
                    }
                }
            }
            while (!inputs.Any());
            if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == SubCMDs.Cancel.ToString().ToLower()) return;

            if (inputs.Count != ann.NumOfInputNodes) Console.WriteLine("Number of inputs don't match the current Artificial Nueral Network's number of inputs.");
            else
            {
                List<float> outputs = ann.PropagateNetwork(inputs);
                string outputStr = "Outputs: ";
                outputs.ForEach(output => outputStr += $"{output} ");
                Console.WriteLine(outputStr);
            }
        }
    }
}
