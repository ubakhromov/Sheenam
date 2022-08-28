//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use Comfort and Peace
// ==================================================

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : RESTFulController
    {
        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Todalashib kelish"); 
    }
}
