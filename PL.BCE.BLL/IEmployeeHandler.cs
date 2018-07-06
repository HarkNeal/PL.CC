using PL.BCE.Data;

namespace PL.BCE.BLL
{
    public interface IEmployeeHandler
    {
        Employee CreateNewEmployee();
        BenefitCostSummary GenerateBenefitEstimate(Employee employee);
    }
}
