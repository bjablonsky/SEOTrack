using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SEOTrack.Web.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.AspNetCore.WebUtilities;

namespace SEOTrack.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private string _apiUrl;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _apiUrl = _config.GetValue<string>("ConnectionStrings:API");
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult RankingResults()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RankingResults([FromBody] RankingResultsRequest postRequest)
        {
            try
            {
                List<RankingResultViewModel> searchResults = null;

                if(string.IsNullOrEmpty(postRequest.keywords) || string.IsNullOrEmpty(postRequest.url))
                {
                    return BadRequest();
                }

                using (var client = new HttpClient())
                {
                    var param = new Dictionary<string, string>() { { "keywords", postRequest.keywords }, { "url", postRequest.url } };
                    var apiUrl = new Uri(QueryHelpers.AddQueryString(_apiUrl + "GoogleSearchResults", param));
                    var response = client.GetAsync(apiUrl);
                    response.Wait();

                    var responseResult = response.Result;
                    if (responseResult.IsSuccessStatusCode)
                    {
                        var json = responseResult.Content.ReadAsStringAsync();
                        json.Wait();

                        if (string.IsNullOrEmpty(json.Result))
                        {
                            return new NoContentResult();
                        }
                        searchResults = JsonSerializer.Deserialize<List<RankingResultViewModel>>(json.Result);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return Ok(searchResults);
            }
            catch 
            {
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class RankingResultsRequest
        {
            public string keywords { get; set; }
            public string url { get; set; }
        }
    }
}
