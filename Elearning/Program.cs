var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add this line to read the Oracle DB connection string and configure your DbContext
var connectionString = builder.Configuration.GetConnectionString("OracleDbContext");
//builder.Services.AddDbContext<YourDbContext>(options => options.UseOracle(connectionString));


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
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "instructor",
    pattern: "{controller=Instructor}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "student",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "course",
    pattern: "{controller=Course}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "lesson",
    pattern: "{controller=Lesson}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "enrolment",
    pattern: "{controller=Enrolment}/{action=Index}/{id?}");


//app.MapControllerRoute(
//    name: "course",
//    pattern: "Course/{action=Index}/{id?}",
//    defaults: new { controller = "Course" });

app.Run();
