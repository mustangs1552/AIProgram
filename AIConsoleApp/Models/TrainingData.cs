namespace AIConsoleApp.Models
{
    public class TrainingData
    {
        public string Path { get; set; } = "";
        public List<List<float>> Inputs { get; set; } = new List<List<float>>();
        public List<List<float>> CorrectOutputs { get; set; } = new List<List<float>>();

        public TrainingData() { }
        public TrainingData(string path, IEnumerable<string> csvData, int inputCount, int outputCount)
        {
            if (csvData == null) return;

            Path = path;

            int valueI = 0;
            string[] values = new string[] { };
            float currValue = -1;
            List<float> currValues = new List<float>();
            foreach (string row in csvData)
            {
                values = row.Split(',');

                currValues = new List<float>();
                for (valueI = 0; valueI < inputCount; valueI++)
                {
                    if (valueI >= values.Length) break;
                    if (float.TryParse(values[valueI], out currValue)) currValues.Add(currValue);
                }
                if (currValues.Any()) Inputs.Add(currValues);

                currValues = new List<float>();
                for (valueI = inputCount; valueI < inputCount + outputCount; valueI++)
                {
                    if (valueI >= values.Length) break;
                    if (float.TryParse(values[valueI], out currValue)) currValues.Add(currValue);
                }
                if (currValues.Any()) CorrectOutputs.Add(currValues);
            }
        }

        public override string ToString()
        {
            if (Inputs.Count == 0) return "No training data.";

            string str = $"Input Rows: {Inputs.Count} Correct Outputs Rows: {CorrectOutputs.Count} Inputs: {Inputs.First().Count} Outputs: {CorrectOutputs.First().Count}\n";
            for (int rowI = 0; rowI < Inputs.Count; rowI++)
            {
                str += $"- Row {rowI + 1}: Inputs: ";
                foreach (float input in Inputs[rowI]) str += $"{input} ";
                str += $"Correct Outputs: ";
                foreach (float output in CorrectOutputs[rowI]) str += $"{output} ";
                str += "\n";
            }
            str += $"Input Rows: {Inputs.Count} Correct Outputs Rows: {CorrectOutputs.Count} Inputs: {Inputs.First().Count} Outputs: {CorrectOutputs.First().Count}";

            return str;
        }
    }
}
