using GSSAssessment.Common.Database;
using GSSAssessment.Common.Database.JsonTestDb;
using GSSAssessment.Common.DataManagers;
using GSSAssessment.Common.Models;
using GSSAssessment.Controllers;

namespace GSSAssessment.API.Tests
{
    public class BinControllerTests
    {
        [SetUp]
        public void Setup()
        {
            DatabaseContextFactory.Init(typeof(JsonTestDbContext));
            DatabaseContextFactory.GetDatabaseContext().InitDb();
        }

        [Test]
        public void T010_TestGetBins()
        {
            var bin1 = new Bin()
            {
                Id = 1,
                Description = "Bin1"
            };
            var bin2 = new Bin()
            {
                Id = 2,
                Description = "Bin2"
            };
            BinManager.AddOrUpdate(bin1);
            BinManager.AddOrUpdate(bin2);

            var controller = new BinController();

            var getBinsResult = controller.GetBins().ToList();

            Assert.IsNotNull(getBinsResult);
            Assert.AreEqual(2, getBinsResult.Count);
        }

        [Test]
        public void T020_TestGetBin()
        {
            var bin1 = new Bin()
            {
                Id = 1,
                Description = "Bin1"
            };
            var bin2 = new Bin()
            {
                Id = 2,
                Description = "Bin2"
            };
            BinManager.AddOrUpdate(bin1);
            BinManager.AddOrUpdate(bin2);

            var controller = new BinController();

            var getBinResult = controller.GetBin(1).Value;

            Assert.IsNotNull(getBinResult);
            Assert.That(getBinResult.Id, Is.EqualTo(1));
            Assert.That(getBinResult.Description, Is.EqualTo("Bin1"));
        }

        [Test]
        public void T030_TestGetItemsInBin()
        {
            var bin1 = new Bin()
            {
                Id = 1,
                Description = "Bin1"
            };
            var bin2 = new Bin()
            {
                Id = 2,
                Description = "Bin2"
            };
            BinManager.AddOrUpdate(bin1);
            BinManager.AddOrUpdate(bin2);

            var item3 = new Item()
            {
                Id = 3,
                Description = "Item3"
            };
            var item4 = new Item()
            {
                Id = 4,
                Description = "Item4"
            };
            ItemManager.AddOrUpdate(item3, true);
            ItemManager.AddOrUpdate(item4, true);

            ItemAssignmentManager.AssignItemToBin(bin1, item3);
            ItemAssignmentManager.UpdateItemQtyInBin(bin1, item3, 5);

            var controller = new BinController();

            var result = controller.GetItemsInBin(1).Value.ToList();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(3));
        }

        [Test]
        public void T040_TestGetItemQtysInBin()
        {
            var bin1 = new Bin()
            {
                Id = 1,
                Description = "Bin1"
            };
            var bin2 = new Bin()
            {
                Id = 2,
                Description = "Bin2"
            };
            BinManager.AddOrUpdate(bin1);
            BinManager.AddOrUpdate(bin2);

            var item3 = new Item()
            {
                Id = 3,
                Description = "Item3"
            };
            var item4 = new Item()
            {
                Id = 4,
                Description = "Item4"
            };
            ItemManager.AddOrUpdate(item3, true);
            ItemManager.AddOrUpdate(item4, true);

            ItemAssignmentManager.AssignItemToBin(bin1, item3);
            ItemAssignmentManager.UpdateItemQtyInBin(bin1, item3, 5);

            var controller = new BinController();

            var result = controller.GetItemQuantitiesInBin(1).Value.ToList();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Quantity, Is.EqualTo(5));
            Assert.That(result.First().ItemId, Is.EqualTo(3));
            Assert.That(result.First().BinId, Is.EqualTo(1));
        }
    }
}