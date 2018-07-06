using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using PL.BCE.Data;

namespace PL.BCE.Web.Controllers
{
    public class BenefitEstimatorController : Controller
    {
        // GET: BenefitEstimator
        [HttpGet]
        public ActionResult Index()
        {
            Employee employee = new Employee()
            {
                SalaryPerPeriod = 2000m,
                PeriodType = PeriodType.BiWeekly
            };
            return View(employee);
        }

        [HttpPost]
        public ActionResult Index(Employee employee)
        {
            return ViewEmployee(employee);
        }

        [HttpPost]
        public ActionResult ViewEmployee(Employee employee)
        {
            return View("ViewEmployee", employee);
        }

        [HttpPost]
        public ActionResult NextStep(Employee employee, string ActionButton)
        {
            switch (ActionButton)
            {
                case "Add Dependent":
                    return AddDependent(employee);
                case "Generate Estimate":
                    return GenerateEstimate(employee);
                default:
                    return View("Index");
            }
        }
        

        [HttpPost]
        public ActionResult ViewEstimate(Employee employee)
        {
            return View(employee);
        }

        [HttpPost]
        public ActionResult AddDependent(Employee employee)
        {
            return View("AddDependent", employee);
        }

        [HttpPost]
        public ActionResult CreateDependent(Person newDependent, Employee employee)
        {
            employee.Dependents.Add(newDependent);
            return ViewEmployee(employee);
        }

        public ActionResult GenerateEstimate(Employee employee)
        {
            var task = SubmitGenerateEstimate(employee);
            Task.WaitAll(task);
            return View("ViewEstimate", task.Result);
        }

        public async Task<BenefitCostSummary> SubmitGenerateEstimate(Employee employee)
        {
            const string URL = @"http://localhost:51431/BenefitCalculator/GenerateEstimate/";

            var json = JsonConvert.SerializeObject(employee);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                var postData = new StringContent(json);
                postData.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                postData.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("charset", "utf-8"));
                postData.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("IEEE754Compatible", "true"));

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(URL, postData).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<BenefitCostSummary>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

    }
}