﻿using FluentAssertions;
using TaskoMask.Services.Boards.Read.Api.Features.Cards.GetCardById;
using TaskoMask.Services.Boards.Read.IntegrationTests.Fixtures;
using TaskoMask.Services.Boards.Read.IntegrationTests.TestData;
using Xunit;

namespace TaskoMask.Services.Boards.Read.IntegrationTests.Features.Card
{
    [Collection(nameof(CardCollectionFixture))]
    public class GetCardByIdTests
    {

        #region Fields

        private readonly CardCollectionFixture _fixture;

        #endregion

        #region Ctor

        public GetCardByIdTests(CardCollectionFixture fixture)
        {
            _fixture = fixture;
        }

        #endregion

        #region Test Methods


        [Fact]
        public async Task Card_Is_Fetched_By_Id()
        {
            //Arrange
            var expectedCard = CardObjectMother.GetCard();
            await _fixture.SeedCardAsync(expectedCard);
            var getCardByIdHandler = new GetCardByIdHandler(_fixture.DbContext, _fixture.Mapper);
            var request = new GetCardByIdRequest(expectedCard.Id);

            //Act
            var result = await getCardByIdHandler.Handle(request, CancellationToken.None);

            //Assert
            result.Id.Should().Be(expectedCard.Id);
            result.Name.Should().Be(expectedCard.Name);
        }


        #endregion
    }
}