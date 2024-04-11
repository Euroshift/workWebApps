using CsvHelper;
using GunRangeMgmtSys.Models;
using Microsoft.AspNetCore.Mvc;
namespace GunRangeMgmtSys.Controllers
{
    public class AwardsController : Controller
    {
        private readonly string _csvFilePath = "shooterData.csv";


        [HttpPost]
        private List<Award> LoadShootersFromCSV()
        {
            List<Award> shooters = new List<Award>();
            if (System.IO.File.Exists(_csvFilePath))
            {
                using (var reader = new StreamReader(_csvFilePath))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    shooters = csv.GetRecords<Award>().ToList();
                }
            }
            return shooters;
        }

        public IActionResult UpdateAwards(string officerId, string marksAwards)
        {
            var shooters = LoadShootersFromCSV();
            var shooterToUpdate = shooters.FirstOrDefault(s => s.OfficerId == officerId);
            if (shooterToUpdate != null)
            {
                shooterToUpdate.MarksAwards = marksAwards;
                // Save the updated shooters list back to the CSV file
                using (var writer = new StreamWriter(_csvFilePath))
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(shooters);
                }
            }
            // Redirect to the Awards view after the update
            return RedirectToAction("Awards");
        }
        public IActionResult Inventory()
        {
            return View();
        }
        public IActionResult Rangecompliance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Awards(string officerId, string marksAwards)
        {
            if (!string.IsNullOrEmpty(officerId))
            {
                // Call the UpdateAwards method to update the awards
                return UpdateAwards(officerId, marksAwards);
            }
            // Redirect back to the Awards view if officerId is not provided
            return RedirectToAction("Awards");
        }
        public IActionResult DownloadCSV()
        {
            byte[] fileContents = System.IO.File.ReadAllBytes(_csvFilePath);
            string fileName = "shooterData.csv";
            return File(fileContents, "text/csv", fileName);
        }

    }
}
