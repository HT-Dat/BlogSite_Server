using System;
using BLL.Services;
using BLL.Utilities;
using Xunit;

namespace BLL.UnitTests;

public class TimeHelperTests
{
    [Fact]
    public void GetCurrentTimeAsNumericDate_ReturnsExpectedNumericDate()
    {
        // Arrange
        var currentTime = new DateTime(2023, 4, 11);
        var expectedNumericDate = "1681171200";
        var mockSystemClock = new MockSystemClock { UtcNow = currentTime };
        var timeHelper = new TimeHelper(mockSystemClock);
        
        // Act
        var actualNumericDate = timeHelper.CurrentNumericDate();
        
        // Assert
        Assert.Equal(expectedNumericDate, actualNumericDate);
    }
}