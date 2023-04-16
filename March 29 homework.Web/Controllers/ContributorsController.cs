using March_29_homework.Data;
using March_29_homework.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace March_29_homework.Web.Controllers
{
    public class ContributorsController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund; Integrated Security=true;";

        public IActionResult Index()
        {
            SimchaFundManager manager = new(_connectionString);
            ContributorsViewModel model = new()
            {
                Contributors = manager.GetContributors(),
                Total = manager.GetBalance(-1)

            };

            if (TempData["message"] != null)
            {
                model.Message = (string)TempData["message"];
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult New(Contributor c, int initialDeposit, DateTime dateCreated)
        {
            SimchaFundManager manager = new(_connectionString);
           
            int id = manager.AddContributor(c);
            manager.AddDeposite(id, initialDeposit, dateCreated);
            if (c.AlwaysInclude)
            {
                manager.ContributeToAllUpcomingSimchas(manager.GetSimchas(), id);
            }

            TempData["message"] = $"Person has been successfully added";

            return Redirect("/contributors");
        }

        [HttpPost]
        public IActionResult Edit(Contributor c)
        {
            SimchaFundManager manager = new(_connectionString);
            manager.UpdateContributor(c);
            TempData["message"] = $"Person has been successfully edited";
            return Redirect("/contributors");
        }

        [HttpPost]
        public IActionResult Deposit(int contributorId, int amount, DateTime date)
        {
            SimchaFundManager manager = new(_connectionString);
            manager.AddDeposite(contributorId, amount, date);
            TempData["message"] = $"Deposit successfully recorded";
            return Redirect("/contributors");
        }

        public IActionResult History(int contribId)
        {
            SimchaFundManager manager = new(_connectionString);
            List<Actions> contributions = manager.GetContributionsForContributor(contribId);
            List<Actions> deposits = manager.GetDepositsFoPerson(contribId);
            contributions.AddRange(deposits);

            HistoryViewModel vm = new()
            {
                Contributor = manager.GetContributor(contribId),
                Actions = contributions.OrderBy(a => a.Date).ToList()
            };
            return View(vm);
        }
    }
}
