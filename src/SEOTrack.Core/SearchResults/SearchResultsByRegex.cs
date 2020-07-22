using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEOTrack.Core
{
    public class SearchResultsByRegex : ISearchResults
    {
        private List<SearchResult> _results;
        private Regex _resultsRX;
        private Regex _linkRX;

        public SearchResultsByRegex()
        {
            _results = new List<SearchResult>();
        }

        public async Task<string> GetSearchResultsResponse(string url)
        {
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url);
                var contents = await response.Content.ReadAsStringAsync();

                return contents;
            }
            catch { }

            return null;
        }

        public void ParseSearchResults(string response, string resultGroupPattern, string linkPattern)
        {
            try
            {
                _results.Clear();

                _resultsRX = new Regex(resultGroupPattern);
                MatchCollection results = _resultsRX.Matches(response);

                _linkRX = new Regex(linkPattern);

                int rank = 1;

                foreach (Match result in results)
                {
                    var resultVal = result.Groups[0].Value;
                    var linkVal = _linkRX.Match(resultVal);

                    _results.Add(new SearchResult(rank, linkVal.Groups[1].Value));
                    rank++;
                }
            }
            catch { }
        }

        public IEnumerable<SearchResult> GetSearchResults()
        {
            return _results;
        }

        public void PrintSearchResults()
        {
            foreach (SearchResult result in _results)
            {
                Console.WriteLine("#{0}: {1}", result.RankNum, result.URL);
            }
        }
    }
}
