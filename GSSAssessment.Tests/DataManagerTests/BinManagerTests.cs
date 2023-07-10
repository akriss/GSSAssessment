using GSSAssessment.Common.Database.JsonTestDb;
using GSSAssessment.Common.Database;
using GSSAssessment.Common.DataManagers;
using GSSAssessment.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Tests.DataManagerTests
{
    public class BinManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseContextFactory.Init(typeof(JsonTestDbContext));
            DatabaseContextFactory.GetDatabaseContext().InitDb();
        }

        [Test]
        public void T010_AddUpdateRemoveTest()
        {
            var bin = new Bin()
            {
                Id = 10,
                Description = "test"
            };

            // Test addition of new Bin
            var result = BinManager.AddOrUpdate(bin);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Bin>(x => x.Id == 10);

                Assert.That(bins.Count == 1, Is.True);
            }

            // Test update of Bin
            bin.Description = "test2";
            result = BinManager.AddOrUpdate(bin);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Bin>(x => x.Id == 10);

                Assert.That(bins.Count == 1, Is.True);

                Assert.That(bins.First().Description, Is.EqualTo("test2"));
            }

            // Test removal
            result = BinManager.RemoveBin(bin);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Bin>(x => x.Id == 10);

                Assert.That(bins.Count == 0, Is.True);
            }
        }

        [Test]
        public void T020_TestAssignmentAndQuantityChanges()
        {
            var item = new Item
            {
                Id = 10,
                Description = "item desc"
            };

            var bin = new Bin
            {
                Id = 20,
                Description = "bin desc"
            };

            string result = BinManager.AddOrUpdate(bin);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                context.AddOrUpdateModel(item);
            }

            // Test bin assignment
            result = ItemAssignmentManager.AssignItemToBin(bin, item);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var qty = context.LoadModels<ItemQuantity>(x => x.ItemId == item.Id && x.BinId == bin.Id).FirstOrDefault();

                Assert.That(qty, Is.Not.Null);
                Assert.That(qty.Quantity, Is.EqualTo(0));
                Assert.That(qty.BinId, Is.EqualTo(bin.Id));
                Assert.That(qty.ItemId, Is.EqualTo(item.Id));
            }

            // Test quantity update
            result = ItemAssignmentManager.UpdateItemQtyInBin(bin, item, 2);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var qty = context.LoadModels<ItemQuantity>(x => x.ItemId == item.Id && x.BinId == bin.Id).FirstOrDefault();

                Assert.That(qty, Is.Not.Null);
                Assert.That(qty.Quantity, Is.EqualTo(2));
            }

            // Test floor of 0
            result = ItemAssignmentManager.UpdateItemQtyInBin(bin, item, -1);
            Assert.That(result, Is.EqualTo("Total quantity must be non-negative"));

            // Test removal from bin
            result = ItemAssignmentManager.RemoveItemFromBin(bin, item);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var qty = context.LoadModels<ItemQuantity>(x => x.ItemId == item.Id && x.BinId == bin.Id).FirstOrDefault();

                Assert.That(qty, Is.Null);
            }
        }
    }
}
