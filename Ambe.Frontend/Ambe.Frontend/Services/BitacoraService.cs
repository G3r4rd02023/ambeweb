using Ambe.Frontend.Models.Entidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Net.Http;
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

       
    }
}
