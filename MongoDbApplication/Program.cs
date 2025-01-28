using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using MongoDbApplication.Contracts;
using MongoDbApplication.DB;
using MongoDbApplication.Repositories;
using MongoDbApplication.Services;
//using 	Microsoft.AspNetCore.OpenApi
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}).AddXmlDataContractSerializerFormatters();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("InventoryOpenAPISpecification",

    new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "InventoryManager Api",
        Version = "v1"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);



});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}
app.UseSwagger();
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/swagger/InventoryOpenAPISpecification/swagger.json", "InventoryManager");
    option.RoutePrefix = "";
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
