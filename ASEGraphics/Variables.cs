using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ASEGraphics

{
    public class Variables
    {
        private Dictionary<string, int> variableDictionary;

        public Variables()
        {
            variableDictionary = new Dictionary<string, int>();
        }

        public void SetVariable(string variableName, int value)
        {

            if (variableDictionary.ContainsKey(variableName))
            {
                variableDictionary[variableName] = value;
            }
            else
            {

                variableDictionary.Add(variableName, value);
            }
        }

        public int GetVariable(string variableName)
        {

            return variableDictionary.TryGetValue(variableName, out int value) ? value : 0;
        }

        public int EvaluateExpression(string expression)
        {
            // Remove spaces from the expression
            expression = expression.Replace(" ", "");

            foreach (var variable in variableDictionary)
            {
                expression = expression.Replace(variable.Key, variable.Value.ToString());
            }

            try
            {
                return EvaluateSimpleExpression(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error evaluating expression: {ex.Message}");
            }
        }

        private int EvaluateSimpleExpression(string expression)

        {
            // Split the expression into operands and operator
            string[] parts = expression.Split(new char[] { '+', '-', '*', '/', '%' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                throw new InvalidOperationException("Invalid expression format.");
            }

            // Parse the operands
            if (!int.TryParse(parts[0], out int operand1) || !int.TryParse(parts[1], out int operand2))
            {
                throw new InvalidOperationException("Invalid operands in the expression.");
            }

            // Perform the operation based on the operator
            if (expression.Contains('+'))
            {
                return operand1 + operand2;
            }
            else if (expression.Contains('-'))
            {
                return operand1 - operand2;
            }
            else if (expression.Contains('*'))
            {
                return operand1 * operand2;
            }
            else if (expression.Contains('/'))
            {
                return operand1 / operand2;
            }
            else if (expression.Contains('%'))
            {
                return operand1 % operand2;
            }
            else
            {
                throw new InvalidOperationException("Invalid operator in the expression.");
            }
        }

        public void ClearVariables()
        {
            variableDictionary.Clear();
        }
    }
}