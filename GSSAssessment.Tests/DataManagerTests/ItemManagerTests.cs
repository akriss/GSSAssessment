using GSSAssessment.Common.Database.JsonTestDb;
using GSSAssessment.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSSAssessment.Common.DataManagers;
using GSSAssessment.Common.Models;
using GSSAssessment.Common.Models.Transactions;

namespace GSSAssessment.Tests.DataManagerTests
{
    public class ItemManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseContextFactory.Init(typeof(JsonTestDbContext));
            DatabaseContextFactory.GetDatabaseContext().InitDb();
        }

        [Test]
        public void T010_AddUpdateRemoveTestFromExternal()
        {
            var item = new Item()
            {
                Id = 10,
                Description = "test"
            };

            // Test addition of new Item
            var result = ItemManager.AddOrUpdate(item, true);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var items = context.LoadModels<Item>(x => x.Id == 10);

                Assert.That(items.Count == 1, Is.True);
            }

            // Test update of Item
            item.Description = "test2";
            result = ItemManager.AddOrUpdate(item, true);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Item>(x => x.Id == 10);

                Assert.That(bins.Count == 1, Is.True);

                Assert.That(bins.First().Description, Is.EqualTo("test2"));
            }

            // Test removal
            result = ItemManager.RemoveItem(item, true);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Item>(x => x.Id == 10);

                Assert.That(bins.Count == 0, Is.True);
            }
        }

        [Test]
        public void T020_AddUpdateRemoveTestNotExternal()
        {
            var item = new Item()
            {
                Id = 20,
                Description = "test"
            };

            // Test addition of new Item
            var result = ItemManager.AddOrUpdate(item, false);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var items = context.LoadModels<Item>(x => x.Id == 20);

                Assert.That(items.Count == 1, Is.True);

                var transaction = context.LoadModels<ItemUpdateTransaction>(x => x.ItemId == 20).FirstOrDefault();
                Assert.NotNull(transaction);
                Assert.That(transaction.Description, Is.EqualTo("test"));
            }

            // Test update of Item
            item = new Item()
            {
                Id = 30,
                Description = "test2"
            };
            result = ItemManager.AddOrUpdate(item, false);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Item>(x => x.Id == 30);

                Assert.That(bins.Count == 1, Is.True);

                Assert.That(bins.First().Description, Is.EqualTo("test2"));

                var transaction = context.LoadModels<ItemUpdateTransaction>(x => x.ItemId == 30).FirstOrDefault();
                Assert.NotNull(transaction);
                Assert.That(transaction.Description, Is.EqualTo("test2"));
            }

            // Test removal
            result = ItemManager.RemoveItem(item, false);
            Assert.That(result, Is.EqualTo(string.Empty));

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var items = context.LoadModels<Item>(x => x.Id == 30);

                Assert.That(items.Count == 0, Is.True);

                var transaction = context.LoadModels<ItemRemovalTransaction>(x => x.ItemId == 30).FirstOrDefault();
                Assert.NotNull(transaction);
            }
        }
    }
}
