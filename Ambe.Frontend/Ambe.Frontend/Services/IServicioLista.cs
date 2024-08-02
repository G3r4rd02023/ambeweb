using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ambe.Frontend.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaRoles();

        IEnumerable<SelectListItem> GetListaEstados();
    }
}
