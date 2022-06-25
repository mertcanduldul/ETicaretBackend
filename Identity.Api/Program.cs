using System;
using ServiceModel;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options
    .AddDefaultPolicy(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
    ));
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        return CustomErrorResponse(context);
    };
});

builder.Services.AddControllers() //Disable CamelCase
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

BadRequestObjectResult CustomErrorResponse(ActionContext context)
{
    return new BadRequestObjectResult(context.ModelState
        .Where(modelError => modelError.Value.Errors.Count > 0)
        .Select(modelError => new ServicesResponse
        {
            IsSuccess = false,
            Message = String.Format("{0} - {1}", modelError.Key, modelError.Value.Errors.ToList().ToString())
        }).FirstOrDefault());
}

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
