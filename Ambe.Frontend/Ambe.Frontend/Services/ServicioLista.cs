using Ambe.Frontend.Models.Entidades;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NuGet.Versioning;

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

        public async Task<IEnumerable<SelectListItem>> GetListaMarcas()
        {
            var response = await _httpClient.GetAsync("/api/Marcas");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var marcas = JsonConvert.DeserializeObject<IEnumerable<Marcas>>(content);
                var listaMarcas = marcas!.Select(c => new SelectListItem
                {
                    Value = c.IdMarca.ToString(),
                    Text = c.NombreMarca
                }).ToList();

                listaMarcas.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione una Marca"
                });
                return listaMarcas;
            }

            return [];
        }

        public async Task<IEnumerable<SelectListItem>> GetListaModelos()
        {
            var response = await _httpClient.GetAsync("/api/Modelos");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var modelos = JsonConvert.DeserializeObject<IEnumerable<Modelos>>(content);                
                var listaModelos = modelos!.Select(c => new SelectListItem
                {
                    Value = c.IdModelo.ToString(),
                    Text = c.NombreModelo
                }).ToList();

                listaModelos.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione un Modelo"
                });
                return listaModelos;
            }

            return [];
        }

        public async Task<IEnumerable<SelectListItem>> GetListaConductores()
        {
            var response = await _httpClient.GetAsync("/api/Personas");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var personas = JsonConvert.DeserializeObject<IEnumerable<Personas>>(content);
                var conductores = personas!.Where(p => p.IdTipoPersona == 2);
                var listaConductores = conductores!.Select(c => new SelectListItem
                {
                    Value = c.IdPersona.ToString(),
                    Text = c.NombreCompleto
                }).ToList();

                listaConductores.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione un Conductor"
                });
                return listaConductores;
            }

            return [];
        }
    }
}
