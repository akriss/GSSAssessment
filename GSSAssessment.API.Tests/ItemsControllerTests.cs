using GSSAssessment.Common.Database.JsonTestDb;
using GSSAssessment.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSSAssessment.Common.DataManagers;
using GSSAssessment.Common.Models;
using GSSAssessment.Controllers;

namespace GSSAssessment.API.Tests
{
    public class ItemsControllerTests
    {
        [SetUp]
        public void Setup()
        {
            DatabaseContextFactory.Init(typeof(JsonTestDbContext));
            DatabaseContextFactory.GetDatabaseContext().InitDb();
        }

        [Test]
        public void T010_TestGetItems()
        {
            var item1 = new Item()
            {
                Id = 1,
                Description = "Item1"
            };
            var item2 = new Item()
            {
                Id = 2,
                Description = "Item2"
            };
            ItemManager.AddOrUpdate(item1, true);
            ItemManager.AddOrUpdate(item2, true);

            var controller = new ItemsController();

            var getItemsResult = controller.GetItems().ToList();

            Assert.IsNotNull(getItemsResult);
            Assert.AreEqual(2, getItemsResult.Count);
        }

        [Test]
        public void T020_TestGetItem()
        {
            var item1 = new Item()
            {
                Id = 1,
                Description = "Item1"
            };
            var item2 = new Item()
            {
                Id = 2,
                Description = "Item2"
            };
            ItemManager.AddOrUpdate(item1, true);
            ItemManager.AddOrUpdate(item2, true);

            var controller = new ItemsController();

            var getItemResult = controller.GetItem(1).Value;

            Assert.IsNotNull(getItemResult);
            Assert.That(getItemResult.Id, Is.EqualTo(1));
            Assert.That(getItemResult.Description, Is.EqualTo("Item1"));
        }

        [Test]
        public void T030_TestGetItemAssignments()
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

            var controller = new ItemsController();

            var result = controller.GetItemAssignments(3).Value.ToList();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().ItemId, Is.EqualTo(3));
            Assert.That(result.First().BinId, Is.EqualTo(1));
            Assert.That(result.First().Quantity, Is.EqualTo(5));
        }

        [Test]
        public void T040_TestAssignItem()
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

            var controller = new ItemsController();

            controller.AssignItem(1, 3);

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var assignments = context.LoadModels<ItemQuantity>(x => x.ItemId == 3 && x.BinId == 1).ToList();

                Assert.That(assignments.Count, Is.EqualTo(1));
                Assert.That(assignments.First().ItemId, Is.EqualTo(3));
                Assert.That(assignments.First().BinId, Is.EqualTo(1));
                Assert.That(assignments.First().Quantity, Is.EqualTo(0));
            }
        }

        [Test]
        public void T050_TestRemoveItemFromBin()
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

            var controller = new ItemsController();
            controller.RemoveFromBin(bin1.Id, item3.Id);

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var assignments = context.LoadModels<ItemQuantity>(x => x.ItemId == 3 && x.BinId == 1).ToList();
                Assert.That(assignments.Count, Is.EqualTo(0));
            }
        }

        [Test]
        public void T060_TestUpdateItemQuantity()
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
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var assignments = context.LoadModels<ItemQuantity>(x => x.ItemId == 3 && x.BinId == 1).ToList();

                Assert.That(assignments.Count, Is.EqualTo(1));
                Assert.That(assignments.First().ItemId, Is.EqualTo(3));
                Assert.That(assignments.First().BinId, Is.EqualTo(1));
                Assert.That(assignments.First().Quantity, Is.EqualTo(0));
            }

            var controller = new ItemsController();
            controller.UpdateItemQuantity(bin1.Id, item3.Id, 5);

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var assignments = context.LoadModels<ItemQuantity>(x => x.ItemId == 3 && x.BinId == 1).ToList();

                Assert.That(assignments.Count, Is.EqualTo(1));
                Assert.That(assignments.First().ItemId, Is.EqualTo(3));
                Assert.That(assignments.First().BinId, Is.EqualTo(1));
                Assert.That(assignments.First().Quantity, Is.EqualTo(5));
            }
        }
    }
}
