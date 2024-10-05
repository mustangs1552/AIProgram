﻿using AI.Classes;
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
        protected bool exit = false;
        protected ArtificialNueralNetwork? ann = null;
        protected TrainingData trainingData = new TrainingData();

        public bool Run(string[] args)
        {
            if (args == null || args.Length == 0) return false;

            Command cmd = ProcessArguments(args);
            if (cmd.CMD == Commands.None) return false;

            PerformCMD(cmd);

            return true;
        }

        public Command ProcessArguments(string[] args)
        {
            if (args == null || args.Length == 0) return new Command();

            Command command = new Command();
            command.CMD = ProcessCMD(args[0]);

            for (int argI = 1; argI < args.Length; argI++) command.Args.Add(ProcessArgument(args[argI]));

            return command;
        }

        protected Commands ProcessCMD(string? cmdStr)
        {
            if (string.IsNullOrWhiteSpace(cmdStr)) return Commands.None;

            Commands cmd = Commands.None;
            if (cmdStr.ToLower() == Commands.Help.ToString().ToLower()) cmd = Commands.Help;
            else if (cmdStr.ToLower() == Commands.Exit.ToString().ToLower()) cmd = Commands.Exit;
            else if (cmdStr.ToLower() == Commands.NewANN.ToString().ToLower()) cmd = Commands.NewANN;
            else if (cmdStr.ToLower() == Commands.LoadANN.ToString().ToLower()) cmd = Commands.LoadANN;
            else if (cmdStr.ToLower() == Commands.SaveANN.ToString().ToLower()) cmd = Commands.SaveANN;
            else if (cmdStr.ToLower() == Commands.DisplayANN.ToString().ToLower()) cmd = Commands.DisplayANN;
            else if (cmdStr.ToLower() == Commands.LoadData.ToString().ToLower()) cmd = Commands.LoadData;
            else if (cmdStr.ToLower() == Commands.RunData.ToString().ToLower()) cmd = Commands.RunData;
            else if (cmdStr.ToLower() == Commands.DisplayData.ToString().ToLower()) cmd = Commands.DisplayData;
            else if (cmdStr.ToLower() == Commands.Calculate.ToString().ToLower()) cmd = Commands.Calculate;

            return cmd;
        }

        protected Argument ProcessArgument(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg)) return new Argument();

            Argument argument = new Argument();
            string annNameStr = CMDArguments.ANNName.ToString().ToLower();
            string inputsStr = CMDArguments.Inputs.ToString().ToLower();
            string dataPathStr = CMDArguments.DataPath.ToString().ToLower();
            if (arg.Length > annNameStr.Length + 1 && arg.Substring(0, annNameStr.Length + 1).ToLower() == annNameStr) argument.Arg = CMDArguments.ANNName;
            else if (arg.Length > inputsStr.Length + 1 && arg.Substring(0, inputsStr.Length + 1).ToLower() == inputsStr) argument.Arg = CMDArguments.Inputs;
            else if (arg.Length > dataPathStr.Length + 1 && arg.Substring(0, dataPathStr.Length + 1).ToLower() == dataPathStr) argument.Arg = CMDArguments.DataPath;
            if (argument.Arg == CMDArguments.None) return argument;

            switch (argument.Arg)
            {
                case CMDArguments.ANNName:
                case CMDArguments.Inputs:
                case CMDArguments.DataPath:
                    int equalsI = arg.IndexOf('=');
                    if (equalsI > 0) argument.Value = arg.Substring(equalsI);
                    break;
            }

            return argument;
        }

        protected void PerformCMD(Command cmd)
        {
            if (cmd == null) return;

            switch (cmd.CMD)
            {
                case Commands.Help:
                    PerformHelpCMD();
                    break;
                case Commands.Exit:
                    PerformExitCMD();
                    break;
                case Commands.NewANN:
                    PerformNewANNCMD(cmd);
                    break;
                case Commands.LoadANN:
                    PerformLoadANNCMD(cmd);
                    break;
                case Commands.SaveANN:
                    PerformSaveANNCMD(cmd);
                    break;
                case Commands.DisplayANN:
                    PerformDisplayANNCMD(cmd);
                    break;
                case Commands.LoadData:
                    PerformLoadDataCMD(cmd);
                    break;
                case Commands.RunData:
                    PerformRunDataCMD(cmd);
                    break;
                case Commands.DisplayData:
                    PerformDisplayDataCMD(cmd);
                    break;
                case Commands.Calculate:
                    PerformCalculateCMD(cmd);
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

        protected void PerformNewANNCMD(Command cmd)
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

        protected void PerformLoadANNCMD(Command cmd)
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

        protected void PerformSaveANNCMD(Command cmd)
        {
            Console.WriteLine("Save Artificial Nueral Network");
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else
            {
                bool success = fileService.SaveANN(ann);
                Console.WriteLine($"Save {(success ? "successful" : "failed")}.");
            }
        }

        protected void PerformDisplayANNCMD(Command cmd)
        {
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else
            {
                Console.WriteLine("Current Artificial Nueral Network");
                Console.WriteLine(ann.ToString());
            }
        }

        protected void PerformLoadDataCMD(Command cmd)
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

        protected void PerformRunDataCMD(Command cmd)
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

        protected void PerformDisplayDataCMD(Command cmd)
        {
            if (ann == null) Console.WriteLine("No current Artificial Nueral Network. Use 'NewANN' or 'LoadANN' to create or load an existing one.");
            else if (trainingData == null) Console.WriteLine("No current loading data. Use 'LoadData' load an existing some.");
            else
            {
                Console.WriteLine("Current Loading Data");
                Console.WriteLine(trainingData.ToString());
            }
        }

        protected void PerformCalculateCMD(Command cmd)
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
