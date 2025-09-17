//using koninkrijk.Server.Controllers;
//using koninkrijk.Server.Data;
//using koninkrijk.Server.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Xunit;

//namespace koninkrijk.Tests
//{
//    public class ActionsControllerTests
//    {
//        private Mock<DataContext> _mockContext;
//        private Mock<DbSet<Player>> _mockPlayerSet;
//        private Mock<DbSet<Province>> _mockProvinceSet;

//        public ActionsControllerTests()
//        {
//            _mockContext = new Mock<DataContext>();
//            _mockPlayerSet = new Mock<DbSet<Player>>();
//            _mockProvinceSet = new Mock<DbSet<Province>>();
//        }

//        private ActionsController CreateControllerWithUser(Player player)
//        {
//            var claims = new List<Claim> { new Claim(ClaimTypes.Name, player.ApiKey) };
//            var identity = new ClaimsIdentity(claims, "TestAuth");
//            var claimsPrincipal = new ClaimsPrincipal(identity);

//            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
//            var controllerContext = new ControllerContext { HttpContext = httpContext };

//            var controller = new ActionsController(_mockContext.Object)
//            {
//                ControllerContext = controllerContext
//            };

//            return controller;
//        }

//        [Fact]
//        public async Task Capture_ReturnsUnauthorized_WhenPlayerIsNull()
//        {
//            // Arrange
//            var controller = new ActionsController(_mockContext.Object);

//            // Act
//            var result = await controller.Capture(1, "guess");

//            // Assert
//            Assert.IsType<UnauthorizedResult>(result);
//        }

//        [Fact]
//        public async Task Capture_ReturnsNotFound_WhenProvinceIsNull()
//        {
//            // Arrange
//            var player = new Player("testPlayer") { ApiKey = "test" };
//            _mockContext.Setup(m => m.Players).ReturnsDbSet(new List<Player> { player });
//            _mockContext.Setup(m => m.Provinces.FindAsync(It.IsAny<int>())).ReturnsAsync((Province)null);

//            var controller = CreateControllerWithUser(player);

//            // Act
//            var result = await controller.Capture(1, "guess");

//            // Assert
//            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//            Assert.Equal("Provincie niet gevonden", (notFoundResult.Value as dynamic).message);
//        }

//        [Fact]
//        public async Task Capture_ReturnsConflict_WhenProvinceOwnedByPlayer()
//        {
//            // Arrange
//            var player = new Player("testPlayer") { ApiKey = "test" };
//            var province = new Province { Player = player };

//            _mockContext.Setup(m => m.Players).ReturnsDbSet(new List<Player> { player });
//            _mockContext.Setup(m => m.Provinces.FindAsync(It.IsAny<int>())).ReturnsAsync(province);

//            var controller = CreateControllerWithUser(player);

//            // Act
//            var result = await controller.Capture(1, "guess");

//            // Assert
//            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
//            Assert.Equal("Provincie is al door jou in bezit.", (conflictResult.Value as dynamic).message);
//        }

//        [Fact]
//        public async Task Capture_ReturnsConflict_WhenGuessIsInvalid()
//        {
//            // Arrange
//            var player = new Player("testPlayer") { ApiKey = "test" };
//            var province = new Province { Player = new Player("anotherPlayer") };

//            _mockContext.Setup(m => m.Players).ReturnsDbSet(new List<Player> { player });
//            _mockContext.Setup(m => m.Provinces.FindAsync(It.IsAny<int>())).ReturnsAsync(province);

//            var controller = CreateControllerWithUser(player);

//            // Act
//            var result = await controller.Capture(1, "invalid");

//            // Assert
//            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
//            Assert.Equal("Dit woord komt niet voor in het woordenboek.", (conflictResult.Value as dynamic).message);
//        }

//        [Fact]
//        public async Task Capture_ReturnsOk_WhenGuessIsCorrect()
//        {
//            // Arrange
//            var player = new Player("testPlayer") { ApiKey = "test" };
//            var province = new Province { Player = new Player("anotherPlayer") };

//            _mockContext.Setup(m => m.Players).ReturnsDbSet(new List<Player> { player });
//            _mockContext.Setup(m => m.Provinces.FindAsync(It.IsAny<int>())).ReturnsAsync(province);

//            var controller = CreateControllerWithUser(player);

//            // Act
//            var result = await controller.Capture(1, "correct");

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal("Provincie succesvol veroverd door testPlayer", (okResult.Value as dynamic).message);
//        }

//        // Additional tests can be added here
//    }
//}
