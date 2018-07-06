using System;
using System.Collections.Generic;
using PL.BCE.BLL.Calculator;
using PL.BCE.Data;

namespace PL.BCE.BLL
{
    public static class RuleFactory
    {
        public static List<ICalcRule> BuildCalcRules(Person person)
        {
            var ruleList = new List<ICalcRule>();

            if (person.FirstName.StartsWith("A", StringComparison.OrdinalIgnoreCase))
            {
                ruleList.Add(new Discount(CalcAmountType.Percentage, 0.10m){Priority = 1});
            }

            return ruleList;
        }
    }
}
