using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEOTrack.Core
{
    public interface ISearchResults
    {
        Task<string> GetSearchResultsResponse(string siteURL);
        void ParseSearchResults(string results, string resultGroupPattern, string linkPattern);
        void PrintSearchResults();
        IEnumerable<SearchResult> GetSearchResults();
    }
}
