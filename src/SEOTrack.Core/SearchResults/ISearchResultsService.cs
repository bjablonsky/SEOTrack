using System;
using System.Collections.Generic;
using System.Text;

namespace SEOTrack.Core
{
    public interface ISearchResultsService
    {
        private const int MaxNumSearchResults = 100;
        public List<SearchResult> FindRankByKeywords(string keywords, string siteURL, int numberOfResults = MaxNumSearchResults);
    }
}
