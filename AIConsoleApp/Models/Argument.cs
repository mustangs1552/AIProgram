using AIConsoleApp.Enums;

namespace AIConsoleApp.Models
{
    public class Argument
    {
        public CMDArguments Arg { get; set; } = CMDArguments.None;
        public string Value { get; set; } = "";
    }
}
