﻿using System.Web.Mvc;
using Logs.Authentication.Contracts;
using Logs.Services.Contracts;
using Logs.Web.Controllers;
using Logs.Web.Models.Entries;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace Logs.Web.Tests.Controllers.CommentControllerTests
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void TestComment_ModelStateIsNotValid_ShouldNotCallAuthenticationProviderCurrentUserId()
        {
            // Arrange
            var model = new NewCommentViewModel();

            var mockedService = new Mock<ICommentService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new CommentController(mockedService.Object, mockedAuthenticationProvider.Object);
            controller.ModelState.AddModelError("", "");

            // Act
            controller.Comment(model);

            // Assert
            mockedAuthenticationProvider.Verify(p => p.CurrentUserId, Times.Never);
        }

        [Test]
        public void TestComment_ShouldCallAuthenticationProviderCurrentUserId()
        {
            // Arrange
            var model = new NewCommentViewModel();

            var mockedService = new Mock<ICommentService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new CommentController(mockedService.Object, mockedAuthenticationProvider.Object);

            // Act
            controller.Comment(model);

            // Assert
            mockedAuthenticationProvider.Verify(p => p.CurrentUserId, Times.Once);
        }

        [TestCase(1, "content", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "much content", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestComment_ShouldCallEntryServiceAddEntryToLogCorrectly(int logId, string content, string userId)
        {
            // Arrange
            var model = new NewCommentViewModel
            {
                LogId = logId,
                Content = content
            };

            var mockedService = new Mock<ICommentService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.CurrentUserId).Returns(userId);

            var controller = new CommentController(mockedService.Object, mockedAuthenticationProvider.Object);

            // Act
            controller.Comment(model);

            // Assert
            mockedService.Verify(s => s.AddCommentToLog(content, logId, userId), Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void TestComment_ShouldRedirectCorrectly(int logId)
        {
            // Arrange
            var model = new NewCommentViewModel { LogId = logId };

            var mockedService = new Mock<ICommentService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new CommentController(mockedService.Object, mockedAuthenticationProvider.Object);

            // Act, Assert
            controller
                .WithCallTo(c => c.Comment(model))
                .ShouldRedirectTo((LogsController c) => c.Details(logId, It.IsAny<int>(), It.IsAny<int>()));
        }
    }
}
