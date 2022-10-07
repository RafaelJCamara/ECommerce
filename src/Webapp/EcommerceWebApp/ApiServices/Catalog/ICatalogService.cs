using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebApp.ApiServices.Catalog
{
    public interface ICatalogService
    {
        Task<IEnumerable<Models.Catalog>> GetCatalogProducts();
    }
}
