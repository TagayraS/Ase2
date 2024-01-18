using ASEGraphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace ASEGraphicsTest
{
    [TestClass]
    //for unit test of some commands
    public class CommandTest
    {

        private Graphics g;
        private Pen pen;
        private Command command;

        [TestInitialize]
        public void Setup()
        {
            g = Graphics.FromImage(new Bitmap(400, 400));
            pen = new Pen(Color.Black, 2);
            command = new Command(g, pen, 0, 0);
        }
        [TestMethod]
        public void TestDrawTo()
        {
            // Arrange
            Graphics graphics = Graphics.FromImage(new Bitmap(400, 400));
            Pen pen = new Pen(Color.Black, 2);
            Command command = new Command(graphics, pen, 0, 0);

            // Act and Assert for failure
            Assert.ThrowsException<ArgumentException>(() =>
            {
                command.DrawTo(command, -1, -1);
            });

            // Act and Assert for success
            try
            {
                command.DrawTo(command, 100, 100);
                // If no exception is thrown, the test is successful
                Assert.IsTrue(true);
            }
            catch (ArgumentException)
            {
                // If an exception is thrown, the test fails
                Assert.Fail("Expected no exception, but ArgumentException was thrown.");
            }
        }
        [TestMethod]
        public void TestFill()
        {
            // Act
            bool resultOn = command.Fill("on");
            bool resultOff = command.Fill("off");

            // Assert
            Assert.IsTrue(resultOn);
            Assert.IsFalse(resultOff);

            // Additional assertion for wrong command
            Assert.ThrowsException<ArgumentException>(() => command.Fill("invalidCommand"));
        }
        [TestMethod]
        public void TestPenColor()
        {
            // Act
            Color color = command.PenColor("red", pen);

            // Assert
            Assert.AreEqual(Color.Red, color);

            // Additional assertion for wrong command
            Assert.ThrowsException<ArgumentException>(() => command.PenColor("invalidColor", pen));
        }
        [TestMethod]
        public void TestDrawCircle()
        {
            // Act
            command.DrawCircle(command, true, 10);
            command.DrawCircle(command, false, 15);

            // Assert

            // Check if outlined circle is drawn
            Assert.AreEqual(Color.Black, command.pen.Color);

            // Additional assertions for wrong parameters
            Assert.ThrowsException<ArgumentException>(() => command.DrawCircle(command, true, "fiftene")); // Invalid radius value (string)
            Assert.ThrowsException<ArgumentException>(() => command.DrawCircle(command, true, -5)); // Negative radius value (string)
        }
        //test for the shape rectangle
        [TestMethod]
        public void TestDrawRectangle()
        {
            // Act
            command.DrawRectangle(command, true, 20, 30);
            command.DrawRectangle(command, false, 25, 35);

            // Assert

            // Check if the outlined rectangle is drawn
            Assert.AreEqual(Color.Black, command.pen.Color);

            // Additional assertions for wrong parameters
            Assert.ThrowsException<ArgumentException>(() => command.DrawRectangle(command, true, -10, 15)); // Negative height
            Assert.ThrowsException<ArgumentException>(() => command.DrawRectangle(command, true, 10, -15)); // Negative width
            Assert.ThrowsException<FormatException>(() => command.DrawRectangle(command, true, int.Parse("abc"), 15)); // Invalid height (string)
            Assert.ThrowsException<FormatException>(() => command.DrawRectangle(command, true, 10, int.Parse("def"))); // Invalid width (string)
        }
        [TestMethod]
        public void TestDrawTriangle()
        {
            // Act
            command.DrawTriangle(command, true, 15);
            command.DrawTriangle(command, false, 20);


            // Check if the outlined triangle is drawn
            Assert.AreEqual(Color.Black, command.pen.Color);

            // Additional assertions for wrong parameters
            Assert.ThrowsException<ArgumentException>(() => command.DrawTriangle(command, true, "ab2"));
            Assert.ThrowsException<ArgumentException>(() => command.DrawTriangle(command, true, -8));

        }
    }
}