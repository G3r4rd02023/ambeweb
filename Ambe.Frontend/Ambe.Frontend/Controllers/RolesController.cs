using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class RolesController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;

        public RolesController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Roles");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<IEnumerable<Roles>>(content);
                return View("Index", roles);
            }
            return View(new List<Roles>());
        }

        public IActionResult Create()
        {
            var roles = new Roles()
            {
                IdInstituto = 1,
                FechaCreacion = DateTime.Now,
                CreadoPor = User.Identity!.Name!,
                FechaModificacion = DateTime.Now,
                ModificadoPor = User.Identity!.Name!,                
            };
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Roles roles)
        {
            if (ModelState.IsValid)
            {
                
                var json = JsonConvert.SerializeObject(roles);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Roles/", content);

                if (response.IsSuccessStatusCode)
                {
                    var email = Uri.EscapeDataString(User!.Identity!.Name!);

                    var user = await _bitacora.ObtenerUsuario(email);

                    var bitacora = new BitacoraViewModel()
                    {
                        IdUsuario = user!.IdUsuario,
                        Usuario =  user!.NombreUsuario,
                        IdInstituto = 1,
                        TipoAccion = "Creó",
                        Tabla = "Roles",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Rol creado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear el parametro.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear el parametro!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            return View(roles);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Roles/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Rol eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el rol.";
                return RedirectToAction("Index");
            }
        }
    }
}
