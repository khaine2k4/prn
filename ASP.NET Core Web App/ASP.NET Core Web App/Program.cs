using SignalRChat.Hubs;


var builder = WebApplication.CreateBuilder(args);


// Add services

builder.Services.AddRazorPages();

builder.Services.AddSignalR();


var app = builder.Build();


// Middleware

if (!app.Environment.IsDevelopment())

{

    app.UseExceptionHandler("/Error");

}


app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();

app.MapHub<ChatHub>("/chatHub");


app.Run();