using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASEGraphics
{
    /// <summary>
    /// Main form for the GraphicalProgrammingEnvironmentASE application.
    /// </summary>
    public partial class Form1 : Form
    {

        Bitmap bitmap1 = new Bitmap(411, 414);
        Bitmap bitmap2 = new Bitmap(411, 414);
        Pen pen = new Pen(Color.Yellow, 2);
        Boolean GiveBoolForFillColor = false;
        Color Backgroudcolor = Color.Black;
        Graphics g;
        Point penposition;
        private string feedbackMessage;
        private Variables variables = new Variables();
        private IfStatement ifStatement = new IfStatement();
        

        private Loop loop = new Loop();
        string converttostring;
        string storeSinglelineCode;
        string[] convertstostring;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            penposition = new Point(10, 10);
            InitializeComponent();
            g = Graphics.FromImage(bitmap1);
            g.Clear(Color.Black);

        }



        /// <summary>
        /// Event handler for painting the PictureBox.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        public void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(bitmap1, 0, 0);
            g.DrawImageUnscaled(bitmap2, 0, 0);
            e.Graphics.DrawEllipse(pen, penposition.X, penposition.Y, 10, 10);

        }


        /// <summary>
        /// Event handler for the button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

        public void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrEmpty(textBox2.Text))
            {
                Thread thread1 = new Thread(() => ProcessCommands(textBox1.Lines));
                Thread thread2 = new Thread(() => ProcessCommands(textBox3.Lines));

                thread1.Start();
                thread2.Start();

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    thread1 = new Thread(() => ProcessCommands(textBox1.Lines));
                    thread1.Start();
                }
                else if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    thread2 = new Thread(() => ProcessCommands(textBox3.Lines));
                    thread2.Start();
                }
            }
            else if ((!string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text)) || (!string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox1.Text)))
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text))
                {
                    convertstostring = textBox1.Lines;
                }
                if (!string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox1.Text))
                {
                    convertstostring = textBox3.Lines;
                }

                foreach (string commandline in convertstostring)
                {
                    storeSinglelineCode = Convert.ToString(commandline);
                    storeSinglelineCode = storeSinglelineCode.ToLower();
                    string[] addCommandToList = storeSinglelineCode.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (loop.IsLoopStartCommand(storeSinglelineCode))
                    {
                        // Call LoopCommand to handle the loop
                        LoopCommand(storeSinglelineCode);
                    }
                    else
                    {
                        CommandsInCommandLine(addCommandToList);
                    }
                }
            }

            else if (!string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox3.Text))
            {
                converttostring = textBox2.Text.ToString();
                converttostring = converttostring.ToLower();
                string[] getcommand = converttostring.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                CommandsInCommandLine(getcommand);
            }
        }

        private void ProcessCommands(string[] commandLines)
        {
            Point initialPenPosition = penposition;
            Point lastMovetoPosition = initialPenPosition;

            foreach (string commandLine in commandLines)
            {
                string storeSinglelineCode = Convert.ToString(commandLine);
                storeSinglelineCode = storeSinglelineCode.ToLower();
                string[] addCommandToList = storeSinglelineCode.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Invoke(new Action(() =>
                {
                    if (addCommandToList.Length > 0)
                    {
                        for (int i = 0; i < addCommandToList.Length; i++)
                        {
                            if (variables.GetVariable(addCommandToList[i]) != 0)
                            {
                                addCommandToList[i] = variables.GetVariable(addCommandToList[i]).ToString();
                            }
                        }

                        if (addCommandToList[0] == "moveto")
                        {
                            if (addCommandToList.Length == 3)
                            {
                                if (int.TryParse(addCommandToList[1], out int valueforxaxis) &&
                                    int.TryParse(addCommandToList[2], out int valueforyaxis))
                                {
                                    penposition = new Point(valueforxaxis, valueforyaxis);
                                    lastMovetoPosition = penposition; // Update the last moveto position
                                    Thread.Sleep(1000); // Adjust the sleep duration as needed
                                    pictureBox1.Refresh();
                                }
                                else
                                {
                                    SetFeedbackMessage("Invalid arguments for moveto command.");
                                }
                            }
                            else
                            {
                                SetFeedbackMessage("Incorrect amount of arguments for moveto command.");
                            }
                        }
                        else
                        {
                            CommandsInCommandLine(addCommandToList);
                            Thread.Sleep(1000);
                            pictureBox1.Refresh();
                        }
                    }
                }));

                // Move the pen back to the initial position after each set of commands
                penposition = lastMovetoPosition;
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox1.Text))
            {
                string convertToString = textBox2.Text.ToLower();
                string[] commandLines = convertToString.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                List<string> invalidCommands = new List<string>();

                foreach (string commandLine in commandLines)
                {
                    string[] getCommand = commandLine.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string validationMessage = SyntaxChecker.ValidateSyntax(getCommand);

                    if (!validationMessage.StartsWith("Syntax is correct"))
                    {
                        invalidCommands.Add($"Invalid command in line: '{commandLine}' {validationMessage}");
                    }
                }

                if (invalidCommands.Count == 0)
                {
                    // Perform the appropriate action for the commands in textBox2
                    // For example, you can call CommandsInCommandLine(getCommand);
                    MessageBox.Show("Syntax is correct.");
                }
                else
                {
                    MessageBox.Show(string.Join("\n", invalidCommands));
                }
            }
            else if (string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox1.Text))
            {
                string convertToString = textBox1.Text.ToLower();
                string[] commandLines = convertToString.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                List<string> invalidCommands = new List<string>();

                foreach (string commandLine in commandLines)
                {
                    string[] getCommand = commandLine.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string validationMessage = SyntaxChecker.ValidateSyntax(getCommand);

                    if (!validationMessage.StartsWith("Syntax is correct"))
                    {
                        invalidCommands.Add($"Invalid command in line: '{commandLine}' {validationMessage}");
                    }
                }

                if (invalidCommands.Count == 0)
                {
                    // Perform the appropriate action for the commands in textBox1
                    // For example, you can call CommandsInCommandLine(getCommand);
                    MessageBox.Show("Syntax is correct.");
                }
                else
                {
                    MessageBox.Show(string.Join("\n", invalidCommands));
                }
            }
            else
            {
                MessageBox.Show("Please enter commands in either textBox1 or textBox2.");
            }
        }

        /// <summary>
        /// Processes the commands provided in the command line.
        /// </summary>
        /// <param name="listForCommands">The list of commands to be processed.</param>
        public void CommandsInCommandLine(string[] listForCommands)
        {
            Command command = new Command(g, pen, penposition.X, penposition.Y);

            if (listForCommands == null || listForCommands.Length == 0)
            {
                SetFeedbackMessage("No command entered.");
                return;
            }


            else if (listForCommands.Length >= 3 && listForCommands[1].Equals("="))
            {

                // Variable assignment, e.g., x = 50
                string variableName = listForCommands[0];

                // Join all parts after the equal sign, then trim the expression
                string expression = string.Join(" ", listForCommands.Skip(2)).Trim();

                try
                {

                    if (int.TryParse(expression, out int variableValue))
                    {
                        variables.SetVariable(variableName, variableValue);
                    }
                    else
                    {
                        int result = variables.EvaluateExpression(expression);
                        variables.SetVariable(variableName, result);
                    }
                }
                catch (Exception ex)
                {
                    SetFeedbackMessage($"Error evaluating expression: {ex.Message}");
                }
            }
            else
            {
                // Check for variable usage in the command
                for (int i = 0; i < listForCommands.Length; i++)
                {
                    if (variables.GetVariable(listForCommands[i]) != 0)
                    {
                        listForCommands[i] = variables.GetVariable(listForCommands[i]).ToString();
                    }
                }


                switch (listForCommands[0].ToLower())
                {
                    case "if":
                        ExecuteIfStatement(listForCommands);
                        break;

                    case "endif":
                        // Reset the SkipCommands flag
                        ifStatement.SkipCommands = false;
                        break;

                    case "rectangle":
                        if (listForCommands.Length == 3)
                        {
                            if (!ifStatement.SkipCommands)
                            {  // Check if commands should be skipped
                                if (int.TryParse(listForCommands[1], out int valueForWidth) &&
                                int.TryParse(listForCommands[2], out int valueForHeight))
                                {
                                    if (valueForWidth > 0 && valueForHeight > 0)
                                    {
                                        command.DrawRectangle(command, GiveBoolForFillColor, valueForHeight, valueForWidth);
                                        pictureBox1.Refresh();
                                    }
                                    else
                                    {
                                        SetFeedbackMessage("Width and height must be greater than zero for rectangle command.");
                                    }
                                }
                                else
                                {
                                    SetFeedbackMessage("Invalid arguments for rectangle command.");
                                }
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Insufficient arguments for rectangle command.");
                        }
                        break;

                    case "triangle":


                        if (listForCommands.Length == 2 || listForCommands.Length == 4)
                        {
                            // Check if commands should be skipped
                            if (!ifStatement.SkipCommands)
                            {
                                if (int.TryParse(listForCommands[1], out int valueforsidelength))
                                {
                                    if (valueforsidelength > 0)
                                    {
                                        command.DrawTriangle(command, GiveBoolForFillColor, valueforsidelength);
                                        pictureBox1.Refresh();
                                    }
                                    else
                                    {
                                        SetFeedbackMessage("Negative or zero parameter for triangle command.");
                                    }
                                }
                                else
                                {
                                    SetFeedbackMessage("Invalid argument for triangle command.");
                                }
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Incorrect amount of arguments for triangle command.");
                        }
                        break;

                    case "circle":
                        if (listForCommands.Length == 2)
                        {
                            if (!ifStatement.SkipCommands)
                            {// Check if commands should be skipped

                                if (int.TryParse(listForCommands[1], out int valueforradius))
                                {
                                    if (valueforradius > 0)
                                    {
                                        command.DrawCircle(command, GiveBoolForFillColor, valueforradius);
                                        pictureBox1.Refresh();
                                    }
                                    else
                                    {
                                        SetFeedbackMessage("Negative or zero parameter for circle command.");
                                    }
                                }
                                else
                                {
                                    SetFeedbackMessage("Invalid argument for circle command.");
                                }
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Incorrect amount of arguments for circle command.");
                        }
                        break;

                    case "moveto":
                        if (listForCommands.Length == 3)
                        {
                            if (!ifStatement.SkipCommands)
                            {
                                if (int.TryParse(listForCommands[1], out int valueforxaxis) &&
                                    int.TryParse(listForCommands[2], out int valueforyaxis))
                                {
                                    penposition = new Point(valueforxaxis, valueforyaxis);
                                    pictureBox1.Refresh();
                                }
                                else
                                {
                                    SetFeedbackMessage("Invalid arguments for moveto command.");
                                }
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Incorrect amount of arguments for moveto command.");
                        }
                        break;

                    case "drawto":
                        if (listForCommands.Length == 3)
                        {
                            if (!ifStatement.SkipCommands)
                            {
                                if (int.TryParse(listForCommands[1], out int valueforxaxis) &&
                                int.TryParse(listForCommands[2], out int valueforyaxis))
                                {
                                    command.DrawTo(command, valueforxaxis, valueforyaxis);
                                    penposition = new Point(valueforxaxis, valueforyaxis);
                                    pictureBox1.Refresh();
                                }
                                else
                                {
                                    SetFeedbackMessage("Invalid arguments for drawto command.");
                                }
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Incorrect amount of arguments for drawto");
                        }
                        break;

                    case "pen":
                        if (listForCommands.Length == 2)
                        {
                            if (!ifStatement.SkipCommands)
                            {
                                try
                                {
                                    if (listForCommands[1].Equals("yellow") || listForCommands[1].Equals("white") ||
                                        listForCommands[1].Equals("red") || listForCommands[1].Equals("green") ||
                                        listForCommands[1].Equals("blue") || listForCommands[1].Equals("pink") ||
                                        listForCommands[1].Equals("purple") || listForCommands[1].Equals("orange"))
                                    {
                                        command.PenColor(listForCommands[1], pen);
                                    }
                                    else
                                    {
                                        SetFeedbackMessage("Try entering one of these colors: green, blue, pink, white, yellow, orange, red, purple");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SetFeedbackMessage("An error occurred: " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Invalid number of parameters for the 'pen' command.");
                        }
                        break;

                    case "fill":
                        if (listForCommands.Length == 2)
                        {
                            if (!ifStatement.SkipCommands)
                            {
                                try
                                {
                                    if (command.Fill(listForCommands[1]))
                                    {
                                        GiveBoolForFillColor = true;
                                    }
                                    else
                                    {
                                        GiveBoolForFillColor = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SetFeedbackMessage("An error occurred: " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Incorrect amount of arguments for fill command.");
                        }
                        break;
                    case "while":
                        if (!ifStatement.SkipCommands)
                        {
                            LoopCommand(converttostring);
                        }
                        break;
                    case "endloop":
                        if (!ifStatement.SkipCommands)
                        {
                            //continue;
                        }
                        break;
                    

                    case "reset":
                        if (listForCommands.Length == 1)
                        {
                            penposition = new Point(10, 10);
                            g.Clear(Color.Black);
                            pictureBox1.Refresh();
                        }
                        else
                        {
                            SetFeedbackMessage("Reset does not take arguments.");
                        }
                        break;

                    case "clear":
                        if (listForCommands.Length == 1)
                        {
                            g.Clear(Color.Black);
                            pictureBox1.Refresh();
                        }
                        else
                        {
                            SetFeedbackMessage("Clear does not take arguments.");
                        }
                        break;

                    default:
                        if (!ifStatement.SkipCommands)  // Check if commands should be skipped
                        {
                            SetFeedbackMessage($"Invalid command: {listForCommands[0]}");
                        }
                        break;
                }
            }
        }

        public void LoopCommand(string command)
        {
            try
            {
                if (!ifStatement.SkipCommands)
                {
                    if (loop.IsLoopStartCommand(command))
                    {
                        int loopCount = loop.GetLoopCount(command);
                        int startIndex = Array.IndexOf(textBox1.Lines, command);

                        while (loopCount > 0)
                        {
                            for (int i = startIndex + 1; i < textBox1.Lines.Length; i++)
                            {
                                string loopCommand = textBox1.Lines[i].ToLower();

                                if (loop.IsLoopStartCommand(loopCommand))
                                {
                                    loopCount++;
                                }
                                else if (loopCommand.Contains("endloop"))
                                {
                                    loopCount--;
                                }

                                if (loopCount == 0)
                                {
                                    break;
                                }

                                storeSinglelineCode = loopCommand;
                                storeSinglelineCode = storeSinglelineCode.ToLower();
                                string[] lines = storeSinglelineCode.Split('\n');

                                if (lines.Length > 0 && !string.IsNullOrWhiteSpace(lines[0]))
                                {
                                    string[] getloopcommand = lines[0].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    CommandsInCommandLine(getloopcommand);
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                SetFeedbackMessage($"Error in loop command: {ex.Message}");
            }
            catch (Exception ex)
            {
                SetFeedbackMessage($"An unexpected error occurred: {ex.Message}");
            }
        }



        private void ExecuteIfStatement(string[] listForCommands)
        {
            if (listForCommands.Length == 4)
            {
                if (int.TryParse(listForCommands[1], out int operand1) &&
                    int.TryParse(listForCommands[3], out int operand2))
                {
                    ifStatement.CheckCondition(operand1, operand2, listForCommands[2]);
                }
                else
                {
                    SetFeedbackMessage("Invalid operands for the if statement.");
                }
            }
            else
            {
                SetFeedbackMessage("Incorrect number of arguments for the if statement.");
            }

            if (!ifStatement.ConditionMet)
            {
                // Skip the commands inside if/endif block
                SetFeedbackMessage("Condition not met. Skipping commands inside if statement.");
                ifStatement.SkipCommands = true;
            }
            // else, condition is met, so continue with execution
        }




        private void SetFeedbackMessage(string message)
        {
            feedbackMessage = message;
            // Display the feedback message, for example, in a label or MessageBox
            MessageBox.Show(message);
        }


        /// <summary>
        /// Gets the current pen position on the canvas.
        /// </summary>
        public Point PenPosition
        {
            get { return penposition; }
        }


        // <summary>
        /// Gets the background color of the canvas.
        /// </summary>
        public Color BackgroundColor
        {
            get { return Backgroudcolor; }
        }

        // <summary>
        /// Gets the TextBox control associated with the Form.
        /// </summary>
        public TextBox TextBox1
        {
            get { return textBox1; }
        }


        // <summary>
        /// Gets the feedback message generated by the application.
        /// </summary>
        public string FeedbackMessage
        {
            get { return feedbackMessage; }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        // <summary>
        /// Event handler for the button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "\"Text Files (.txt)|*.txt|All Files|*.*";
            openFileDialog.DefaultExt = "txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        g.Clear(Color.Black);
                        while (!reader.EndOfStream)
                        {

                            string command = reader.ReadLine();
                            string[] singlecommand = command.Split(' ');
                            CommandsInCommandLine(singlecommand);
                        }
                    }
                    MessageBox.Show("Successfully Loaded File");
                }
                catch
                {
                    MessageBox.Show("ERROR LOADING THE FILE");

                }
            }



        }

        /// <summary>
        /// Event handler for the button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void button4_Click(object sender, EventArgs e)
        {


            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                string[] storeLineInFile = textBox1.Lines;

                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (string commandline in storeLineInFile)
                        {
                            writer.WriteLine(commandline);
                        }
                    }

                    MessageBox.Show("FILE ADDED");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR IN SAVING THE FILE");
                }

            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

    }
}