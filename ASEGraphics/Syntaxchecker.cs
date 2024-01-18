using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ASEGraphics
{
    public class SyntaxChecker
    {
        public static string ValidateSyntax(string[] commands)
        {
            List<string> validKeywords = new List<string> { "rectangle", "triangle", "circle", "moveto", "drawto", "pen", "fill", "reset", "clear" };

            if (commands.Length == 0 || !validKeywords.Contains(commands[0].ToLower()))
            {
                return "Invalid command: " + (commands.Length > 0 ? commands[0] : "No command");
            }

            string keyword = commands[0].ToLower();

            switch (keyword)
            {
                case "rectangle":
                    if (commands.Length != 3 || !int.TryParse(commands[1], out int width) || !int.TryParse(commands[2], out int height) || width <= 0 || height <= 0)
                    {
                        return "Invalid syntax for rectangle command.";
                    }
                    break;

                case "triangle":
                    if (commands.Length != 2 && commands.Length != 4 || !int.TryParse(commands[1], out int sideLength) || sideLength <= 0)
                    {
                        return "Invalid syntax for triangle command.";
                    }
                    break;

                case "circle":
                    if (commands.Length != 2 || !int.TryParse(commands[1], out int radius) || radius <= 0)
                    {
                        return "Invalid syntax for circle command.";
                    }
                    break;

                case "moveto":
                case "drawto":
                    if (commands.Length != 3 || !int.TryParse(commands[1], out int xCoordinate) || !int.TryParse(commands[2], out int yCoordinate) || xCoordinate < 0 || yCoordinate < 0)
                    {
                        return $"Invalid syntax for {keyword} command.";
                    }
                    break;

                case "pen":
                    if (commands.Length != 2 || !new List<string> { "yellow", "white", "red", "green", "blue", "pink", "purple", "orange" }.Contains(commands[1].ToLower()))
                    {
                        return "Invalid syntax for pen command.";
                    }
                    break;

                case "fill":
                    if (commands.Length != 2 || !commands[1].ToLower().Equals("on") && !commands[1].ToLower().Equals("off"))
                    {
                        return "Invalid syntax for fill command.";
                    }
                    break;

                default:
                    break;
            }

            return "Syntax is correct.";
        }
    }
}