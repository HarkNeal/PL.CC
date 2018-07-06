using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PL.BCE.Data
{
    public class Employee : Person
    {
        [DisplayName("Dependents")]
        public List<Person> Dependents { get; set; }

        [DisplayName("Salary Per Period")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal SalaryPerPeriod { get; set; }
        [DisplayName("Period Type")]
        public PeriodType PeriodType { get; set; }

        public Employee() : base()
        {
            PersonType = PersonTypes.Employee;
            Dependents = new List<Person>();
        }
    }

    public enum PeriodType
    {
        Weekly = 1,
        BiWeekly = 2,
        Monthly = 3
    }
}
