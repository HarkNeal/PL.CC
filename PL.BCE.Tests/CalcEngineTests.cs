using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL.BCE.BLL.Calculator;

namespace PL.BCE.Tests
{
    [TestClass]
    public class CalcEngineTests
    {
        private ICalcRule CreateCalcRule(CalcRuleType _ruleType, CalcAmountType _amountType, decimal _amount, int _priority)
        {
            ICalcRule ruleToCreate;
            switch (_ruleType)
            {
                case CalcRuleType.Discount:
                    ruleToCreate = new Discount(_amountType, _amount);
                    break;
                case CalcRuleType.Fee:
                default:
                    ruleToCreate = new Fee(_amountType, _amount);
                    break;
            }

            ruleToCreate.Priority = _priority;
            return ruleToCreate;
        }

        [TestMethod]
        public void CalcEngineTests_NoCalcRules_ShouldReturnOriginalAmount()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>());

            Assert.AreEqual(baseAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_NullCalcRules_ShouldReturnOriginalAmount()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, null);

            Assert.AreEqual(baseAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_SimpleDiscountPercentage_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal discountPercent = .20m;

            ICalcRule discountRule = CreateCalcRule(CalcRuleType.Discount, CalcAmountType.Percentage, discountPercent, 1);

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() {discountRule});

            Assert.AreEqual(baseAmount - (baseAmount * discountPercent), newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_MultipleDiscountPercentage_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal discountPercentOne = .20m;
            const decimal discountPercentTwo = .10m;

            ICalcRule discountRuleOne = CreateCalcRule(CalcRuleType.Discount, CalcAmountType.Percentage, discountPercentOne, 1);
            ICalcRule discountRuleTwo = CreateCalcRule(CalcRuleType.Discount, CalcAmountType.Percentage, discountPercentTwo, 2);

            decimal calcCheckAmount = baseAmount - (baseAmount * discountPercentOne);
            calcCheckAmount -= (calcCheckAmount * discountPercentTwo);

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() { discountRuleOne, discountRuleTwo });

            Assert.AreEqual(calcCheckAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_MultipleDiscountPercentageAndFlat_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal discountPercentOne = .20m;
            const decimal discountFlatAmount = 75m;

            ICalcRule discountRuleOne = CreateCalcRule(CalcRuleType.Discount, CalcAmountType.Percentage, discountPercentOne, 1);
            ICalcRule discountRuleTwo = CreateCalcRule(CalcRuleType.Discount, CalcAmountType.Flat, discountFlatAmount, 2);

            decimal calcCheckAmount = baseAmount - (baseAmount * discountPercentOne);
            calcCheckAmount -= discountFlatAmount;

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() { discountRuleOne, discountRuleTwo });

            Assert.AreEqual(calcCheckAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_SimpleFlatFee_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal feeAmount = 50m;

            ICalcRule feeRule = CreateCalcRule(CalcRuleType.Fee, CalcAmountType.Flat, feeAmount, 1);

            var calcCheckAmount = baseAmount + feeAmount;

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() { feeRule });

            Assert.AreEqual(calcCheckAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_SimplePercentagetFee_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal feeAmount = .50m;

            ICalcRule feeRule = CreateCalcRule(CalcRuleType.Fee, CalcAmountType.Percentage, feeAmount, 1);

            var calcCheckAmount = baseAmount + (baseAmount * feeAmount);

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() { feeRule });

            Assert.AreEqual(calcCheckAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_MultipleFees_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal feeAmountOne = 50m;
            const decimal feePercentageTwo = .50m;

            ICalcRule feeRule = CreateCalcRule(CalcRuleType.Fee, CalcAmountType.Flat, feeAmountOne, 1);
            ICalcRule feeRuleTwo = CreateCalcRule(CalcRuleType.Fee, CalcAmountType.Percentage, feePercentageTwo, 2);

            var afterFirstRule = baseAmount + feeAmountOne;
            var calcCheckAmount = afterFirstRule + (afterFirstRule * feePercentageTwo);

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() { feeRule, feeRuleTwo });

            Assert.AreEqual(calcCheckAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_FeeBeforeDiscount_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal feeAmountOne = 50m;
            const decimal discountAmountPercentage = .50m;

            ICalcRule feeRule = CreateCalcRule(CalcRuleType.Fee, CalcAmountType.Flat, feeAmountOne, 1);
            ICalcRule discountRule = CreateCalcRule(CalcRuleType.Discount, CalcAmountType.Percentage, discountAmountPercentage, 2);

            var afterFirstRule = baseAmount + feeAmountOne;
            var calcCheckAmount = afterFirstRule - (afterFirstRule * discountAmountPercentage);

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() { feeRule, discountRule });

            Assert.AreEqual(calcCheckAmount, newAmount);
        }

        [TestMethod]
        public void CalcEngineTests_FeeAfterDiscount_AssertCalculationIsCorrect()
        {
            CalcEngine calcEngine = new CalcEngine();
            const decimal baseAmount = 250.00m;
            const decimal discountPercentage = 50m;
            const decimal feeAmount = 100m;

            ICalcRule discountRule = CreateCalcRule(CalcRuleType.Discount, CalcAmountType.Percentage, discountPercentage, 1);
            ICalcRule feeRule = CreateCalcRule(CalcRuleType.Fee, CalcAmountType.Flat, feeAmount, 2);

            var afterFirstRule = baseAmount - (baseAmount * discountPercentage);
            var calcCheckAmount = afterFirstRule + feeAmount;

            decimal newAmount = calcEngine.GenerateCostEstimate(baseAmount, new List<ICalcRule>() { discountRule, feeRule });

            Assert.AreEqual(calcCheckAmount, newAmount);
        }
    }
}
