using Ambe.Frontend.Models;
using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class ViajesController : Controller
    {

        private readonly IServicioViajes _viajes;
        private readonly IServicioLista _lista;
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;
        private readonly IServicioPersonas _personas;

        public ViajesController(IServicioViajes viajes, IServicioLista lista, IHttpClientFactory httpClientFactory, 
            IBitacoraService bitacoraService, IServicioPersonas personas)
        {
            _viajes = viajes;
            _lista = lista;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
            _personas = personas;
        }

        public async Task<IActionResult> Index()
        {
            var viajes = await _viajes.GetViajesAsync();
            return View(viajes);
        }

        public async Task<IActionResult> Create()
        {
            var viajes = new Viajes()
            {
                IdInstituto = 1,
                Estado = "Nuevo",
                CreadoPor = User.Identity!.Name!,
                ModificadoPor = User.Identity!.Name!,
                FechaDeCreacion = DateTime.Now,
                FechaDeModificacion = DateTime.Now,
                Unidades = await _lista.GetListaUnidades(),
                TipoViajes = await _lista.GetListaTiposViaje(),
                Conductores = await _lista.GetListaConductores(),
                Nineras = await _lista.GetListaNineras()
            };
            return View(viajes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Viajes viajes)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(viajes);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Viajes/", content);

                if (response.IsSuccessStatusCode)
                {
                    var email = Uri.EscapeDataString(User!.Identity!.Name!);
                    var user = await _bitacora.ObtenerUsuario(email);
                    var bitacora = new BitacoraViewModel()
                    {
                        IdUsuario = user!.IdUsuario,
                        Usuario = user!.NombreUsuario,
                        IdInstituto = 1,
                        TipoAccion = "Creó",
                        Tabla = "Viajes",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Viaje creado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear el viaje.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear el viaje!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            viajes.TipoViajes = await _lista.GetListaTiposViaje();
            viajes.Conductores = await _lista.GetListaConductores();
            viajes.Nineras = await _lista.GetListaNineras();
            viajes.Unidades = await _lista.GetListaUnidades();
            return View(viajes);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Viajes/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener viaje.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var viajes = JsonConvert.DeserializeObject<Viajes>(jsonString);

            viajes!.Estado = "Nuevo";
            viajes.TipoViajes = await _lista.GetListaTiposViaje();
            viajes.Unidades = await _lista.GetListaUnidades();
            viajes.Nineras = await _lista.GetListaNineras();
            viajes.Conductores = await _lista.GetListaConductores();
            return View(viajes);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Viajes viajes)
        {
            if (ModelState.IsValid)
            {
                
                var json = JsonConvert.SerializeObject(viajes);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Viajes/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    var email = Uri.EscapeDataString(User!.Identity!.Name!);
                    var user = await _bitacora.ObtenerUsuario(email);
                    var bitacora = new BitacoraViewModel()
                    {
                        IdUsuario = user!.IdUsuario,
                        Usuario = user!.NombreUsuario,
                        IdInstituto = 1,
                        TipoAccion = "Editó",
                        Tabla = "Viajes",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Viaje actualizado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar el viaje.";
                    return RedirectToAction("Index");
                }
            }
            return View(viajes);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Viajes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var email = Uri.EscapeDataString(User!.Identity!.Name!);
                var user = await _bitacora.ObtenerUsuario(email);
                var bitacora = new BitacoraViewModel()
                {
                    IdUsuario = user!.IdUsuario,
                    Usuario = user!.NombreUsuario,
                    IdInstituto = 1,
                    TipoAccion = "Eliminó",
                    Tabla = "Vaijes",
                    Fecha = DateTime.Now
                };
                await _bitacora.AgregarRegistro(bitacora);
                TempData["AlertMessage"] = "Viaje eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar el viaje, ya que tiene registros relacionados";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> BitacoraViaje(int idViaje)
        {
            var viajes = await _viajes.GetBitacoraViajeAsync(idViaje);
            return View(viajes);
        }

        public async Task<IActionResult> ListaAlumnos(int idViaje)
        {
            var persona = await _personas.GetPersonasAsync();

            var viajes = await _viajes.GetRegistroViajeAsync(idViaje);

            var alumnosDTO = viajes.Select(v => new AlumnosDTO
            {
                IdViaje = v.IdViaje,
                IdRegistroViaje = v.IdRegistroViaje,
                IdPersonaAlumno = v.IdPersonaAlumno,
                NombreAlumno = persona.FirstOrDefault(tp => tp.IdPersona == v.IdPersonaAlumno)?.NombreCompleto ?? "Desconocido",
                CreadoPor = v.CreadoPor,
                FechaDeCreacion = v.FechaDeCreacion,
            });
            return View(alumnosDTO);
        }

        public async Task<IActionResult> Incidentes(int idViaje)
        {
            var viajes = await _viajes.GetIncidentesAsync(idViaje);
            return View(viajes);
        }
    }
}
