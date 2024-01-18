using ASEGraphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ASEGraphicsTest
{
    [TestClass]
    public class VariablesTests
    {
        [TestMethod]
        public void SetVariable_ShouldSetVariable()
        {
            // Arrange
            var variables = new Variables();

            // Act
            variables.SetVariable("x", 10);

            // Assert
            Assert.AreEqual(10, variables.GetVariable("x"));
        }

        [TestMethod]
        public void SetVariable_ShouldUpdateVariable()
        {
            // Arrange
            var variables = new Variables();

            // Act
            variables.SetVariable("x", 10);
            variables.SetVariable("x", 20);

            // Assert
            Assert.AreEqual(20, variables.GetVariable("x"));
        }

        [TestMethod]
        public void GetVariable_ShouldReturnZeroForUndefinedVariable()
        {
            // Arrange
            var variables = new Variables();

            // Act
            int result = variables.GetVariable("undefinedVariable");

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void EvaluateExpression_ShouldEvaluateSimpleExpression()
        {
            // Arrange
            var variables = new Variables();

            // Act
            variables.SetVariable("x", 5);
            int result = variables.EvaluateExpression("x + 3");

            // Assert
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EvaluateExpression_ShouldThrowExceptionForInvalidExpression()
        {
            // Arrange
            var variables = new Variables();

            // Act
            int result = variables.EvaluateExpression("invalidExpression");
        }

        [TestMethod]
        public void ClearVariables_ShouldClearAllVariables()
        {
            // Arrange
            var variables = new Variables();
            variables.SetVariable("x", 10);

            // Act
            variables.ClearVariables();

            // Assert
            Assert.AreEqual(0, variables.GetVariable("x"));
        }
    }
}