using Api;
using Api.Endpoints;
using Application;
using Infrastructure;
using Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddDomainServices();

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
app.AddItemEndpoints();
app.AddWorkflowInstancesEndpoints();

app.Run();


