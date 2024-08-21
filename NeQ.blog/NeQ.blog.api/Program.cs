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

            //���ÿ���
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        //����������Դ/����ͷ/����HTTP����
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // ���� Cookie ��֤
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // ����ڴ滺�����
            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();

            //ע�����
            builder.Services.AddDbContext<BlogContext>(options =>
                   options.UseSqlServer(builder.Configuration
                   .GetConnectionString("BlogConnection")));

            // ����EmailService
            builder.Services.AddSingleton(new EmailService(
                smtpServer: "smtp.qq.com",
                smtpPort: 465, // ���SMTP�˿�
                smtpUser: "3282095848@qq.com",
                smtpPass: "thvtygswekyrdaaa"
            ));


            builder.Services.AddEndpointsApiExplorer();
            //swagger����
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v0", new OpenApiInfo { Title = "NeQ.Api", Version = "0.1" });
                options.OrderActionsBy(o => o.RelativePath);
                var file = Path.Combine(AppContext.BaseDirectory, "NeQ.blog.api.xml");
                options.IncludeXmlComments(file, true);
            });

            var app = builder.Build();

            ////�������ݿ�ͱ�ṹ��
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
