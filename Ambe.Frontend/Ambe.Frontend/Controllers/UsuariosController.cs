using Ambe.Frontend.Models;
using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
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

                    var user = await _bitacora.ObtenerUsuario(email);

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

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Usuario eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el usuario.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> CambiarPassword()
        {
            var email = Uri.EscapeDataString(User.Identity!.Name!);

            var user = await _bitacora.ObtenerUsuario(email);
            if (user == null)
            {
                return NotFound();
            }

            PasswordViewModel model = new()
            {
                UserId = user.IdUsuario,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarPassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Uri.EscapeDataString(User.Identity!.Name!);
                var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");

                if (userResponse.IsSuccessStatusCode)
                {
                    var usuarioJson = await userResponse.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<Usuarios>(usuarioJson);

                    if (user != null)
                    {
                        var changePasswordModel = new
                        {
                            UserId = model.UserId,
                            OldPassword = model.OldPassword,
                            NewPassword = model.NewPassword,
                            Confirmation = model.Confirmation
                        };

                        var content = new StringContent(JsonConvert.SerializeObject(changePasswordModel), Encoding.UTF8, "application/json");
                        var resultResponse = await _httpClient.PutAsync($"/api/Usuarios/CambiarPassword/{model.UserId}", content);

                        if (resultResponse.IsSuccessStatusCode)
                        {
                            var bitacora = new BitacoraViewModel()
                            {
                                IdUsuario = user!.IdUsuario,
                                Usuario = user!.NombreUsuario,
                                IdInstituto = 1,
                                TipoAccion = "Cambió contraseña",
                                Tabla = "Usuarios",
                                Fecha = DateTime.Now
                            };
                            await _bitacora.AgregarRegistro(bitacora);
                            TempData["AlertMessage"] = "La clave se ha modificado exitosamente!";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            var errorResponse = await resultResponse.Content.ReadAsStringAsync();

                            ModelState.AddModelError(string.Empty, errorResponse);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al obtener el usuario.");
                }
            }
            return View(model);
        }
    }
}
