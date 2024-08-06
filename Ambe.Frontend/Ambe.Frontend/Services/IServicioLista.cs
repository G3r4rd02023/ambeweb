using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ambe.Frontend.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaRoles();

        Task<IEnumerable<SelectListItem>> GetListaMarcas();

        Task<IEnumerable<SelectListItem>> GetListaModelos();

        Task<IEnumerable<SelectListItem>> GetListaConductores();

        Task<IEnumerable<SelectListItem>> GetListaNineras();

        Task<IEnumerable<SelectListItem>> GetListaUnidades();

        Task<IEnumerable<SelectListItem>> GetListaTiposViaje();

        IEnumerable<SelectListItem> GetListaEstados();

    }
}
