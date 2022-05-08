using MimeKit;
using System;

namespace ElRegistratura.Email
{
    public class Service
    {
        public void SendEmailCustom()
        {
            //try
            //{
            //    MimeMessage message = new MimeMessage();
            //    message.From.Add(new MailboxAddress("Электронная регистратура", "admin@mycompany.com")); //отправитель сообщения
            //    message.To.Add(new MailboxAddress("Gльзователь","marina_gritsanik@mail.ru")); //адресат сообщения
            //    message.Subject = "Сообщение от MailKit"; //тема сообщения
            //    message.Body = new BodyBuilder() { HtmlBody = "<div style=\"color: green;\">Сообщение от MailKit</div>" }.ToMessageBody(); //тело сообщения (так же в формате HTML)

            //    using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            //    {
            //        client.Connect("smtp.gmail.com", 465, true); //либо использум порт 465
            //        client.Authenticate("elektronnayregistra@gmail.com", "652299QW"); //логин-пароль от аккаунта
            //        client.Send(message);

            //        client.Disconnect(true);
            //        Console.WriteLine("Сообщение отправлено");
            //        //logger.LogInformation("Сообщение отправлено успешно!");
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
        }
    }
}
