namespace PL.BCE.BLL.Calculator
{
    public class Fee : RuleBase
    {
        public Fee(CalcAmountType _amountType, decimal _amount) : base(_amountType, _amount)
        {
        }

        public override CalcRuleType RuleType => CalcRuleType.Fee;
    }
}
