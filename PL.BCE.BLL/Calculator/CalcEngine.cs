using System;
using System.Collections.Generic;
using System.Linq;

namespace PL.BCE.BLL.Calculator
{
    public class CalcEngine : ICalcEngine
    {
        public decimal GenerateCostEstimate(decimal baseAmount, List<ICalcRule> calcRules)
        {
            decimal costEstimate = baseAmount;

            calcRules = calcRules ?? new List<ICalcRule>();

            calcRules.OrderBy(cr => cr.Priority).ToList().ForEach(cr => { costEstimate = cr.Apply(costEstimate); });

            return Math.Round(costEstimate, 2, MidpointRounding.AwayFromZero);
        }
    }
}
