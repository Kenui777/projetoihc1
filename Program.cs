using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionar os servi�os de controladores e views
builder.Services.AddControllersWithViews();

// Configura��o de CORS (permitir qualquer origem, m�todo e cabe�alho)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Configura��o de cookies de antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Garantir que o cookie s� seja enviado via HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict;             // Prevenir ataques CSRF
});

// Configura��o de cookies de autentica��o
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Garantir que o cookie de autentica��o s� seja enviado via HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict;             // Prevenir ataques CSRF
});

var app = builder.Build();

// Configura��o do pipeline de requisi��es
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HSTS (HTTP Strict Transport Security) para for�ar HTTPS
}

app.UseHttpsRedirection();  // Redireciona automaticamente para HTTPS
app.UseStaticFiles();
app.UseRouting();

app.UseCors();  // Habilitar CORS

app.UseAuthentication();  // Autentica��o de usu�rios
app.UseAuthorization();   // Autoriza��o de usu�rios

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
