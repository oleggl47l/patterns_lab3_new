using ChatApp.Core.Interfaces;
using ChatApp.Core.Services;
using ChatApp.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options => {
    var connection = builder.Configuration.GetConnectionString("Redis");
    options.Configuration = connection;
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();
builder.Services.AddTransient<IDataSource, DataSource>();
builder.Services.AddTransient<DataProxy>();

// builder.Services.AddSingleton<IDataSource>(provider =>
//     new DataProxy(provider.GetRequiredService<IDataSource>())
// );

var app = builder.Build();


app.MapHub<ChatHub>("/chat");

app.UseCors();

app.Run();