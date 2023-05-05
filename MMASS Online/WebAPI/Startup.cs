using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Helpers;
using WebApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }
        public static IConfiguration StaticConfig { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddControllers().AddNewtonsoftJson();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            // Agregar aqui todos los servicios que se vayan a utilizar
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContactoService, ContactoService>();
            services.AddScoped<IMedioService, MedioService>();
            services.AddScoped<ITarifaService, TarifaService>();
            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<ITipos_avisosService, Tipos_avisosService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IDg_orden_pub_apService, Dg_orden_pub_apService>();
            services.AddScoped<IForma_PagoService, Forma_PagoService>();
            services.AddScoped<IGoogleAdManagerService, GoogleAdManagerService>();
            services.AddScoped<IR_VentasService, R_VentasService>();
            services.AddScoped<IArchivosService, ArchivosService>();
            services.AddScoped<IDg_medios_asociadosService, Dg_medios_asociadosService>();
            services.AddScoped<IProgramaService, ProgramaService>();

            PerformCorsSetup(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            
            app.UseMvc();
        }

        private static void PerformCorsSetup(IServiceCollection services)
        {
            // ********************
            // Setup CORS
            // ********************
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
                                          //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            //corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {

                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });
        }
    }
}
