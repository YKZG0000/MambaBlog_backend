using NeQ.blog.Model;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NeQ.blog.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //配置跨域
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        //允许所有来源/所有头/所有HTTP方法
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // 配置 Cookie 认证
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // 添加内存缓存服务
            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();

            //注册服务
            builder.Services.AddDbContext<BlogContext>(options =>
                   options.UseSqlServer(builder.Configuration
                   .GetConnectionString("BlogConnection")));

            // 配置EmailService
            builder.Services.AddSingleton(new EmailService(
                smtpServer: "smtp.qq.com",
                smtpPort: 465, // 你的SMTP端口
                smtpUser: "3282095848@qq.com",
                smtpPass: "thvtygswekyrdaaa"
            ));


            builder.Services.AddEndpointsApiExplorer();
            //swagger配置
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v0", new OpenApiInfo { Title = "NeQ.Api", Version = "0.1" });
                options.OrderActionsBy(o => o.RelativePath);
                var file = Path.Combine(AppContext.BaseDirectory, "NeQ.blog.api.xml");
                options.IncludeXmlComments(file, true);
            });

            var app = builder.Build();

            ////生成数据库和表结构。
            var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<BlogContext>().Database.EnsureCreated();


            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v0/swagger.json", "v0");
                });
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
