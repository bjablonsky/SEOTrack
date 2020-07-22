namespace SEOTrack.Core
{
    public class SearchResult
    {
        public int RankNum { get; private set; }
        public string URL { get; private set; }

        public SearchResult(int rankNum, string url)
        {
            RankNum = rankNum;
            URL = url;
        }
    }
}
