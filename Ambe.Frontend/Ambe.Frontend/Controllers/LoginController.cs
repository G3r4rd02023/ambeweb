using Ambe.Frontend.Models.Entidades;
using Ambe.Frontend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace Ambe.Frontend.Controllers
{
    public class LoginController : Controller
    {

        private readonly HttpClient _httpClient;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var email = Uri.EscapeDataString(model.Email);
                var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
                var usuarioJson = await userResponse.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<Usuarios>(usuarioJson);

                var response = await _httpClient.PostAsync("/api/Login/IniciarSesion", content);
                if (response.IsSuccessStatusCode)
                {

                    var result = await _httpClient.GetAsync($"/api/Roles/{usuario!.IdRol}");
                    var rolJson = await result.Content.ReadAsStringAsync();
                    var rol = JsonConvert.DeserializeObject<Roles>(rolJson);
                    var descripcion = rol!.Descripcion;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Role, descripcion),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ViewData["AlertMessage"] = "Usuario o clave incorrectos!!! numero de intentos: ";
                }

            }
            return View(model);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("IniciarSesion", "Login");
        }
    }
}
