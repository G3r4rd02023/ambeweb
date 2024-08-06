using Ambe.Frontend.Models.Entidades;
using Newtonsoft.Json;

namespace Ambe.Frontend.Services
{
    public class ServicioViajes : IServicioViajes
    {

        private readonly HttpClient _httpClient;
        public ServicioViajes(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
        }
        public async Task<IEnumerable<Viajes>> GetViajesAsync()
        {
            var response = await _httpClient.GetAsync("/api/Viajes");
            var content = await response.Content.ReadAsStringAsync();
            var viajes = JsonConvert.DeserializeObject<IEnumerable<Viajes>>(content);
            return viajes!;
        }

        public async Task<IEnumerable<BitacoraViaje>> GetBitacoraViajeAsync(int idViaje)
        {
            var response = await _httpClient.GetAsync("/api/BitacoraViaje");
            var content = await response.Content.ReadAsStringAsync();
            var viajes = JsonConvert.DeserializeObject<IEnumerable<BitacoraViaje>>(content);
            var filtro = viajes!.Where(v => v.IdViaje == idViaje);
            return filtro;
        }

        public async Task<IEnumerable<RegistroViaje>> GetRegistroViajeAsync(int idViaje)
        {
            var response = await _httpClient.GetAsync("/api/RegistroViaje");
            var content = await response.Content.ReadAsStringAsync();
            var viajes = JsonConvert.DeserializeObject<IEnumerable<RegistroViaje>>(content);
            var filtro = viajes!.Where(v => v.IdViaje == idViaje);
            return filtro;
        }

        public async Task<IEnumerable<Incidentes>> GetIncidentesAsync(int idViaje)
        {
            var response = await _httpClient.GetAsync("/api/Incidentes");
            var content = await response.Content.ReadAsStringAsync();
            var viajes = JsonConvert.DeserializeObject<IEnumerable<Incidentes>>(content);
            var filtro = viajes!.Where(v => v.IdViaje == idViaje);
            return filtro;
        }
    }
}
