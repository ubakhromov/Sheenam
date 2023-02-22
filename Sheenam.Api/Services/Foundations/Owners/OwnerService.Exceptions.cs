﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using System;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public partial class OwnerService
    {
        private delegate ValueTask<Owner> ReturningOwnerFunction();

        private async ValueTask<Owner> TryCatch(ReturningOwnerFunction returningOwnerFunction)
        {
            try
            {
                return await returningOwnerFunction();
            }
            catch (NullOwnerException nullOwnerException)
            {
                throw CreateAndLogValidationException(nullOwnerException);
            }
            catch (InvalidOwnerException invalidOwnerException)
            {
                throw CreateAndLogValidationException(invalidOwnerException);
            }
            catch (SqlException sqlException)
            {
                var failedOwnerStorageException =
                    new FailedOwnerStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedOwnerStorageException);
            }
        }

        private OwnerValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ownerValidationException =
                new OwnerValidationException(exception);

            this.loggingBroker.LogError(ownerValidationException);

            return ownerValidationException;
        }

        private OwnerDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ownerDependencyException =
                new OwnerDependencyException(exception);

            this.loggingBroker.LogCritical(ownerDependencyException);

            return ownerDependencyException;
        }
    }
}