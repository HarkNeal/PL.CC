using System.Collections.Generic;

namespace PL.BCE.BLL.Calculator
{
    public interface ICalcEngine
    {
        decimal GenerateCostEstimate(decimal baseAmount, List<ICalcRule> calcRules);
    }
}