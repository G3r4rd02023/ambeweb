using Ambe.Frontend.Models.Entidades;
using Newtonsoft.Json;
using System.Text;

namespace Ambe.Frontend.Services
{
    public class BitacoraService : IBitacoraService
    {
        private readonly HttpClient _httpClient;
        public BitacoraService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
        }
        public async Task<Bitacora> AgregarRegistro(BitacoraViewModel model)
        {
            Bitacora bitacora = new()
            {
                TipoAccion = model.TipoAccion,
                IdUsuario = model.IdUsuario,
                Fecha = DateTime.Now,
                IdInstituto = model.IdInstituto,
                Tabla = model.Tabla
            };

            var json = JsonConvert.SerializeObject(bitacora);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Bitacora", content);
            if (response.IsSuccessStatusCode)
            {
                return model;
            }
            return bitacora;
        }

        public async Task<Usuarios> ObtenerUsuario(string email)
        {
            var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
            var usuarioJson = await userResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Usuarios>(usuarioJson);
            return user!;
        }

    }
}
