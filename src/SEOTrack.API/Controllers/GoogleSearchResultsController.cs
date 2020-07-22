using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SEOTrack.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SEOTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleSearchResultsController : Controller
    {
        private readonly ILogger<GoogleSearchResultsController> _logger;
        private readonly ISearchResultsService _google;

        public GoogleSearchResultsController(ILogger<GoogleSearchResultsController> logger, ISearchResultsService searchResultsService)
        {
            _logger = logger;
            _google = searchResultsService;
        }

        // GET: api/<GoogleSearchResults>
        [HttpGet]
        public IActionResult Get(string keywords, string url)
        {
            var results = _google.FindRankByKeywords(keywords, url);

            if(results == null || !results.Any())
            {
                return new NoContentResult();
            }

            return Json(results);
        }
    }
}
