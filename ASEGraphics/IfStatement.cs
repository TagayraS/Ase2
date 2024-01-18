using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASEGraphics
{
    public class IfStatement
    {
        private bool conditionMet;
        private bool skipCommands;

        public IfStatement()
        {
            conditionMet = false;
            skipCommands = false;
        }

        public void CheckCondition(int operand1, int operand2, string comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case "<":
                    conditionMet = operand1 < operand2;
                    break;
                case ">":
                    conditionMet = operand1 > operand2;
                    break;
                case "==":
                    conditionMet = operand1 == operand2;
                    break;
                case "<=":
                    conditionMet = operand1 <= operand2;
                    break;
                case ">=":
                    conditionMet = operand1 >= operand2;
                    break;
                case "!=":
                    conditionMet = operand1 != operand2;
                    break;
                default:
                    // Handle invalid comparison operator
                    conditionMet = false;
                    break;
            }
        }

        public bool ConditionMet
        {
            get { return conditionMet; }
        }

        public bool SkipCommands
        {
            get { return skipCommands; }
            set { skipCommands = value; }
        }
    }
}