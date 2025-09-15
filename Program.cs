using pct.ecole.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
