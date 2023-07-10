using GSSAssessment.Common.Database.JsonTestDb;
using GSSAssessment.Common.Database;
using GSSAssessment.Common.DataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSSAssessment.Common.Models;
using GSSAssessment.Common.Models.Transactions;

namespace GSSAssessment.Tests.SyncTests
{
    public class SyncTest
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseContextFactory.Init(typeof(JsonTestDbContext));
            DatabaseContextFactory.GetDatabaseContext().InitDb();
        }

        [Test]
        public void T010_TestBinSync()
        {
            var binSync = new BinSync();
            binSync.Sync();

            using(var context =  DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Bin>();

                Assert.That(bins.Count(), Is.EqualTo(3));

                bins = bins.OrderBy(x => x.Id).ToList();

                var bin100 = bins.FirstOrDefault();
                var bin200 = bins.Skip(1).FirstOrDefault();
                var bin300 = bins.Skip(2).FirstOrDefault();

                Assert.That(bin100.Id, Is.EqualTo(100));
                Assert.That(bin100.Description, Is.EqualTo("Bin100"));

                Assert.That(bin200.Id, Is.EqualTo(200));
                Assert.That(bin200.Description, Is.EqualTo("Bin200"));

                Assert.That(bin300.Id, Is.EqualTo(300));
                Assert.That(bin300.Description, Is.EqualTo("Bin300"));
            }
        }

        [Test]
        public void T020_TestItemSync()
        {
            var itemSync = new ItemSync();
            itemSync.Sync();

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var items = context.LoadModels<Item>();

                Assert.That(items.Count(), Is.EqualTo(3));

                items = items.OrderBy(x => x.Id).ToList();

                var item100 = items.FirstOrDefault();
                var item200 = items.Skip(1).FirstOrDefault();
                var item300 = items.Skip(2).FirstOrDefault();

                Assert.That(item100.Id, Is.EqualTo(100));
                Assert.That(item100.Description, Is.EqualTo("Item100"));

                Assert.That(item200.Id, Is.EqualTo(200));
                Assert.That(item200.Description, Is.EqualTo("Item200"));

                Assert.That(item300.Id, Is.EqualTo(300));
                Assert.That(item300.Description, Is.EqualTo("Item300"));
            }
        }

        [Test]
        public void T030_TestItemTransactions()
        {
            var itemRemovalTrans = new ItemRemovalTransaction()
            {
                Id = 1,
                ItemId = 2,
            };
            var itemUpdateTrans = new ItemUpdateTransaction()
            {
                Id = 1,
                ItemId = 2,
                Description = "ItemUpdateTrans"
            };

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                context.AddOrUpdateModel(itemRemovalTrans);
                context.AddOrUpdateModel(itemUpdateTrans);
            }

            var itemSync = new ItemSync();

            itemSync.ItemRemoval(itemRemovalTrans);
            using(var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var removals = context.LoadModels<ItemRemovalTransaction>();
                Assert.IsEmpty(removals);
            }

            itemSync.ItemUpdate(itemUpdateTrans);
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var updates = context.LoadModels<ItemUpdateTransaction>();
                Assert.IsEmpty(updates);
            }

        }
    }
}
