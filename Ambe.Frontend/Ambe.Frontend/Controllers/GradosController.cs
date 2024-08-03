using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class GradosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;

        public GradosController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Grados");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var grados = JsonConvert.DeserializeObject<IEnumerable<Grados>>(content);
                return View("Index", grados);
            }
            return View(new List<Grados>());
        }

        public IActionResult Create()
        {
            var grados = new Grados()
            {
                IdInstituto = 1,
                Estado = "Activo",
                FechaDeCreacion = DateTime.Now,
                CreadoPor = User.Identity!.Name!,
                FechaDeModificacion = DateTime.Now,
                ModificadoPor = User.Identity!.Name!,
            };
            return View(grados);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Grados grados)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(grados);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Grados/", content);

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
                        Tabla = "Grados",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Grado creado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear el grado.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear el grado!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            return View(grados);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Grados/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Grado eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el grado.";
                return RedirectToAction("Index");
            }
        }
    }
}
