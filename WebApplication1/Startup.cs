using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // получаем строку подключения из файла конфигурации
            var connection = Configuration.GetConnectionString("DefaultConnection");

            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            services.AddDbContext<ApplicationDbContext>(options =>
                                                                options.UseSqlServer(connection));

            services.AddIdentity<SiteUser, IdentityRole>(options =>
                    {
                        options.User.RequireUniqueEmail = true; // у каждого юзера уникальная почта
                        options.Password.RequireDigit = false; // в пароле НЕ требуются обязательно наличие цифр
                        options.Password.RequiredLength = 6; // длина пароля
                        options.Password.RequireUppercase = false; // в пароле НЕ требуется обязательно заглавные буквы
                        options.Password.RequireLowercase = false; // в пароле НЕ требуется обязательно в нижнем регистре буквы
                        options.Password.RequireNonAlphanumeric = false; // в пароле НЕ требуется обязательно символы
                        options.Password.RequiredUniqueChars = 0; // количество обязательных символов в пароле
                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>() // где будут юзеры
                    .AddDefaultTokenProviders(); // просто надо

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,

                            // строка, представляющая издателя
                            ValidIssuer = AuthService.Issuer,

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,

                            // установка потребителя токена
                            ValidAudience = AuthService.Audience,

                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = AuthService.GetSymmetricSecurityKey(),

                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true
                        };
                    });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors(builder => builder
                                   .AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader()
                                   .AllowCredentials());
            app.UseAuthentication();
        }
    }
}