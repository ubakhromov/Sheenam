//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Services.Foundations.Accommodations;

namespace Sheenam.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationsController : RESTFulController
    {
        private readonly IAccommodationService accommodationService;

        public AccommodationsController(IAccommodationService accommodationService) =>
            this.accommodationService = accommodationService;

        [HttpPost]
        public async ValueTask<ActionResult<Accommodation>> PostAccommodationAsync(Accommodation accommodation)
        {
            try
            {
                Accommodation addedAccommodation =
                    await this.accommodationService.AddAccommodationAsync(accommodation);

                return Created(addedAccommodation);
            }
            catch (AccommodationValidationException accommodationValidationException)
            {
                return BadRequest(accommodationValidationException.InnerException);
            }
            catch (AccommodationDependencyValidationException accommodationDependencyValidationException)
               when (accommodationDependencyValidationException.InnerException is AlreadyExistAccommodationException)
            {
                return Conflict(accommodationDependencyValidationException.InnerException);
            }
            catch (AccommodationDependencyException accommodationDependencyException)
            {
                return InternalServerError(accommodationDependencyException);
            }
            catch (AccommodationServiceException accommodationServiceException)
            {
                return InternalServerError(accommodationServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Accommodation>> GetAllAccommodations()
        {
            try
            {
                IQueryable<Accommodation> retrievedAccommodations =
                    this.accommodationService.RetrieveAllAccommodations();

                return Ok(retrievedAccommodations);
            }
            catch (AccommodationDependencyException accommodationDependencyException)
            {
                return InternalServerError(accommodationDependencyException);
            }
            catch (AccommodationServiceException accommodationServiceException)
            {
                return InternalServerError(accommodationServiceException);
            }
        }
    }
}
