using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class ParametrosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;

        public ParametrosController(IHttpClientFactory httpClientFactory, IBitacoraService bitacoraService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
            _bitacora = bitacoraService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Parametros");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var parametros = JsonConvert.DeserializeObject<IEnumerable<Parametros>>(content);
                return View("Index", parametros);
            }
            return View(new List<Parametros>());
        }

        public async Task<IActionResult> Create()
        {
            var email = Uri.EscapeDataString(User.Identity!.Name!);
            var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
            var usuarioJson = await userResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Usuarios>(usuarioJson);

            Parametros parametros = new()
            {
                IdUsuario = user!.IdUsuario
            };
            return View(parametros);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Parametros parametros)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(parametros);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Parametros/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Parametro creado exitosamente!!!";
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
            return View(parametros);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Parametros/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener parametro.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var parametros = JsonConvert.DeserializeObject<Parametros>(jsonString);

            return View(parametros);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Parametros parametros)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(parametros);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Parametros/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Parametro actualizado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar el parametro.";
                    return RedirectToAction("Index");
                }
            }
            return View(parametros);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Parametros/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Parametro eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el parametro.";
                return RedirectToAction("Index");
            }
        }
    }
}
