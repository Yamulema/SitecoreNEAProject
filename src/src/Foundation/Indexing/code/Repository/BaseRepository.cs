using Sitecore.ContentSearch;

namespace Neambc.Seiumb.Foundation.Indexing.Repository
{
    public class BaseRepository
    {
        protected ISearchIndex Index;

        public BaseRepository(string indexName)
        {
            Index = ContentSearchManager.GetIndex(indexName);
        }

        public ISearchIndex GetIndex()
        {
            return Index;
        }
    }
}