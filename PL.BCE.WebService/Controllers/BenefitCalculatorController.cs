using System.Web.Http;
using PL.BCE.BLL;
using PL.BCE.Data;

namespace PL.BCE.WebService.Controllers
{
    [RoutePrefix("BenefitCalculator")]
    public class BenefitCalculatorController : ApiController
    {
        // GET: BenefitCalculator
        [AcceptVerbs("GET", "POST")]
        [Route("")]
        public IHttpActionResult Index()
        {
            return Ok(new Employee());
        }

        [Route("GenerateEstimate")]
        [HttpPost]
        public IHttpActionResult GenerateBenefitEstimate(Employee employee)
        {
            return Ok(new EmployeeHandler().GenerateBenefitEstimate(employee));
        }
    }
}