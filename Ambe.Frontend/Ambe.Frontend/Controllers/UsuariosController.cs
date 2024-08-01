using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;

        public UsuariosController(IHttpClientFactory httpClientFactory, IBitacoraService bitacora)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");  
            _bitacora = bitacora;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Usuarios");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<IEnumerable<Usuarios>>(content);
                return View("Index", usuarios);
            }
            return View(new List<Usuarios>());
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                usuarios.FechaCreacion = DateTime.Now;                               
                usuarios.IdRol = 2;
                var json = JsonConvert.SerializeObject(usuarios);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Usuarios/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Usuario creado exitosamente!!!";
                    var email = Uri.EscapeDataString(usuarios.CorreoElectronico);
                    var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
                    var usuarioJson = await userResponse.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<Usuarios>(usuarioJson);
                    var model = new BitacoraViewModel()
                    {
                        IdUsuario = user!.IdUsuario,
                        Usuario = user!.NombreUsuario,
                        IdInstituto = 1,
                        TipoAccion = "Consultó",
                        Tabla = "Bitacora",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(model);                    
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al crear el usuario administrador!!!";
                }
            }

            return View(usuarios);
        }
    }
}
