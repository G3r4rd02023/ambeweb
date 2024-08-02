using Ambe.Frontend.Models;
using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;


namespace Ambe.Frontend.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBitacoraService _bitacora;
        private readonly IServicioLista _lista;

        public UsuariosController(IHttpClientFactory httpClientFactory, IBitacoraService bitacora, IServicioLista lista)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");  
            _bitacora = bitacora;
            _lista = lista;
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
            PersonaViewModel model = new()
            {
                IdTipoPersona = 1,
                IdInstituto = 1,                
                Estado = "Activo",
                IdRol = 2,
                FechaUltimaConexion = DateTime.Now,
                Usuario = "usuario",
                NombreUsuario = "nombreUsuario",
                CreadoPor = "usuario",
                ModificadoPor = "usuario"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonaViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Usuario = model.PrimerNombre + " " + model.PrimerApellido!;
                model.NombreUsuario = model.CorreoElectronico;
                model.CreadoPor = User.Identity!.Name!;
                model.ModificadoPor = User.Identity!.Name!;

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Usuarios/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Usuario creado exitosamente!!!";
                    var email = Uri.EscapeDataString(User!.Identity!.Name!);
                    var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
                    var usuarioJson = await userResponse.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<Usuarios>(usuarioJson);
                    var bitacora = new BitacoraViewModel()
                    {
                        IdUsuario = user!.IdUsuario,
                        Usuario = user!.NombreUsuario,
                        IdInstituto = 1,
                        TipoAccion = "Creó",
                        Tabla = "Usuarios",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);                    
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al crear el usuario administrador!!!";
                }
            }

            return View(model);
        }

        public async Task<IActionResult> AprobarUsuario(int id)
        {
            var response = await _httpClient.GetAsync($"api/Usuarios/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Usuarios>(json);
            if (user == null)
            {
                return NotFound();
            }

            user.Roles = await _lista.GetListaRoles();
            user.Estados = _lista.GetListaEstados();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> AprobarUsuario(Usuarios user)
        {
            if (ModelState.IsValid)
            {
                user.FechaUltimaConexion = DateTime.Now;
                user.FechaModificacion = DateTime.Now;
                
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Usuarios/{user.IdUsuario}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Usuario actualizado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {

                    TempData["ErrorMessage"] = "Error al actualizar usuario!!";
                }
            }
            return View(user);
        }
    }
}
