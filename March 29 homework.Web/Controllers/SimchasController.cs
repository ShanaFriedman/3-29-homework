using March_29_homework.Data;
using March_29_homework.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace March_29_homework.Web.Controllers
{
    public class SimchasController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund; Integrated Security=true;";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contributions(int simchaId)
        {
            SimchaFundManager manager = new(_connectionString);
            ContributoinsViewModel vm = new()
            {
                Contributors = manager.GetContributors(),
                Simcha = manager.GetSimchaById(simchaId),
                ContIdAmount = manager.GetContributionsForSimcha(simchaId)
            };
           
            return View(vm);
        }

        public IActionResult New(Simcha s)
        {
            SimchaFundManager manager = new(_connectionString);
            int id = manager.AddSimcha(s);
            manager.ContributeForAlwaysInclude(manager.GetContributors(), id);
            TempData["message"] = $"New simcha created!";
            return Redirect("/");

        }
        [HttpPost]
        public IActionResult UpdateContributions(int simchaId, List<Contribution> contributors)
        {
            SimchaFundManager manager = new(_connectionString);
            manager.UpdateContribution(simchaId, contributors);
            TempData["message"] = $"Simcah successfully updated";
            return Redirect("/");

        }
    }
}
