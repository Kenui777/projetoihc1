using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext com a string de conexão do banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionando suporte ao log de eventos no console
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();  // Remove os provedores de log padrão
    logging.AddConsole();  // Adiciona o log no console
});

// Adicionando os serviços MVC (Controllers com Views)
builder.Services.AddControllersWithViews();

// Configuração de CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();  // Permite qualquer origem, método e cabeçalho
    });
});

// Configuração de Anti-forgery (Proteção contra CSRF)
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configuração de Cookies para Autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configuração do Kestrel (Servidor Web) para suportar HTTPS em uma porta específica
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7030, listenOptions =>
    {
        listenOptions.UseHttps();  // Configura o servidor para escutar na porta 7030 com HTTPS
    });
});

var app = builder.Build();

// Configurações para ambientes diferentes
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // Página de erro customizada
    app.UseHsts();  // HSTS (HTTP Strict Transport Security) para segurança
}

app.UseHttpsRedirection();  // Redireciona todas as requisições HTTP para HTTPS
app.UseStaticFiles();  // Serve arquivos estáticos (CSS, JS, imagens)
app.UseRouting();  // Habilita o roteamento
app.UseCors();  // Habilita as políticas de CORS definidas
app.UseAuthentication();  // Habilita a autenticação
app.UseAuthorization();  // Habilita a autorização

// Configuração da rota padrão para Controllers/Views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Inicia a aplicação
