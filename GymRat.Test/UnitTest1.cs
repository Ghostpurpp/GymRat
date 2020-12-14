using GymRat.Models;
using GymRat.Services;
using System.Collections.Generic;
using Xunit;

namespace GymRat.Test
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var sessionService = new SessionService();

            //Arrange
            List<Session> expected = null;

            //Act
            List<Session> actual = await sessionService.GetSessionsAsync("ThisIsMadeUp");

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
