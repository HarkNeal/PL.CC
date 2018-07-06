namespace PL.BCE.BLL.Calculator
{
    public abstract class RuleBase : ICalcRule
    {
        public abstract CalcRuleType RuleType { get; }
        public CalcAmountType AmountType { get; }
        public decimal Amount { get; set; }
        public int Priority { get; set; }

        public RuleBase(CalcAmountType _amountType, decimal _amount)
        {
            AmountType = _amountType;
            Amount = _amount;
        }

        public virtual decimal Apply(decimal costBasis)
        {
            decimal adjustmentAmount = 0;
            if (AmountType == CalcAmountType.Flat)
            {
                adjustmentAmount = Amount;
            } else if (AmountType == CalcAmountType.Percentage)
            {
                adjustmentAmount = costBasis * Amount;
            }

            if (RuleType == CalcRuleType.Discount)
            {
                adjustmentAmount = -(adjustmentAmount);
            }

            return costBasis + adjustmentAmount;
        }
    }
}