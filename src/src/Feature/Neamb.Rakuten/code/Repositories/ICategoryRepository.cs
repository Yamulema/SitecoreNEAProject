using System.Collections.Generic;
using Neambc.Neamb.Feature.Rakuten.Model;

namespace Neambc.Neamb.Feature.Rakuten.Repositories
{
    public interface ICategoryRepository {
        List<Category> GetCategories();
    }
}
