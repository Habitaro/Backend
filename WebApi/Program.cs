using Hangfire;
using Newtonsoft.Json.Serialization;
using WebApi.Startup;
using WebApi.Startup.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.RegisterServices(builder.Configuration);

builder.Services.RegisterAuthorization(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(opt => 
    {
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

builder.Services.RegisterSwagger();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
