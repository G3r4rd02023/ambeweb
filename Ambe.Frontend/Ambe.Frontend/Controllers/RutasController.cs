using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class RutasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;

        public RutasController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Rutas");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rutas = JsonConvert.DeserializeObject<IEnumerable<Rutas>>(content);
                return View("Index", rutas);
            }
            return View(new List<Rutas>());
        }

        public IActionResult Create()
        {
            var rutas = new Rutas()
            {
                IdInstituto = 1,
                Estado = "Activo",
                Departamento = "Francisco Morazán",
                Municipio = "Tegucigalpa MDC",
                FechaDeCreacion = DateTime.Now,
                CreadoPor = User.Identity!.Name!,
                FechaDeModificacion = DateTime.Now,
                ModificadoPor = User.Identity!.Name!,
            };
            return View(rutas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rutas rutas)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(rutas);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Rutas/", content);

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
                        Tabla = "Rutas",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Ruta creada exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear la ruta.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear la ruta!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            return View(rutas);
        }

        public async Task<IActionResult> Edit(int id)
        {            
            var response = await _httpClient.GetAsync($"/api/Rutas/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener ruta.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var ruta = JsonConvert.DeserializeObject<Rutas>(jsonString);

            ruta!.Estado = "Activo";
            return View(ruta);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Rutas ruta)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(ruta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Rutas/{id}", content);
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
                        Tabla = "Rutas",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Ruta actualizada exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar la ruta.";
                    return RedirectToAction("Index");
                }
            }
            return View(ruta);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Rutas/{id}");

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
                    Tabla = "Rutas",
                    Fecha = DateTime.Now
                };
                await _bitacora.AgregarRegistro(bitacora);
                TempData["AlertMessage"] = "Ruta eliminada exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar la ruta.";
                return RedirectToAction("Index");
            }
        }
    }
}
