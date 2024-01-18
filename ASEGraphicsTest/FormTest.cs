using ASEGraphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace ASEGraphicsTest
{
    [TestClass]
    public class FormTest
    {
        [TestMethod]
        public void TestReset()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.CommandsInCommandLine(new string[] { "reset" });

            // Assert
            Assert.AreEqual(new Point(10, 10), form.PenPosition);
            Assert.AreEqual(Color.Black, form.BackgroundColor);
        }
        [TestMethod]
        public void TestMoveTo()
        {
            // Arrange
            var form = new Form1();

            // Act - Success case
            form.CommandsInCommandLine(new string[] { "moveto", "50", "50" });

            // Assert - Success case
            Assert.AreEqual(new Point(50, 50), form.PenPosition);

            // Act - Invalid arguments case
            form.CommandsInCommandLine(new string[] { "moveto", "invalid", "arguments" });

            // Assert - Invalid arguments case
            Assert.AreEqual("Invalid arguments for moveto command.", form.FeedbackMessage);

            // Act - Insufficient arguments case
            form.CommandsInCommandLine(new string[] { "moveto", "50" });

            // Assert - Insufficient arguments case
            Assert.AreEqual("Incorrect amount of arguments for moveto command.", form.FeedbackMessage);
        }
        [TestMethod]
        public void TestClear()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.CommandsInCommandLine(new string[] { "clear" });

            // Assert
            Assert.AreEqual(Color.Black, form.BackgroundColor);
        }
        [TestMethod]
        public void TestSuccessfulMultiLineCommands()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.TextBox1.Text = "rectangle 20 30\nmoveto 50 50\ncircle 15";
            form.button1_Click(null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(new Point(50, 50), form.PenPosition);
            Assert.AreEqual(Color.Black, form.BackgroundColor);
        }
        [TestMethod]
        public void TestSuccessfulSingleCommandLine()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.TextBox1.Text = "rectangle 20 30";
            form.button1_Click(null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(new Point(10, 10), form.PenPosition);
            Assert.AreEqual(Color.Black, form.BackgroundColor);
        }
        //test for unsucessful single line command
        [TestMethod]
        public void TestUnsuccessfulSingleCommandLine()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.TextBox1.Text = "invalid_command 20 30";
            form.button1_Click(null, EventArgs.Empty);

            // Assert
            // Check for feedback or error handling in your application
            // For example, you might check a label or display for an error message
            Assert.AreEqual("Invalid command: invalid_command", form.FeedbackMessage);
        }
        //test for unsucessful command
        [TestMethod]
        public void TestUnsuccessfulMultiLineCommands()
        {
            // Arrange
            var form = new Form1();

            // Act
            form.TextBox1.Text = "rectangle 20 30\nmoveto invalid arguments\ncircle 100";
            form.button1_Click(null, EventArgs.Empty);

            // Assert
            // Check if the feedback message indicates an error for the invalid command
            Assert.AreEqual("Invalid arguments for moveto command.", form.FeedbackMessage);


            // Check if the pen position and background color remain unchanged
            Assert.AreEqual(new Point(10, 10), form.PenPosition);
            Assert.AreEqual(Color.Black, form.BackgroundColor);
        }

        [TestMethod]
        public void TestFileLoading()
        {
            // Arrange
            var form = new Form1();
            string testFilePath = "test_commands.txt";

            try
            {
                // Create a test file with some commands
                using (StreamWriter writer = new StreamWriter(testFilePath))
                {
                    writer.WriteLine("rectangle 20 30");
                    writer.WriteLine("moveto 50 50");
                    writer.WriteLine("circle 15");
                }

                // Act - Load and execute commands from the file
                form.button3_Click(null, EventArgs.Empty);

                // Assert - Check the result after loading and executing commands
                Assert.AreEqual(new Point(50, 50), form.PenPosition);
                Assert.AreEqual(Color.Black, form.BackgroundColor);
            }
            finally
            {
                // Clean up - delete the test file
                File.Delete(testFilePath);
            }
        }

        [TestMethod]
        public void TestFileSaving()
        {
            // Arrange
            var form = new Form1();
            string testFilePath = "test_save.txt";

            try
            {
                // Execute some commands in the application
                form.TextBox1.Text = "rectangle 20 30\nmoveto 50 50\ncircle 15";
                form.button1_Click(null, EventArgs.Empty);

                // Act - Save the commands to a file
                form.button4_Click(null, EventArgs.Empty);

                // Assert - Check if the file has been saved
                Assert.IsTrue(File.Exists(testFilePath));

                // Read the saved file and verify its content
                string[] savedCommands = File.ReadAllLines(testFilePath);
                Assert.AreEqual(3, savedCommands.Length);
                Assert.AreEqual("rectangle 20 30", savedCommands[0]);
                Assert.AreEqual("moveto 50 50", savedCommands[1]);
                Assert.AreEqual("circle 15", savedCommands[2]);
            }
            finally
            {
                // Clean up - delete the test file
                File.Delete(testFilePath);
            }
        }

        [TestMethod]
        public void TestIfStatementConditionNotMet()
        {
            // Arrange
            var form = new Form1();


            // Act
            form.CommandsInCommandLine(new string[] { "x", "=", "10" });
            form.CommandsInCommandLine(new string[] { "y", "=", "20" });
            form.CommandsInCommandLine(new string[] { "if", "x", ">", "y" });
            form.CommandsInCommandLine(new string[] { "moveto", "100", "120" });
            form.CommandsInCommandLine(new string[] { "endif" });

            // Assert
            Assert.AreEqual("Condition not met. Skipping commands inside if statement.", form.FeedbackMessage);
        }

        [TestMethod]
        public void TestIfStatementConditionMet()
        {
            // Arrange
            var form = new Form1();


            // Act
            form.CommandsInCommandLine(new string[] { "x", "=", "30" });
            form.CommandsInCommandLine(new string[] { "y", "=", "20" });
            form.CommandsInCommandLine(new string[] { "if", "x", ">", "y" });
            form.CommandsInCommandLine(new string[] { "moveto", "100", "120" });
            form.CommandsInCommandLine(new string[] { "endif" });

            // Assert
            Assert.AreEqual(new Point(100, 120), form.PenPosition);

        }

        [TestClass]
        public class LoopTests
        {
            [TestMethod]
            public void TestLoopCommands()
            {
                // Arrange
                Loop loop = new Loop();

                // Act
                bool isLoopStart = loop.IsLoopStartCommand("while count < 5");
                bool isLoopEnd = loop.IsLoopEndCommand("endloop");
                int loopCount = loop.GetLoopCount("while count < 5");

                // Assert
                Assert.IsTrue(isLoopStart);
                Assert.IsTrue(isLoopEnd);
                Assert.AreEqual(5, loopCount);
            }
        }
    }
}