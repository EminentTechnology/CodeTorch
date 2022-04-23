using CodeTorch.Abstractions;
using CodeTorch.Configuration.FileStore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options => {
    options.Conventions.AddPageRoute("/_codetorchpage", "{*url}");
});

builder.Services.AddSingleton<IConfigurationStore, FileConfigurationStore>();


var app = builder.Build();

CodeTorch.Core.Configuration.GetInstance().ConfigurationPath = @"C:\Sandbox\CodeTorch2\samples\CodeTorch.Samples.Web.Config";
var store = app.Services.GetService<IConfigurationStore>();
CodeTorch.Core.ConfigurationLoader.Store = store;

await CodeTorch.Core.ConfigurationLoader.LoadConfiguration();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
