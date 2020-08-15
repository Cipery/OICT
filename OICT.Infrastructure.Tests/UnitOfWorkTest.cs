using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace OICT.Infrastructure.Tests
{
    public class UnitOfWorkTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Should_ThrowException_WhenInitializedWithNullContext()
        {
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(null));
        }

        [Test]
        public void Should_ReturnCorrectContext_WhenContextPropertyGet()
        {
            // Arrange
            var contextMock = Mock.Of<OICTApiContext>();

            // Act
            var unitOfWork = new UnitOfWork(contextMock);

            // Assert
            unitOfWork.Context.Should().BeSameAs(contextMock);
        }

        [Test]
        public async Task Should_CallSaveChangesAsyncOnContext_WhenCommitAsyncIsCalled()
        {
            // Arrange
            var contextMock = new Mock<OICTApiContext>();
            contextMock.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var unitOfWork = new UnitOfWork(contextMock.Object);
            await unitOfWork.CommitAsync();

            // Assert
            contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Should_DisposeOfContext_WhenBeingDisposed()
        {
            // Arrange
            var contextMock = new Mock<OICTApiContext>();
            contextMock.Setup(context => context.Dispose());

            // Act
            var unitOfWork = new UnitOfWork(contextMock.Object);
            unitOfWork.Dispose();

            // Assert
            contextMock.Verify(context => context.Dispose(), Times.Once);
        }
    }
}