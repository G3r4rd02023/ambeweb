using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class InstitutosController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;

        public InstitutosController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Institutos");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var institutos = JsonConvert.DeserializeObject<IEnumerable<Institutos>>(content);
                return View("Index", institutos);
            }
            return View(new List<Institutos>());
        }

        public IActionResult Create()
        {
            var institutos = new Institutos()
            {
                CreadoPor = User.Identity!.Name!,
                ModificadoPor = User.Identity!.Name!,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };
            return View(institutos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Institutos institutos)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(institutos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Institutos/", content);

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
                        Tabla = "Institutos",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Instituto creado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear el instituto.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear el instituto!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            return View(institutos);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Institutos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener instituto.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var instituto = JsonConvert.DeserializeObject<Institutos>(jsonString);

            return View(instituto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Institutos institutos)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(institutos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Institutos/{id}", content);
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
                        Tabla = "Institutos",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Institutos actualizado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar el instituto.";
                    return RedirectToAction("Index");
                }
            }
            return View(institutos);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Institutos/{id}");

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
                    Tabla = "Institutos",
                    Fecha = DateTime.Now
                };
                await _bitacora.AgregarRegistro(bitacora);
                TempData["AlertMessage"] = "Instituto eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el instituto.";
                return RedirectToAction("Index");
            }
        }
    }
}
