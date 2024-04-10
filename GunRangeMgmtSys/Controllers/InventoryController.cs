using GunRangeMgmtSys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Globalization;
using CsvHelper.Configuration;

namespace GunRangeMgmtSys.Controllers
{
    public class InventoryController : Controller
    {
        private readonly string _csvFilePath = "inventoryData.csv";

        public IActionResult Inventory(string searchQuery)
        {
            var invitems = LoadInvitemsFromCSV();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                invitems = invitems.Where(s => s.OfficerId.ToString().Contains(searchQuery)).ToList();
            }

            return View("~/Views/Home/Inventory.cshtml", invitems);
        }

        [HttpPost]
        public IActionResult AddInvitem(Invitem invitem)
        {
            List<Invitem> invitems = LoadInvitemsFromCSV();
            invitems.Add(invitem);
            SaveInvitemsToCSV(invitems);
            return RedirectToAction("Inventory");
        }


        [HttpPost]
        public IActionResult ClearDatabase()
        {
            if (System.IO.File.Exists(_csvFilePath))
            {
                System.IO.File.Delete(_csvFilePath);
            }
            return RedirectToAction("Inventory");
        }

        private List<Invitem> LoadInvitemsFromCSV()
        {
            List<Invitem> invitems = new List<Invitem>();
            if (System.IO.File.Exists(_csvFilePath))
            {
                //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                //{
                //    // Ignore header validation
                //    HeaderValidated = null,
                //    MissingFieldFound = null
                //};

                using (var reader = new StreamReader(_csvFilePath))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    invitems = csv.GetRecords<Invitem>().ToList();
                }
            }
            return invitems;
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
        private void SaveInvitemsToCSV(List<Invitem> invitems)
        {
            using (var writer = new StreamWriter(_csvFilePath, append: false))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(invitems);
            }
        }

        public IActionResult DownloadCSV()
        {
            byte[] fileContents = System.IO.File.ReadAllBytes(_csvFilePath);
            string fileName = "inventoryData.csv";
            return File(fileContents, "text/csv", fileName);
        }
    }
}
