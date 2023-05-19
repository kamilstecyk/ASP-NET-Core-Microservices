using Catalog2.API.Entities;
using MongoDB.Driver;

namespace Catalog2.API.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get;  }
    }
}
