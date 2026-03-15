using Microsoft.EntityFrameworkCore;
using CareBusiness;
using CareBusiness.Entities;
using CareDataAccess;
using CareRepositories;
using CareRepositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HealthcareContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthcareDB")));

// Register DAOs
builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<DoctorDAO>();
builder.Services.AddScoped<AppointmentDAO>();
builder.Services.AddScoped<SessionDAO>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Data Seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HealthcareContext>();
    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        // Add Admin
        context.Users.Add(new User
        {
            FullName = "Administrator",
            Email = "admin@healthcare.com",
            Password = HashString("admin123"),
            Role = "Admin",
            CreatedAt = DateTime.Now
        });

        // Add dummy doctors for testing search
        context.Doctors.AddRange(new List<Doctor>
        {
            new Doctor { DoctorName = "Dr. John Doe", Specialty = "Cardiology", LicenseNumber = "L123456", MaxPatients = 10, Active = true },
            new Doctor { DoctorName = "Dr. Jane Smith", Specialty = "General", LicenseNumber = "L789012", MaxPatients = 8, Active = true },
            new Doctor { DoctorName = "Dr. Robert Wilson", Specialty = "Cardiology", LicenseNumber = "L345678", MaxPatients = 15, Active = true }
        });

        context.SaveChanges();
    }
}

string HashString(string input)
{
    using var sha256 = System.Security.Cryptography.SHA256.Create();
    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
    return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
}

app.Run();
