using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using pct.ecole.services;

namespace pct.ecole_front.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ContactModel(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Subject { get; set; }
        [BindProperty] public string Message { get; set; }
        [TempData] public string RespuestaCorreo { get; set; }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostSend()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string cuerpoMensaje = $"Nombre: {Name}<br>Email: {Email}<br>Mensaje: {Message} <br>";


            var titulo = _configuration.GetValue<string>("CorreoElectronico:titulo");
            var tituloEnviar = titulo + Subject;
            cuerpoMensaje += _configuration.GetValue<string>("CorreoElectronico:cuerpo");


            bool resultado = await _emailService.GestionEnvioCorreoElectronicoAsync(
               Subject,
               Email,
               cuerpoMensaje,
               true
           );

            RespuestaCorreo = resultado ? "Correo enviado exitosamente" : "Error enviando correo";

            return RedirectToPage("/Contact", new { mensaje = "Correo enviado exitosamente" });
        }
    }
}
