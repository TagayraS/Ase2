
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEGraphics
{
    public class Loop
    {
        private string LoopStartCommand = "while";
        private string LoopEndCommand = "endloop";
        private int loopCount;

        public Loop()
        {
            loopCount = 0;
        }

        public bool IsLoopStartCommand(string command)
        {
            return command.Trim().StartsWith(LoopStartCommand, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsLoopEndCommand(string command)
        {
            return command.Trim().Equals(LoopEndCommand, StringComparison.OrdinalIgnoreCase);
        }

        public int GetLoopCount(string loopStartCommand)
        {
            string[] parts = loopStartCommand.Split(new[] { '<', '>', '=', '!', '>', '<' });
            if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int count))
            {
                loopCount = count;
            }

            return loopCount;
        }

    }
}