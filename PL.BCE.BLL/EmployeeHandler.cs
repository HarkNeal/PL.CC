using System;
using System.Collections.Generic;
using System.Linq;
using PL.BCE.BLL.Calculator;
using PL.BCE.Data;

namespace PL.BCE.BLL
{
    public class EmployeeHandler : IEmployeeHandler
    {
        public Employee CreateNewEmployee()
        {
            return new Employee();
        }

        private void GenerateBenefitDefaults(List<Person> allIndividuals)
        {
            allIndividuals.ForEach(ai => ai.BenefitDetail = GenerateBenefitDefaults(ai));
        }

        internal BenefitDetail GenerateBenefitDefaults(Person person)
        {
            var baseAmount = person.PersonType == PersonTypes.Employee ? 1000m : 500m;
            return new BenefitDetail
            {
                BaseAmount = baseAmount
            };
        }

        public BenefitCostSummary GenerateBenefitEstimate(Employee employee)
        {
            //flatten employee hierarchy
            var allIndividuals = new List<Person>();
            allIndividuals.Add(employee);
            if (employee.Dependents?.Any() ?? false)
            {
                allIndividuals.AddRange(employee.Dependents);
            }

            GenerateBenefitDefaults(allIndividuals);
            GenerateEstimates(allIndividuals);

            var numberOfPayPeriods = GetNumberOfPeriods(employee.PeriodType);
            var yearlyTotal = allIndividuals.Sum(ai => ai.BenefitDetail.AmountPerYear);
            return new BenefitCostSummary()
            {
                Employee = employee,
                NumberOfPayPeriods = numberOfPayPeriods,
                TotalPerYear = yearlyTotal,
                TotalPerPeriod = Math.Round(yearlyTotal / numberOfPayPeriods, 2, MidpointRounding.AwayFromZero)
            };
        }

        private int GetNumberOfPeriods(PeriodType employeePeriodType)
        {
            switch(employeePeriodType)
            {
                case PeriodType.Monthly:
                    return 12;
                case PeriodType.Weekly:
                    return 52;
                case PeriodType.BiWeekly:
                default:
                    return 26;
            }
        }

        private void GenerateEstimates(List<Person> allIndividuals)
        {
            var calcEngine = new CalcEngine();
            allIndividuals.ForEach(ai =>
            {
                var calcRules = RuleFactory.BuildCalcRules(ai);
                ai.BenefitDetail.AmountPerYear = calcEngine.GenerateCostEstimate(ai.BenefitDetail.BaseAmount, calcRules);
            });
        }
    }
}
