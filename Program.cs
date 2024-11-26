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

var app = builder.Build();

// Configura��o do pipeline de requisi��es
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Permitir CORS
app.UseCors();

// Ativar autentica��o, caso necess�rio
app.UseAuthorization();

// Servir arquivos est�ticos (CSS, JS, imagens)
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
