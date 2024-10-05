using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIConsoleApp.Enums;

namespace AIConsoleApp.Models
{
    public class Command
    {
        public Commands CMD { get; set; } = Commands.None;
        public List<Argument> Args { get; set; } = new List<Argument>();
    }
}
