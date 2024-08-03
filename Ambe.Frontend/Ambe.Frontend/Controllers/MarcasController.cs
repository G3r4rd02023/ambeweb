using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class MarcasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;
        private readonly IServicioPersonas _personas;

        public MarcasController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService,IServicioPersonas personas)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
            _personas = personas;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Marcas");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var marcas = JsonConvert.DeserializeObject<IEnumerable<Marcas>>(content);
                return View("Index", marcas);
            }
            return View(new List<Marcas>());
        }

        public IActionResult Create()
        {
            var marcas = new Marcas()
            {
                IdInstituto = 1,
                Estado = "Activo",
                FechaDeCreacion = DateTime.Now,
                CreadoPor = User.Identity!.Name!,
                FechaDeModificacion = DateTime.Now,
                ModificadoPor = User.Identity!.Name!,
            };
            return View(marcas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Marcas marcas)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(marcas);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Marcas/", content);

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
                        Tabla = "Marcas",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Marca creada exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear la marca.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear la marca!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            return View(marcas);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Marcas/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Marca eliminada exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la marca, ya que tiene registros relacionados";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var modelos = await _personas.GetModelosAsync(id);
            return View(modelos);
        }

        public IActionResult AddModel(int id)
        {
            var modelo = new Modelos()
            {
                IdMarca = id,
                Estado = "Activo",
                FechaDeCreacion = DateTime.Now,
                CreadoPor = User.Identity!.Name!,
                FechaDeModificacion = DateTime.Now,
                ModificadoPor = User.Identity!.Name!,
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(Modelos modelo)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(modelo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Modelos/", content);

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
                        Tabla = "Marcas",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Modelo agregado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear el modelo.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear el modelo!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            return View(modelo);
        }

        public async Task<IActionResult> DeleteModel(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Modelos/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Modelo exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar el modelo, ya que tiene registros relacionados";
                return RedirectToAction("Index");
            }
        }
    }
}
