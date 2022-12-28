﻿using FluentAssertions;
using TaskoMask.Services.Boards.Read.Api.Features.Boards.GetBoardById;
using TaskoMask.Services.Boards.Read.IntegrationTests.Fixtures;
using TaskoMask.Services.Boards.Read.IntegrationTests.TestData;
using Xunit;

namespace TaskoMask.Services.Boards.Read.IntegrationTests.Features.Boards
{
    [Collection(nameof(BoardCollectionFixture))]
    public class GetBoardByIdTests
    {

        #region Fields

        private readonly BoardCollectionFixture _fixture;

        #endregion

        #region Ctor

        public GetBoardByIdTests(BoardCollectionFixture fixture)
        {
            _fixture = fixture;
        }

        #endregion

        #region Test Methods


        [Fact]
        public async Task Board_Is_Fetched_By_Id()
        {
            //Arrange
            var expectedBoard = BoardObjectMother.GetBoard();
            await _fixture.SeedBoardAsync(expectedBoard);
            var getBoardByIdHandler = new GetBoardByIdHandler(_fixture.DbContext, _fixture.Mapper);
            var request = new GetBoardByIdRequest(expectedBoard.Id);

            //Act
            var result = await getBoardByIdHandler.Handle(request, CancellationToken.None);

            //Assert
            result.Id.Should().Be(expectedBoard.Id);
            result.Name.Should().Be(expectedBoard.Name);
        }


        #endregion
    }
}