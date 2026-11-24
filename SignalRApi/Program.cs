using FluentValidation;
using Scalar.AspNetCore;
using SignalR.BusinessLayer.Extensions;
using SignalR.BusinessLayer.ValidationRules.BookingValidations;
using SignalR.DataAccessLayer.Concrete;
using SignalR.EntityLayer.Entities;
using SignalRApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceRegistration();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", crs =>
    {
        crs.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials();
    });
});

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<SignalrContext>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingValidator>();

builder.Services.AddControllers(o => o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("CorsPolicy");
app.MapHub<SignalRHub>("/signalrhub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
