using Ambe.Frontend.Models.Entidades;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;

namespace Ambe.Frontend.Services
{
    public class ServicioPersonas : IServicioPersonas
    {
        private readonly HttpClient _httpClient;
        public ServicioPersonas(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://ambetest.somee.com");
        }
        public async Task<IEnumerable<Personas>> GetPersonasAsync()
        {
            var response = await _httpClient.GetAsync("/api/Personas");
            var content = await response.Content.ReadAsStringAsync();
            var personas = JsonConvert.DeserializeObject<IEnumerable<Personas>>(content);
            return personas!;
        }

        public async Task<IEnumerable<TipoPersonas>> GetTiposPersonaAsync()
        {
            var response = await _httpClient.GetAsync("/api/TipoPersonas");
            var content = await response.Content.ReadAsStringAsync();
            var tiposPersonas = JsonConvert.DeserializeObject<IEnumerable<TipoPersonas>>(content);
            return tiposPersonas!;
        }

        public async Task<IEnumerable<Contactos>> GetContactosAsync(int idPersona)
        {
            var response = await _httpClient.GetAsync($"api/contactos/{idPersona}");
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Intentar deserializar como array primero
                try
                {
                    var contactosArray = JsonConvert.DeserializeObject<IEnumerable<Contactos>>(responseString);
                    if (contactosArray != null)
                    {
                        return contactosArray;
                    }
                }
                catch (JsonSerializationException) { }

                // Si falla, intentar deserializar como objeto único
                try
                {
                    var contactoObjeto = JsonConvert.DeserializeObject<Contactos>(responseString);
                    if (contactoObjeto != null)
                    {
                        return new List<Contactos> { contactoObjeto };
                    }
                }
                catch (JsonSerializationException) { }
            }

            return Enumerable.Empty<Contactos>();
        }

        public async Task<IEnumerable<Modelos>> GetModelosAsync(int idMarca)
        {
            var response = await _httpClient.GetAsync("api/Modelos");
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Intentar deserializar como array primero
                try
                {
                    var modelosArray = JsonConvert.DeserializeObject<IEnumerable<Modelos>>(responseString);
                    if (modelosArray != null)
                    {
                        var modelos = modelosArray.Where(m => m.IdMarca == idMarca);
                        return modelos;
                    }
                }
                catch (JsonSerializationException) { }

                // Si falla, intentar deserializar como objeto único
                try
                {
                    var modelosObjeto = JsonConvert.DeserializeObject<Modelos>(responseString);
                    if (modelosObjeto != null)
                    {
                        return new List<Modelos> { modelosObjeto };
                    }
                }
                catch (JsonSerializationException) { }
            }

            return Enumerable.Empty<Modelos>();
        }
    }
}
