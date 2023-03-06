//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Owners;
using Sheenam.Api.Models.Foundations.Owners.Exceptions;
using Sheenam.Api.Services.Foundations.Owners;

namespace Sheenam.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : RESTFulController
    {
        private readonly IOwnerService ownerService;

        public OwnersController(IOwnerService ownerService) =>
            this.ownerService = ownerService;

        [HttpPost]
        public async ValueTask<ActionResult<Owner>> PostOwnerAsync(Owner owner)
        {
            try
            {
                Owner addedOwner =
                    await this.ownerService.AddOwnerAsync(owner);

                return Created(addedOwner);
            }
            catch (OwnerValidationException ownerValidationException)
            {
                return BadRequest(ownerValidationException.InnerException);
            }
            catch (OwnerDependencyValidationException ownerDependencyValidationException)
               when (ownerDependencyValidationException.InnerException is AlreadyExistsOwnerException)
            {
                return Conflict(ownerDependencyValidationException.InnerException);
            }
            catch (OwnerDependencyException ownerDependencyException)
            {
                return InternalServerError(ownerDependencyException);
            }
            catch (OwnerServiceException ownerServiceException)
            {
                return InternalServerError(ownerServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Owner>> GetAllOwners()
        {
            try
            {
                IQueryable<Owner> retrievedOwners =
                    this.ownerService.RetrieveAllOwners();

                return Ok(retrievedOwners);
            }
            catch (OwnerDependencyException ownerDependencyException)
            {
                return InternalServerError(ownerDependencyException);
            }
            catch (OwnerServiceException ownerServiceException)
            {
                return InternalServerError(ownerServiceException);
            }
        }

        [HttpGet("{ownerId}")]
        public async ValueTask<ActionResult<Owner>> GetOwnerByIdAsync(Guid ownerId)
        {
            try
            {
                Owner owner = await this.ownerService.RetrieveOwnerByIdAsync(ownerId);

                return Ok(owner);
            }
            catch (OwnerValidationException ownerValidationException)
                when (ownerValidationException.InnerException is NotFoundOwnerException)
            {
                return NotFound(ownerValidationException.InnerException);
            }
            catch (OwnerValidationException ownerValidationException)
            {
                return BadRequest(ownerValidationException.InnerException);
            }
            catch (OwnerDependencyException ownerDependencyException)
            {
                return InternalServerError(ownerDependencyException);
            }
            catch (OwnerServiceException ownerServiceException)
            {
                return InternalServerError(ownerServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Owner>> PutOwnerAsync(Owner owner)
        {
            try
            {
                Owner modifiedOwner =
                    await this.ownerService.ModifyOwnerAsync(owner);

                return Ok(modifiedOwner);
            }
            catch (OwnerValidationException ownerValidationException)
                when (ownerValidationException.InnerException is NotFoundOwnerException)
            {
                return NotFound(ownerValidationException.InnerException);
            }
            catch (OwnerValidationException ownerValidationException)
            {
                return BadRequest(ownerValidationException.InnerException);
            }
            catch (OwnerDependencyValidationException ownerDependencyValidationException)
                when (ownerDependencyValidationException.InnerException is AlreadyExistsOwnerException)
            {
                return Conflict(ownerDependencyValidationException.InnerException);
            }
            catch (OwnerDependencyException ownerDependencyException)
            {
                return InternalServerError(ownerDependencyException);
            }
            catch (OwnerServiceException ownerServiceException)
            {
                return InternalServerError(ownerServiceException);
            }
        }

        [HttpDelete("{ownerId}")]
        public async ValueTask<ActionResult<Owner>> DeleteOwnerByIdAsync(Guid ownerId)
        {
            try
            {
                Owner deletedOwner =
                    await this.ownerService.RemoveOwnerByIdAsync(ownerId);

                return Ok(deletedOwner);
            }
            catch (OwnerValidationException ownerValidationException)
                when (ownerValidationException.InnerException is NotFoundOwnerException)
            {
                return NotFound(ownerValidationException.InnerException);
            }
            catch (OwnerValidationException ownerValidationException)
            {
                return BadRequest(ownerValidationException.InnerException);
            }
            catch (OwnerDependencyValidationException ownerDependencyValidationException)
                when (ownerDependencyValidationException.InnerException is LockedOwnerException)
            {
                return Locked(ownerDependencyValidationException.InnerException);
            }
            catch (OwnerDependencyValidationException ownerDependencyValidationException)
            {
                return BadRequest(ownerDependencyValidationException);
            }
            catch (OwnerDependencyException ownerDependencyException)
            {
                return InternalServerError(ownerDependencyException);
            }
            catch (OwnerServiceException ownerServiceException)
            {
                return InternalServerError(ownerServiceException);
            }
        }
    }
}
