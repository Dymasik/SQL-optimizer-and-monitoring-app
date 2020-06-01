using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MssqlDatabaseAdminPanel.Models;
using SqlAnalyzer;

namespace MssqlDatabaseAdminPanel.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult OptimizeSelect(string select) {
            if (string.IsNullOrWhiteSpace(select)) {
                return View("Index");
            }
            var optimizer = new SelectOptimizer();
            var resultSelect = optimizer.GetOptimizedQuery(select);
            return View("Index", resultSelect);
        }

        public IActionResult Login() {
            return View();
        }

        public IActionResult Connect(int type, string serverName, string dbName, string userName, string password) {
            DbConnection.SetConnectionString(type, serverName, dbName, userName, password);
            var result = DbConnection.EstablishConnection();
            if (!result)
                return Redirect("/Home/Error");
            return Redirect("/Home/AdminPanel");
        }

        public IActionResult AdminPanel() {
            var result = DbConnection.EstablishConnection();
            if (!result)
                return Redirect("/Home/Login");
            return View();
        }

        public IActionResult RequestDb() {
            ViewBag.IsRequestView = true;
            ViewBag.Requests = DbAnalyzer.GetMostExpensiveRequest();
            ViewBag.ReadableTables = DbAnalyzer.GetReadableTables();
            ViewBag.WritableTables = DbAnalyzer.GetWritableTables();
            return View("AdminPanel");
        }

        public IActionResult IndexDb() {
            ViewBag.IsIndexView = true;
            ViewBag.RequiredIndexes = DbAnalyzer.GetRequiredIndexes();
            ViewBag.UnusedIndexes = DbAnalyzer.GetUnusedIndexes();
            ViewBag.MostExpensiveIndexes = DbAnalyzer.GetMostExpensiveIndexes();
            ViewBag.AlterIndexText = DbAnalyzer.GetAlterIndexesSql();
            return View("AdminPanel");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
