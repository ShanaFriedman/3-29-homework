using March_29_homework.Data;
using March_29_homework.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace March_29_homework.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund; Integrated Security=true;";

        public IActionResult Index()
        {
            SimchaFundManager manager = new(_connectionString);
            HomeViewModel model = new HomeViewModel()
            {
                Simchas = manager.GetSimchas(),
                TotalContributors = manager.GetTotalContributor()
            };
            if (TempData["message"] != null)
            {
                model.Message = (string)TempData["message"];
            }
            return View(model);
        }

    }
}