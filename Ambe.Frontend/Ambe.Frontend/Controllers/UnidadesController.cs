using Ambe.Frontend.Models;
using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class UnidadesController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;
        private readonly IServicioPersonas _personas;
        private readonly IServicioLista _lista;

        public UnidadesController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService, IServicioPersonas personas, 
            IServicioLista lista)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
            _personas = personas;
            _lista = lista;
        }
        public async Task<IActionResult> Index()
        {
            var persona = await _personas.GetPersonasAsync();

            var response = await _httpClient.GetAsync("/api/Unidades");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var unidades = JsonConvert.DeserializeObject<IEnumerable<Unidades>>(content);

                var unidadesDTO = unidades!.Select(u => new UnidadesDTO
                {
                    IdUnidad = u.IdUnidad,
                    Capacidad = u.Capacidad,
                    IdPersonaConductor = u.IdPersonaConductor,
                    NombreConductor = persona.FirstOrDefault(tp => tp.IdPersona == u.IdPersonaConductor)?.NombreCompleto??"Desconocido",
                    Placa = u.Placa,
                    NumeroUnidad = u.NumeroUnidad
                });
                return View("Index", unidadesDTO);
            }
            return View(new List<Unidades>());
        }

        public async Task<IActionResult> Create()
        {
            var unidades = new Unidades()
            {
                IdInstituto = 1,
                CreadoPor = User.Identity!.Name!,
                ModificadoPor = User.Identity!.Name!,
                FechaDeCreacion = DateTime.Now,
                FechaDeModificacion = DateTime.Now,
                Marcas = await _lista.GetListaMarcas(),
                Modelos = await _lista.GetListaModelos(),
                Conductores = await _lista.GetListaConductores()
            };
            return View(unidades);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Unidades unidades)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(unidades);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Unidades/", content);

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
                        Tabla = "Unidades",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Unidad creada exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear la unidad.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear la unidad!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            unidades.Marcas = await _lista.GetListaMarcas();
            unidades.Modelos = await _lista.GetListaModelos();
            unidades.Conductores = await _lista.GetListaConductores();
            return View(unidades);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Unidades/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener unidad.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var unidad = JsonConvert.DeserializeObject<Unidades>(jsonString);

            unidad!.Marcas = await _lista.GetListaMarcas();
            unidad.Modelos = await _lista.GetListaModelos();
            unidad.Conductores = await _lista.GetListaConductores();
            return View(unidad);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Unidades unidad)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(unidad);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Unidades/{id}", content);
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
                        Tabla = "Unidades",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Unidad actualizada exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar la unidad.";
                    return RedirectToAction("Index");
                }
            }
            return View(unidad);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Unidades/{id}");

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
                    Tabla = "Unidades",
                    Fecha = DateTime.Now
                };
                await _bitacora.AgregarRegistro(bitacora);
                TempData["AlertMessage"] = "Unidad eliminada exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar la unidad.";
                return RedirectToAction("Index");
            }
        }
    }
}
