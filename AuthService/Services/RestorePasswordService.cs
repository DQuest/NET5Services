using System;
using System.Threading.Tasks;
using AuthService.Interfaces;
using AuthService.Models.RestorePassword;
using AuthService.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AuthService.Services
{
    public class RestorePasswordService : IRestorePasswordService
    {
        private readonly string _siteEmail;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public RestorePasswordService(UserManager<User> userManager, IConfiguration configuration)
        {
            _siteEmail = configuration.GetValue<string>("SiteEmail");
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public async Task<IActionResult> RestorePassword(RestorePasswordModel restorePasswordModel)
        {
            return null;
            // var user = await _userManager.FindByEmailAsync(restorePasswordModel.Email);
            //
            // if (user == null)
            // {
            //     return new NotFoundObjectResult(new RestorePasswordModel
            //     {
            //         Email = restorePasswordModel.Email
            //     });
            // }
            //
            // // todo админ не восстанавливает пароль, как крутые парни не смотрят на взрыв, проверить роли
            //
            // var emailMessage = new MimeMessage();
            // emailMessage.From.Add(new MailboxAddress("Администрация", _siteEmail));
            // emailMessage.To.Add(new MailboxAddress("Пользователь", restorePasswordModel.Email));
            // emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            // {
            //     Text = $"Вы запросили восстановление пароля на сайте {_siteEmail}"
            //            + "<br>Ссылка на страницу восстановления пароля: "
            //            + "<br><h5>ссылка действительна в течение часа</h5>"
            // };
            //
            // using var client = new SmtpClient();
            // await client.ConnectAsync("smtp.true.com", 25, false);
            //
            // await client.AuthenticateAsync("site@true.com", "password");
            // await client.SendAsync(emailMessage);
            //
            // await client.DisconnectAsync(true);
        }
    }
}