﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Extensions.Hosting;
using Moq;
using Sheenam.Api.Models.Foundations.Owner;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldModifyOwnerAsync()
        {
            // given
            DateTimeOffset randomDate =
                GetRandomDateTimeOffset();

            Owner randomOwner =
                CreateRandomOwner(randomDate);

            Owner inputOwner =
                randomOwner;

            inputOwner.UpdatedDate =
                randomDate.AddMinutes(1);
            Owner storageOwner =
                inputOwner;
            Owner updatedOwner =
                inputOwner;

            Owner expectedOwner =
                updatedOwner.DeepClone();

            Guid inputOwnerId =
                inputOwner.Id;

          

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(
                    inputOwnerId))
                        .ReturnsAsync(storageOwner);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOwnerAsync(
                    inputOwner))
                        .ReturnsAsync(updatedOwner);

            // when
            Owner actualOwner =
                await this.ownerService.
                    ModifyOwnerAsync(inputOwner);

            // then
            actualOwner.Should().BeEquivalentTo(
                expectedOwner);

       
            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(inputOwnerId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOwnerAsync(inputOwner),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
