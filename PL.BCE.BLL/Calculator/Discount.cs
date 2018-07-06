namespace PL.BCE.BLL.Calculator
{
    public class Discount : RuleBase
    {
        public override CalcRuleType RuleType => CalcRuleType.Discount;
        
        public Discount(CalcAmountType _amountType, decimal _amount) : base(_amountType, _amount)
        {
        }
    }
}
