using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionar os serviços de controladores e views
builder.Services.AddControllersWithViews();

// Configuração de CORS (permitir qualquer origem, método e cabeçalho)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Configuração de cookies de antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Garantir que o cookie só seja enviado via HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict;             // Prevenir ataques CSRF
});

// Configuração de cookies de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Garantir que o cookie de autenticação só seja enviado via HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict;             // Prevenir ataques CSRF
});

var app = builder.Build();

// Configuração do pipeline de requisições
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HSTS (HTTP Strict Transport Security) para forçar HTTPS
}

app.UseHttpsRedirection();  // Redireciona automaticamente para HTTPS
app.UseStaticFiles();
app.UseRouting();

app.UseCors();  // Habilitar CORS

app.UseAuthentication();  // Autenticação de usuários
app.UseAuthorization();   // Autorização de usuários

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
