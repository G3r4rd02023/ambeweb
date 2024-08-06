using Ambe.Frontend.Models.Entidades;
using Newtonsoft.Json;

namespace Ambe.Frontend.Services
{
    public class ServicioParametro : IServicioParametro
    {

        private readonly HttpClient _httpClient;

        public ServicioParametro(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
        }

        public async Task<string> ObtenerValor(string nombre)
        {
            var response = await _httpClient.GetAsync("/api/Parametros");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var parametros = JsonConvert.DeserializeObject<IEnumerable<Parametros>>(content);
                var parametro = parametros!.FirstOrDefault(p => p.Parametro == nombre);
                var valor = parametro!.Valor;
                return valor!;
            }
            return "Parametro no encontrado";
        }
    }
}
