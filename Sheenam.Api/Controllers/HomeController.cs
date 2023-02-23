//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RESTFulSense.Controllers;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : RESTFulController
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            logger.LogTrace("I am logging trace");
            logger.LogInformation("I am logging info");
            logger.LogWarning("I am logging warning");
            logger.LogError("I am logging error");
            logger.LogCritical("I am logging critical");

            return Ok("Here we go!"); 
        }
    }
}
