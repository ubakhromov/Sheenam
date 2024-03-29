﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Owners;
using Sheenam.Api.Models.Foundations.Owners.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public partial class OwnerService
    {
        private delegate ValueTask<Owner> ReturningOwnerFunction();
        private delegate IQueryable<Owner> ReturningOwnersFunction();

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
            catch (NotFoundOwnerException notFoundOwnerException)
            {
                throw CreateAndLogValidationException(notFoundOwnerException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistOwnerException =
                    new AlreadyExistsOwnerException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistOwnerException);
            }
            catch (DbUpdateConcurrencyException databaseUpdateConcurrencyException)
            {
                var lockedOwnerException =
                    new LockedOwnerException(databaseUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedOwnerException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedOwnerStorageException =
                    new FailedOwnerStorageException(databaseUpdateException);

                throw CreateAndLogDependecyException(failedOwnerStorageException);
            }
            catch (Exception exception)
            {
                var failedOwnerServiceException =
                    new FailedOwnerServiceException(exception);

                throw CreateAndLogServiceException(failedOwnerServiceException);
            }
        }

        private IQueryable<Owner> TryCatch(ReturningOwnersFunction returningOwnersFunction)
        {
            try
            {
                return returningOwnersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedOwnerStorageException =
                     new FailedOwnerStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedOwnerStorageException);
            }
            catch (Exception exception)
            {
                var failedOwnerServiceException =
                    new FailedOwnerServiceException(exception);

                throw CreateAndLogServiceException(failedOwnerServiceException);
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

        private OwnerDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ownerDependencyValidationException =
                new OwnerDependencyValidationException(exception);

            this.loggingBroker.LogError(ownerDependencyValidationException);

            return ownerDependencyValidationException;
        }

        private OwnerDependencyException CreateAndLogDependecyException(Xeption exception)
        {
            var ownerDependencyException = new OwnerDependencyException(exception);
            this.loggingBroker.LogError(ownerDependencyException);

            return ownerDependencyException;
        }

        private OwnerServiceException CreateAndLogServiceException(Xeption exception)
        {
            var ownerServiceException = new OwnerServiceException(exception);
            this.loggingBroker.LogError(ownerServiceException);

            return ownerServiceException;
        }
    }
}
