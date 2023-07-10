using GSSAssessment.Common.Database;
using GSSAssessment.Common.Database.JsonTestDb;
using GSSAssessment.Common.Models;

namespace GSSAssessment.Tests
{
    public class DBContextTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void T010_RegisterDbContextTest()
        {
            DatabaseContextFactory.Init(typeof(JsonTestDbContext));

            var context = DatabaseContextFactory.GetDatabaseContext();

            Assert.That(context is JsonTestDbContext, Is.True);
            Assert.That(context is IDatabaseContext, Is.True);
            
            Assert.Throws<Exception>(TestInvalidDBContextInit);
        }

        private void TestInvalidDBContextInit()
        {
            DatabaseContextFactory.Init(typeof(Bin));
        }

        [Test]
        public void T020_CreateDbTest()
        {
            var context = new JsonTestDbContext();

            context.InitDb();

            var bins = context.LoadModels<Bin>();
            var items = context.LoadModels<Item>();
            var itemQtys = context.LoadModels<ItemQuantity>();

            Assert.That(bins, Is.Not.Null);
            Assert.That(items, Is.Not.Null);
            Assert.That(itemQtys, Is.Not.Null);
        }

        [Test]
        public void T030_ModelReadWriteTests()
        {
            var context = new JsonTestDbContext();

            context.InitDb();

            // Test Insert
            var bin = new Bin()
            {
                Id = 1,
                Description = "Test bin"
            };

            context.AddOrUpdateModel(bin);

            var bins = context.LoadModels<Bin>();

            Assert.That(bins, Has.Count.EqualTo(1));
            Assert.That(bins.First().Id, Is.EqualTo(1));
            Assert.That(bins.First().Description, Is.EqualTo("Test bin"));

            // Test Update
            bin = new Bin()
            {
                Id = 1,
                Description = "Test bin2"
            };

            context.AddOrUpdateModel(bin);

            bins = context.LoadModels<Bin>();

            Assert.That(bins, Has.Count.EqualTo(1));
            Assert.That(bins.First().Id, Is.EqualTo(1));
            Assert.That(bins.First().Description, Is.EqualTo("Test bin2"));

            // Test Remove
            context.RemoveModel<Bin>(1);

            bins = context.LoadModels<Bin>();

            Assert.That(bins, Is.Empty);
        }

        [TearDown]
        public void Teardown()
        {
        }
    }
}