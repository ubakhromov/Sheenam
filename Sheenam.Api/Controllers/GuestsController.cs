//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : RESTFulController
    {
        private readonly IGuestServices guestService;

        public GuestsController(IGuestServices guestService)
        {
            this.guestService = guestService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Guest>> PostGuestAsync(Guest guest)
        {
            try
            {
                Guest postedGuest = await this.guestService.AddGuestAsync(guest);

                return Created(postedGuest);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
            {
                return BadRequest(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Guest>> GetAllGuests()
        {
            try
            {
                IQueryable<Guest> retrievedGuests =
                    this.guestService.RetrieveAllGuests();

                return Ok(retrievedGuests);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException);
            }
        }

        [HttpGet("{guestId}")]
        public async ValueTask<ActionResult<Guest>> GetGuestByIdAsync(Guid guestId)
        {
            try
            {
                Guest guest = await this.guestService.RetrieveGuestByIdAsync(guestId);

                return Ok(guest);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Guest>> PutGuestAsync(Guest guest)
        {
            try
            {
                Guest modifiedGuest =
                    await this.guestService.ModifyGuestAsync(guest);

                return Ok(modifiedGuest);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException);
            }
        }

        [HttpDelete("{guestId}")]
        public async ValueTask<ActionResult<Guest>> DeleteGuestByIdAsync(Guid guestId)
        {
            try
            {
                Guest deletedGuest =
                    await this.guestService.RemoveGuestByIdAsync(guestId);

                return Ok(deletedGuest);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is LockedGuestException)
            {
                return Locked(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
            {
                return BadRequest(guestDependencyValidationException);
            }
            catch (GuestDependencyException GuestDependencyException)
            {
                return InternalServerError(GuestDependencyException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException);
            }
        }
    }
}