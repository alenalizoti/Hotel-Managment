using HotelDomaci.Data;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<ApartmanService>();

var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDbSettings>();
var client = new MongoClient(settings.ConnectionString);

try
{
    //pokusavamo da pristupimo bazi
    var database = client.GetDatabase(settings.DatabaseName);
    var list = database.ListCollectionNames().ToList(); // samo da vidimo da vrati nesto

    Console.WriteLine("Konekcija ka MongoDB uspostavljena!");
    Console.WriteLine($"Kolekcije u bazi: {string.Join(", ", list)}");
}
catch (Exception ex)
{
    Console.WriteLine("❌ Neuspešna konekcija na MongoDB: " + ex.Message);
}




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();