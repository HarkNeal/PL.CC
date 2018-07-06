using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PL.BCE.Data
{
    public class BenefitCostSummary
    {
        public Employee Employee { get; set; }
        [DisplayName("Cost Per Pay Period")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPerPeriod { get; set; }
        [DisplayName("Cost Per Year")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPerYear { get; set; }
        [DisplayName("Number of Pay Periods")]
        public int  NumberOfPayPeriods { get; set; }

    }
}
