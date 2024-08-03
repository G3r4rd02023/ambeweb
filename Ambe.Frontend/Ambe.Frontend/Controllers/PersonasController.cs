using Ambe.Frontend.Models;
using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ambe.Frontend.Controllers
{
    public class PersonasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;
        private readonly IServicioPersonas _personas;

        public PersonasController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService, IServicioPersonas personas)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
            _personas = personas;
        }

        public async Task<IActionResult> Index()
        {
            var personas = await _personas.GetPersonasAsync();
            var tiposPersona = await _personas.GetTiposPersonaAsync();

            var personasDTO = personas.Select(p => new PersonaDTO
            {
                IdPersona = p.IdPersona,
                IdTipoPersona = p.IdTipoPersona,
                Descripcion = tiposPersona.FirstOrDefault(tp => tp.IdTipoPersona == p.IdTipoPersona)?.TipoPersona ?? "Desconocido",
                NombreCompleto = p.PrimerNombre + " " + p.SegundoNombre,
                FechaCreacion = p.FechaCreacion,
                Estado = p.Estado
            }).ToList();

            return View(personasDTO);

        }

        public async Task<IActionResult> VerContacto(int idPersona)
        {           
            var contactos = await _personas.GetContactosAsync(idPersona);
            if (contactos == null || !contactos.Any())
            {
                ViewBag.Message = "No se encontraron contactos para esta persona.";
            }
            return View(contactos);
        }
    }
}
