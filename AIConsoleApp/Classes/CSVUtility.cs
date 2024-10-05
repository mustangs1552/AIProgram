using System.Text;

namespace AIConsoleApp.Classes
{
    public class CSVUtility
    {
        /// <summary>
        /// Read the last row of values in the given CSV file.
        /// </summary>
        /// <param name="fileName">The full path of the CSV file read.</param>
        /// <param name="hasHeaders">Does this file have headers as the first row?</param>
        /// <returns>The values read.</returns>
        public static Dictionary<string, string> ReadLatestCSVFileValues(string fileName, bool hasHeaders = true)
        {
            if (!File.Exists(fileName)) return new Dictionary<string, string>();

            IEnumerable<string> fileLines = ReadLinesNoLocking(fileName);
            List<string> firstLineFields = hasHeaders && fileLines.Count() > 0 ? fileLines.First().Split(',').ToList() : new List<string>();
            string removeChars = "";
            if (firstLineFields != null)
            {
                for (int lineI = 0; lineI < firstLineFields.Count; lineI++)
                {
                    removeChars = "";
                    for (int charI = 0; charI < firstLineFields[lineI].Length; charI++)
                    {
                        if (firstLineFields[lineI][charI] == ' ' || firstLineFields[lineI][charI] == '"') removeChars += firstLineFields[lineI][charI];
                        else break;
                    }
                    if (!string.IsNullOrEmpty(removeChars)) firstLineFields[lineI] = firstLineFields[lineI].Replace(removeChars, "");

                    removeChars = "";
                    for (int charI = firstLineFields[lineI].Length - 1; charI >= 0; charI--)
                    {
                        if (firstLineFields[lineI][charI] == ' ' || firstLineFields[lineI][charI] == '"') removeChars += firstLineFields[lineI][charI];
                        else break;
                    }
                    if (!string.IsNullOrEmpty(removeChars)) firstLineFields[lineI] = firstLineFields[lineI].Replace(removeChars, "");
                }
            }
            List<string> lastLineFields = fileLines.Count() > 0 ? fileLines.Last().Split(',').ToList() : new List<string>();

            Dictionary<string, string> dataDict = new Dictionary<string, string>();
            if (lastLineFields == null || lastLineFields.Count == 0) return dataDict;
            KeyValuePair<string, string> currField = new KeyValuePair<string, string>();
            for (int i = 0; i < lastLineFields.Count; i++)
            {
                if (firstLineFields == null || i >= firstLineFields.Count) currField = new KeyValuePair<string, string>($"Column {i + 1}", lastLineFields[i]);
                else currField = new KeyValuePair<string, string>(firstLineFields[i], lastLineFields[i]);

                dataDict.Add(currField.Key, currField.Value);
            }
            return dataDict;
        }

        /// <summary>
        /// Read the last row of values in the CSV file with the latest write time in the given directory.
        /// </summary>
        /// <param name="path">The directory to look in.</param>
        /// <param name="hasHeaders">Does this file have headers as the first row?</param>
        /// <returns>The values read.</returns>
        public static Dictionary<string, string> ReadLatestValuesOfLatestCSVFile(string path, bool hasHeaders = true)
        {
            if (!Directory.Exists(path)) return new Dictionary<string, string>();

            DirectoryInfo dir = new DirectoryInfo(path);
            List<FileInfo> latestFile = (from file in dir.GetFiles() orderby file.LastWriteTime descending select file).ToList();
            FileInfo? latestLogFile = latestFile.Where(file => file.Extension.ToLower() == ".csv").FirstOrDefault();
            if (latestLogFile == null) return new Dictionary<string, string>();

            return ReadLatestCSVFileValues(latestLogFile.FullName, hasHeaders);
        }

        /// <summary>
        /// A custom version of `System.IO.File.ReadLines()` that doesn't lock the file.
        /// </summary>
        /// <param name="path">The full path to the file.</param>
        /// <returns>The lines of the file read.</returns>
        public static IEnumerable<string> ReadLinesNoLocking(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
