//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Accommodations
{
    public partial class AccommodationService
    {
        private delegate ValueTask<Accommodation> ReturningAccommodationFuntion();

        private async ValueTask<Accommodation> TryCatch(ReturningAccommodationFuntion returningAccommodationFuntion)
        {
            try
            {
                return await returningAccommodationFuntion();
            }
            catch (NullAccommodationException nullAccommodationException)
            {
                throw CreateAndLogValidationException(nullAccommodationException);
            }
            catch (InvalidAccommodationException invalidAccommodationException)
            {
                throw CreateAndLogValidationException(invalidAccommodationException);
            }
            catch (SqlException sqlException)
            {
                var failedAccommodationStorageException =
                    new FailedAccommodationStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAccommodationStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistAccommodationException =
                    new AlreadyExistAccommodationException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistAccommodationException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAccommodationStorageException =
                    new FailedAccommodationStorageException(databaseUpdateException);

                throw CreateAndLogDependecyException(failedAccommodationStorageException);
            }
            catch (Exception exception)
            {
                var failedAccommodationServiceException =
                    new FailedAccommodationServiceException(exception);

                throw CreateAndLogServiceException(failedAccommodationServiceException);
            }
        }

        private AccommodationValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var accommodationValidationException =
                new AccommodationValidationException(exception);

            this.loggingBroker.LogError(accommodationValidationException);

            return accommodationValidationException;
        }

        private AccommodationDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var accommodationDependencyException =
                new AccommodationDependencyException(exception);

            this.loggingBroker.LogCritical(accommodationDependencyException);

            return accommodationDependencyException;
        }

        private AccommodationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var accommodationDependencyValidationException =
                new AccommodationDependencyValidationException(exception);

            this.loggingBroker.LogError(accommodationDependencyValidationException);

            return accommodationDependencyValidationException;
        }

        private AccommodationDependencyException CreateAndLogDependecyException(Xeption exception)
        {
            var accommodationDependencyException = new AccommodationDependencyException(exception);
            this.loggingBroker.LogError(accommodationDependencyException);

            return accommodationDependencyException;
        }

        private AccommodationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var accommodationServiceException = new AccommodationServiceException(exception);
            this.loggingBroker.LogError(accommodationServiceException);

            return accommodationServiceException;
        }
    }
}
