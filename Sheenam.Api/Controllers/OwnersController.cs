﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
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
        public async ValueTask<ActionResult<Owner>> PostPostAsync(Owner owner)
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
    }
}
