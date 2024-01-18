using ASEGraphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ASEGraphics
{
    [TestClass]
    public class SyntaxCheckerTest
    {
        [TestMethod]
        public void ValidateSyntax_ValidRectangleCommand_ReturnsCorrectMessage()
        {
            // Arrange
            string[] commands = { "rectangle", "10", "20" };

            // Act
            string result = SyntaxChecker.ValidateSyntax(commands);

            // Assert
            Assert.AreEqual("Syntax is correct.", result);
        }

        [TestMethod]
        public void ValidateSyntax_InvalidRectangleCommand_ReturnsErrorMessage()
        {
            // Arrange
            string[] commands = { "rectangle", "invalidWidth", "20" };

            // Act
            string result = SyntaxChecker.ValidateSyntax(commands);

            // Assert
            Assert.AreEqual("Invalid syntax for rectangle command.", result);
        }

        [TestMethod]
        public void ValidateSyntax_ValidTriangleCommand_ReturnsCorrectMessage()
        {
            // Arrange
            string[] commands = { "triangle", "5" };

            // Act
            string result = SyntaxChecker.ValidateSyntax(commands);

            // Assert
            Assert.AreEqual("Syntax is correct.", result);
        }

        [TestMethod]
        public void ValidateSyntax_InvalidTriangleCommand_ReturnsErrorMessage()
        {
            // Arrange
            string[] commands = { "triangle", "invalidSide" };

            // Act
            string result = SyntaxChecker.ValidateSyntax(commands);

            // Assert
            Assert.AreEqual("Invalid syntax for triangle command.", result);
        }

        // Add more test methods for other commands as needed
    }
}