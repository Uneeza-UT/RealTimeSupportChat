using RealTimeSupportChat.Api.Middleware;
using RealTimeSupportChat.Identity;
using RealTimeSupportChat.Infrastructure;
using RealTimeSupportChat.Application;
using RealTimeSupportChat.Persistence;
using RealTimeSupportChat.Infrastructure.Hubs;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddSignalR();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddHttpContextAccessor();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
    });


    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });

});



var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectJavascript("/swagger/signalr.min.js");
    options.InjectJavascript("/swagger/SignalRTest.js");

    options.IndexStream = () =>
    typeof(Program).Assembly
        .GetManifestResourceStream(
            "RealTimeSupportChat.Api.Swagger.index.html");

    options.ConfigObject.PersistAuthorization = true;


    options.UseResponseInterceptor("""
        (response) => {
        console.log(response);
        console.log("Intercetor Executed", response);
            if (
                response.url?.includes("/api/Auth/login") &&
                response.status >= 200 &&
                response.status < 300
            ) {
                try {
                    const body = JSON.parse(response.text);
                    const token = body?.token;

                    if (token) {
                        window.ui.preauthorizeApiKey("Bearer", token);
                    }
                } catch (error) {
                    console.error("Could not extract login token:", error);
                }
            }

            return response;
        }
        """);

});
app.MapOpenApi();


app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SupportChatHub>("/hubs/support-chat");

app.Run();
