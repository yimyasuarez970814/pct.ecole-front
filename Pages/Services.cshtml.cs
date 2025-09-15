using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pct.ecole.services;
using pct.ecole_front.Services;

namespace pct.ecole_front.Pages
{
    public class ServicesModel : PageModel
    {

        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ServicesModel(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public int Service { get; set; }
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

            var servicioDescripcion = EnumExtensions.GetDescription((TiposServicio)Service);
            string cuerpoMensaje = $"Nombre: {Name}<br>Email:" +
                $" {Email}<br>Mensaje: Quiero realizar una cotización sobre el servició" +
                $" {servicioDescripcion} <br>";


            var titulo = _configuration.GetValue<string>("CorreoElectronico:titulo");
            var tituloEnviar = titulo + servicioDescripcion;
            cuerpoMensaje += _configuration.GetValue<string>("CorreoElectronico:cuerpo");


            bool resultado = await _emailService.GestionEnvioCorreoElectronicoAsync(
               servicioDescripcion,
               Email,
               cuerpoMensaje,
               true
           );

            RespuestaCorreo = resultado ? "Correo enviado exitosamente" : "Error enviando correo";

            return RedirectToPage("/Services", new { mensaje = "Correo enviado exitosamente" });
        }
    }
}
