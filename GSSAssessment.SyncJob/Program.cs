// See https://aka.ms/new-console-template for more information

using GSSAssessment.Common.Database;
using GSSAssessment.Common.Database.JsonTestDb;
using GSSAssessment.SyncJob;

Console.WriteLine("Starting sync");

DatabaseContextFactory.Init(typeof(JsonTestDbContext));

var syncManager = new SyncManager();

Console.WriteLine("Sending transactions");
syncManager.SendTransactions();

Console.WriteLine("Syncing data");
syncManager.Sync();

Console.WriteLine("Sync finished");