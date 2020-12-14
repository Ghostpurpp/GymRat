using GymRat.Models;
using GymRat.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace GymRat.Test
{
    public class SessionTests
    {
        private readonly Mock<SessionService> sessionService = new Mock<SessionService>();

        [Fact]
        public async void GetSessions()
        {
            List<Session> actual = await sessionService.Object.GetSessionsAsync("ThisIsMadeUp");

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async void AddSessions_ShouldWork()
        {
            //Arrange
            var newSession = new Session
            {
                SessionId = Guid.NewGuid(),
                UserId = "4739849739845"
            };

            //Act
            await sessionService.Object.AddSessionAsync(newSession);
            List<Session> sessions = await sessionService.Object.GetSessionsAsync("ThisIsMadeUp");

            //Assert
            Assert.Contains(newSession, sessions);
        }

        [Fact]
        public async void AddSessions_ShouldNotWork()
        {
            //Arrange
            Session newSession = null;

            //Act
            await sessionService.Object.AddSessionAsync(newSession);
            List<Session> sessions = await sessionService.Object.GetSessionsAsync("ThisIsMadeUp");

            //Assert
            Assert.DoesNotContain(newSession, sessions);
        }
    }
}
