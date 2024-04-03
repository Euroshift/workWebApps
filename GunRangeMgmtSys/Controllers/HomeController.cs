using GunRangeMgmtSys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Formats.Asn1;
namespace GunRangeMgmtSys.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _csvFilePath = "shooterData.csv"; 

        public IActionResult Index(string searchQuery)
        {
            var shooters = LoadShootersFromCSV();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                shooters = shooters.Where(s => s.Name.Contains(searchQuery) || s.OfficerId.ToString().Contains(searchQuery)).ToList();
            }

            return View(shooters);
        }

        [HttpPost]
        public IActionResult AddShooter(Shooter shooter)
        {
            List<Shooter> shooters = LoadShootersFromCSV();
            shooters.Add(shooter);
            SaveShootersToCSV(shooters);
            return RedirectToAction("Index");
        }

        public IActionResult Inventory ()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddInventory(string itemName)
        {

            return RedirectToAction("Inventory");
        }
        public IActionResult ClearDatabase()
        {
            if (System.IO.File.Exists(_csvFilePath))
            {
                System.IO.File.Delete(_csvFilePath);
            }
            return RedirectToAction("Index");
        }

        private List<Shooter> LoadShootersFromCSV()
        {
            List<Shooter> shooters = new List<Shooter>();
            if (System.IO.File.Exists(_csvFilePath))
            {
                using (var reader = new StreamReader(_csvFilePath))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    shooters = csv.GetRecords<Shooter>().ToList();
                }
            }
            return shooters;
        }

        private void SaveShootersToCSV(List<Shooter> shooters)
        {
            using (var writer = new StreamWriter(_csvFilePath, append: false))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(shooters);
            }
        }

        public IActionResult DownloadCSV()
        {
            byte[] fileContents = System.IO.File.ReadAllBytes(_csvFilePath);
            string fileName = "shooterData.csv";
            return File(fileContents, "text/csv", fileName);
        }
    }
}
