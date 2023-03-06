//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
    }
}
