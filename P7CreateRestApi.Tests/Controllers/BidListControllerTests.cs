using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests.Controllers
{
    public class BidListControllerTests
    {
        private readonly Mock<IBidListRepository> _mockRepo;
        private readonly BidListController _controller;

        public BidListControllerTests()
        {
            _mockRepo = new Mock<IBidListRepository>();
            _controller = new BidListController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            // Arrange
            var expectedBidList = new List<BidList> { new(), new() };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedBidList);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedBidList, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            // Arrange
            var bidList = new BidList { BidListId = 1 };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(bidList);

            // Act
            var result = await _controller.GetByIdAsync(1);

            // Assert
            Assert.IsType<ActionResult<BidList>>(result);
            Assert.Equal(bidList, result.Value);
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var bidList = new BidList
            {
                Account = "TestAccount",
                BidType = "TestType",
                BidQuantity = 100,
                Ask = 11.5,
                BidListDate = DateTime.Now
            };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<BidList>())).Returns(Task.FromResult(bidList));

            // Act
            var result = await _controller.CreateAsync(bidList);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(bidList, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var bidListDto = new BidListDto { BidListId = 1, Account = "UpdatedAccount" };
            var originalBidList = new BidList { BidListId = 1, Account = "OriginalAccount" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(originalBidList);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<BidList>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAsync(1, bidListDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Contains(_mockRepo.Invocations, i => i.Method.Name == "UpdateAsync" &&
                ((BidList)i.Arguments[0]).Account == "UpdatedAccount");
        }


        [Fact]
        public async Task DeleteAsync()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAsync(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
