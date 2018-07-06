using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PL.BCE.Data
{
    public class BenefitDetail
    {
        [DisplayName("Base Cost")]
        public decimal BaseAmount { get; set; }
        [DisplayName("Benefit Cost (Year)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal AmountPerYear { get; set; }
    }
}
