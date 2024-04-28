using SendGrid;
using SendGrid.Helpers.Mail;

public class PasswordRecoveryService
{
    private readonly ILogger<PasswordRecoveryService> _logger;
    public PasswordRecoveryService(ILogger<PasswordRecoveryService> logger)
    {
        _logger = logger;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string message, string fromEmail, string fromName)
    {
        _logger.LogInformation("*---------------------- ENVIANDO E-MAIL SENDGRID *----------------------");

        try
        {
            string apiKey = Environment.GetEnvironmentVariable("SendGrid:ApiKey", EnvironmentVariableTarget.User);
            var client = new SendGridClient(apiKey);

            _logger.LogInformation("SendGrid API Key: {ApiKey}", apiKey);

            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, fromName);

            var response = await client.SendEmailAsync(msg);

            if (response != null && (response.StatusCode == System.Net.HttpStatusCode.OK ||
                                   response.StatusCode == System.Net.HttpStatusCode.Accepted))
            {
                _logger.LogInformation("Email sent successfully");
                _logger.LogInformation("message before logging: {message}", message);
                _logger.LogDebug("Message before sending: {Message}", message);
                _logger.LogInformation("Email being sent: {@Msg}", msg);
            }
            else
            {
                _logger.LogError("Failed to send email. Response: {@Response}", response, msg);

            }
        }
            
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email.");
            throw; 
        }
    }
}
