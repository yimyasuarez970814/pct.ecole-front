using System.Net;
using System.Net.Mail;

namespace pct.ecole.services
{
    public interface IEmailService {
        Task<bool> GestionEnvioCorreoElectronicoAsync(string asunto, string destinatario, string mensaje, bool esHtml = true);
    }

    public class EmailService : IEmailService
    {
        private SmtpClient cliente;
        private MailMessage email;
        private IConfiguration _configuration;
        private ILogger<EmailService> _logger;
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            cliente = new SmtpClient(_configuration.GetValue<string>("CorreoElectronico:host"),
                        Int32.Parse(_configuration.GetValue<string>(("CorreoElectronico:port"))))
            {
                EnableSsl = bool.Parse(_configuration.GetValue<string>("CorreoElectronico:enableSsl")),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration.GetValue<string>("CorreoElectronico:Usuario"), _configuration.GetValue<string>("CorreoElectronico:contrasena"))
            };
            _logger = logger;
        }

        public async Task<bool> GestionEnvioCorreoElectronicoAsync(string asunto, string destinatario, string mensaje, bool esHtml = true)
        {
            try
            {
                var activo = _configuration.GetValue<bool>("CorreoElectronico:Activo");
                if (!activo) return false;
                string remitente = _configuration.GetValue<string>("CorreoElectronico:Usuario");
                await EnviarCorreoAsync(remitente, destinatario, asunto, mensaje, esHtml);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.InnerException + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        public async Task EnviarCorreoAsync(string remitente, string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            var destinatarios = destinatario;
            if (!string.IsNullOrEmpty(_configuration.GetValue<string>("CorreoElectronico:tramitante")))
            {
                destinatarios = destinatario + "," + _configuration.GetValue<string>("CorreoElectronico:tramitante");
            }
            destinatarios = destinatarios.TrimEnd(',');
            if (string.IsNullOrEmpty(destinatarios)) return;
            email = new MailMessage(remitente, destinatarios, asunto, mensaje)
            {
                IsBodyHtml = esHtlm
            };
            await cliente.SendMailAsync(email);
        }

    }
}



