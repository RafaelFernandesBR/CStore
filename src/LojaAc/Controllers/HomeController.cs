using System.Diagnostics;
using LojaAc.Data.IModels;
using LojaAc.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaAc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataModel _dataModel;
        private readonly ILogsData _logs;

        public HomeController(ILogger<HomeController> logger,
            IDataModel dataModel,
            ILogsData logsData)
        {
            _logger = logger;
            _dataModel = dataModel;
            _logs = logsData;
        }

        public IActionResult Index()
        {
            var final = _dataModel.GetAleatorio();

            return View(final);
        }

        [Authorize]
        public IActionResult Cadastra()
        {
            return View();
        }

        public IActionResult DefBoard()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Cadastra");
            }

            return View();
        }

        [Authorize]
        public IActionResult CriaConta()
        {
            return View();
        }

        [Authorize]
        public IActionResult Logs()
        {
            var ObterLogs = _logs.GetLogs();

            ViewBag.Logs = ObterLogs;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
