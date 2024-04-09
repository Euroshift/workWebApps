using GunRangeMgmtSys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Formats.Asn1;
namespace GunRangeMgmtSys.Controllers
{
    public class RangecomplianceController : Controller
    {
        private readonly string _csvFilePath = "shooterData.csv";

        //public IActionResult Index(string searchQuery)
        //{
        //    var shooters = LoadShootersFromCSV();

        //    if (!string.IsNullOrEmpty(searchQuery))
        //    {
        //        shooters = shooters.Where(s => s.Name.Contains(searchQuery) || s.OfficerId.ToString().Contains(searchQuery)).ToList();
        //    }

        //    return View(shooters);
        //}

        [HttpPost]
        private List<Rangecomp> LoadShootersFromCSV()
        {
            List<Rangecomp> shooters = new List<Rangecomp>();
            if (System.IO.File.Exists(_csvFilePath))
            {
                using (var reader = new StreamReader(_csvFilePath))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    shooters = csv.GetRecords<Rangecomp>().ToList();
                }
            }
            return shooters;
        }
        public IActionResult Inventory()
        {
            return View();
        }
        public IActionResult Rangecompliance()
        {
            return View();
        }


        public IActionResult DownloadCSV()
        {
            byte[] fileContents = System.IO.File.ReadAllBytes(_csvFilePath);
            string fileName = "shooterData.csv";
            return File(fileContents, "text/csv", fileName);
        }

    }
}
