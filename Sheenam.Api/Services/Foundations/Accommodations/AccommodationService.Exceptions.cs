//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
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
            var AccommodationDependencyValidationException =
                new AccommodationDependencyValidationException(exception);

            this.loggingBroker.LogError(AccommodationDependencyValidationException);

            return AccommodationDependencyValidationException;
        }
    }
}
