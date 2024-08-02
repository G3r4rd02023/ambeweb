using Ambe.Frontend.Models.Entidades;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Ambe.Frontend.Services
{
    public class ServicioLista : IServicioLista
    {
        private readonly HttpClient _httpClient;

        public ServicioLista(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
        }
        public IEnumerable<SelectListItem> GetListaEstados()
        {
            var estados = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nuevo", Value = "Nuevo" },
                new SelectListItem { Text = "Activo", Value = "Activo" },
                new SelectListItem { Text = "Inactivo", Value = "Inactivo" },
                new SelectListItem { Text = "Bloqueado", Value = "Bloqueado" }
            };

            return estados;
        }

        public async Task<IEnumerable<SelectListItem>> GetListaRoles()
        {
            var response = await _httpClient.GetAsync("/api/Roles");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<IEnumerable<Roles>>(content);
                var listaRoles = roles!.Select(c => new SelectListItem
                {
                    Value = c.IdRol.ToString(),
                    Text = c.Descripcion
                }).ToList();

                listaRoles.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione un Rol"
                });
                return listaRoles;
            }

            return [];
        }
    }
}
