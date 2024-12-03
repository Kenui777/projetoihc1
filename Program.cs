using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext com a string de conex�o do banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionando suporte ao log de eventos no console
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();  // Remove os provedores de log padr�o
    logging.AddConsole();  // Adiciona o log no console
});

// Adicionando os servi�os MVC (Controllers com Views)
builder.Services.AddControllersWithViews();

// Configura��o de CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();  // Permite qualquer origem, m�todo e cabe�alho
    });
});

// Configura��o de Anti-forgery (Prote��o contra CSRF)
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configura��o de Cookies para Autentica��o
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configura��o do Kestrel (Servidor Web) para suportar HTTPS em uma porta espec�fica
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7030, listenOptions =>
    {
        listenOptions.UseHttps();  // Configura o servidor para escutar na porta 7030 com HTTPS
    });
});

var app = builder.Build();

// Configura��es para ambientes diferentes
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // P�gina de erro customizada
    app.UseHsts();  // HSTS (HTTP Strict Transport Security) para seguran�a
}

app.UseHttpsRedirection();  // Redireciona todas as requisi��es HTTP para HTTPS
app.UseStaticFiles();  // Serve arquivos est�ticos (CSS, JS, imagens)
app.UseRouting();  // Habilita o roteamento
app.UseCors();  // Habilita as pol�ticas de CORS definidas
app.UseAuthentication();  // Habilita a autentica��o
app.UseAuthorization();  // Habilita a autoriza��o

// Configura��o da rota padr�o para Controllers/Views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Inicia a aplica��o
