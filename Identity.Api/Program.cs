using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ServiceModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
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
    options.InvalidModelStateResponseFactory = context => { return CustomErrorResponse(context); };
});
builder.Services.AddControllers() //Disable CamelCase
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

var auidenceConfig = builder.Configuration.GetSection("Auidence");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        ClockSkew = TimeSpan.Zero
    }
);
builder.Services.AddControllers();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();