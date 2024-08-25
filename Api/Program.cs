using Api;
using Api.Endpoints;
using Application;
using Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.AddUserEndpoints();
app.AddModelEndpoints();
app.AddLibraryEndpoints();

app.Run();


