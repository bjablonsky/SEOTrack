using System;
using System.Collections.Generic;
using System.Linq;

namespace SEOTrack.Core
{
    public class GoogleSearchResultsService : ISearchResultsService
    {
        private string _resultsPattern = "<div><div class=\\\"ZINbbc xpd O9g5cc uUPGi\\\"><div class=\\\"kCrYT\\\"><a href=\\\"[^(\\\">)]*\\\"><h3 class=\\\"zBAuLc\\\"><div class=\\\"BNeawe vvjwJb AP7Wnd\\\">[^<]*<\\/div><\\/h3><div class=\\\"BNeawe UPmit AP7Wnd\\\">[^<]*<\\/div><\\/a><\\/div><div class=\\\"x54gtf\\\"><\\/div><div class=\\\"kCrYT\\\">";
        private string _linkPattern = "<a href=\\\"\\/url\\?q=(.*?)&amp;sa=U&amp;ved=[^(\\\">)]*\\\">";
        private string _googleSearchUrl = "https://www.google.com/search?q=";
        private ISearchResults _googleSearchResults;
        // regex matching for url string, probably don't need
        //private string _urlPattern1 = "^(https?:\\/\\/){0,1}([a-zA-Z\\d-]+\\.){0,}(";
        //private string _urlPattern2 = ")([^:/]*).*$";

        public GoogleSearchResultsService(ISearchResults searchResultsProvider)
        {
            _googleSearchResults = searchResultsProvider;
        }

        public List<SearchResult> FindRankByKeywords(string keywords, string siteUrl, int numberOfResults)
        {
            try
            { 
                // check for empty keywords or siteurl
                if(String.IsNullOrEmpty(keywords) || String.IsNullOrEmpty(siteUrl))
                {
                    // throw exception
                    throw new ArgumentException("Parameter cannot be null");
                }

                // build strings
                string googleSearchUrl = _googleSearchUrl + keywords.Replace(' ', '+') + "&num=" + numberOfResults;

                // get search results
                var response = _googleSearchResults.GetSearchResultsResponse(googleSearchUrl);
                _googleSearchResults.ParseSearchResults(response.Result, _resultsPattern, _linkPattern);

                // find the first search result rank
                var results = _googleSearchResults.GetSearchResults().Where(r => r.URL.Contains(siteUrl)).Select(x => x);

                return results.ToList();
            }
            catch(ArgumentException ex)
            {
                throw ex;
            }
        }
    }
}
