using NetCorePal.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNetCorePalServiceDiscoveryClient();
builder.Services.AddK8SServiceDiscovery(p =>
{
    p.LabelKeyOfServiceName = "app.kubernetes.io/name";
});
//builder.Services.AddEurekaServiceDiscovery(p =>
//{
//    p.AppName = "the app";
//    p.ServerUrl = "http://localhost:8888/eureka/v2";
//});
var app = builder.Build();
var p = app.Services.GetService<NetCorePal.ServiceDiscovery.IServiceDiscoveryClient>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
