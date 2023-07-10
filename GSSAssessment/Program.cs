using GSSAssessment.Common.Database;
using GSSAssessment.Common.Database.JsonTestDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (builder.Configuration["DBConnectionType"] == "Test")
{
    DatabaseContextFactory.Init(typeof(JsonTestDbContext));
    using (JsonTestDbContext context = DatabaseContextFactory.GetDatabaseContext() as JsonTestDbContext)
        context.InitIfNotExists();
}
else
    DatabaseContextFactory.Init(typeof(DatabaseContext));

app.Run();

