using System;
using System.Threading.Tasks;
using BLL.Services;
using BLL.Utilities;
using DAL.Entities;
using DAL.Persistence;
using DTO.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BLL.UnitTests;

public class AuthServiceTests
{
    private readonly AuthService _authServiceUnderTest;
    private readonly Mock<IBlogSiteDbContext> _blogSiteDbContextMock;
    private readonly Mock<ISystemClock> _iSystemClockMock;

    public AuthServiceTests()
    {
        _blogSiteDbContextMock = new Mock<IBlogSiteDbContext>();
        _iSystemClockMock = new Mock<ISystemClock>();
        _authServiceUnderTest = new AuthService(_blogSiteDbContextMock.Object, _iSystemClockMock.Object);
    }

    [Fact]
    public async Task RegisterUser_UserFoundInDb_UpdateAndReturnUser()
    {
        // Arrange
        var userToRegisterDto = new UserToRegisterDto
        {
            Id = "vwWzrxYIMmclf33TtwhdC5pJHcs1",
            DisplayName = "Test Name",
            PhotoUrl = "example.com",
            Email = "example@gmail.com"
        };

        var userInDb = new User
        {
            Id = userToRegisterDto.Id,
            Email = userToRegisterDto.Email,
            DisplayName = userToRegisterDto.DisplayName,
            Intro = "Test intro string",
            PhotoUrl = "https://example.com/old_photo.jpg",
            Profile = "Test profile string",
            CreatedDate = DateTime.UtcNow.AddYears(-1),
            LastLogin = DateTime.UtcNow.AddMonths(-1),
            SexId = 0,
            IsAdmin = false,
        };
        var testTime = DateTime.UtcNow;
        _blogSiteDbContextMock.Setup(x => x.Users.FindAsync(userToRegisterDto.Id)).ReturnsAsync(userInDb);
        _blogSiteDbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
        _blogSiteDbContextMock.Setup(x => x.SetModified(It.IsAny<object>)).Verifiable();
        _iSystemClockMock.Setup(x => x.UtcNow).Returns(testTime).Verifiable();
        // Act 
        var actualUser = await _authServiceUnderTest.RegisterUser(userToRegisterDto);

        // Assert 
        Assert.Equal(userInDb, actualUser);
        _blogSiteDbContextMock.Verify(x => x.SetModified(It.IsAny<object>()), Times.Once());
        _blogSiteDbContextMock.Verify(x => x.SetModified(It.IsAny<object>()), Times.Once());
        Assert.Equal(userToRegisterDto.Id, actualUser.Id);
        Assert.Equal(userToRegisterDto.PhotoUrl, actualUser.PhotoUrl);
        Assert.Equal(testTime, actualUser.LastLogin);
    }
}