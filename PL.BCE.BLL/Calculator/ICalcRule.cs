namespace PL.BCE.BLL.Calculator
{
    public interface ICalcRule
    {
        CalcRuleType RuleType { get; }
        CalcAmountType AmountType { get; }
        decimal Amount { get; set; }
        int Priority { get; set; }
        decimal Apply(decimal costBasis);
    }
}