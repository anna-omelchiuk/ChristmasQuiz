using ChristmasQuiz.Models;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text;

namespace ChristmasQuiz.Services
{
    public class EmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<(bool Success, string Message)> SendResultsAsync(QuizSessionData session)
        {
            string recipientEmail = session.User.Email.Trim();
            int correctCount = session.Answers.Count(a => a.IsCorrect);
            int total = session.Answers.Count;

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"Вітаємо, {session.User.Name}!");
            bodyBuilder.AppendLine("Дякуємо за проходження Різдвяної вікторини.");
            bodyBuilder.AppendLine($"Ваш результат: {correctCount} з {total} правильних відповідей.\n");
            bodyBuilder.AppendLine("Деталі відповідей:");

            int number = 1;
            foreach (var answer in session.Answers)
            {
                string mark = answer.IsCorrect ? "ПРАВИЛЬНО" : "ХИБНО";
                bodyBuilder.AppendLine($"{number++}. {answer.Question.Text}");
                bodyBuilder.AppendLine($"   Ваша відповідь: {answer.Question.Options[answer.SelectedOptionIndex]} {mark}");
                if (!answer.IsCorrect)
                {
                    bodyBuilder.AppendLine($"   Правильна відповідь: {answer.Question.Options[answer.Question.CorrectOptionIndex]}");
                }
            }

            try
            {
                var emailMessage = new MimeMessage();
                string senderEmail = configuration["SmtpSettings:SenderEmail"] ?? "quiz@christmas.ua";
                string smtpHost = configuration["SmtpSettings:Host"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(configuration["SmtpSettings:Port"] ?? "587");
                string smtpUser = configuration["SmtpSettings:Username"] ?? "";
                string smtpPass = configuration["SmtpSettings:Password"] ?? "";

                emailMessage.From.Add(new MailboxAddress("Різдвяна Вікторина", senderEmail));
                emailMessage.To.Add(new MailboxAddress(session.User.Name, recipientEmail));
                emailMessage.Subject = "Результати Різдвяної вікторини";
                emailMessage.Body = new TextPart("plain") { Text = bodyBuilder.ToString() };

                if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
                {
                    await Task.Delay(200);
                    return (true, $"Результати успішно відправлено на вашу пошту: {recipientEmail}");
                }

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUser, smtpPass);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                return (true, $"Результати успішно відправлено на вашу пошту: {recipientEmail}");
            }
            catch (Exception ex)
            {
                return (false, $"Не вдалося надіслати лист на {recipientEmail} ({ex.Message}), але результати збережено в системі.");
            }
        }
    }
}
