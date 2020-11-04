using FlatFilesConverter.Business.Services;
using FlatFilesConverter.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace FlatFilesConverter.Business.Tests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserDAO> _mockUserDAO;
        private readonly UserService _sut;
        private readonly User _user;

        public UserServiceTest()
        {
            _mockUserDAO = new Mock<IUserDAO>();
            _sut = new UserService(_mockUserDAO.Object);
            _user = new User { Username = "test", Password = "test", UserID = 2 };
        }

        [Fact]
        public void AuthenticateUser_CallUserDAO_WithCorrectUser()
        {
            _mockUserDAO.Setup(userDAO => userDAO.AuthenticateUser(_user)).Returns(_user.UserID);
            Assert.Equal(_sut.AuthenticateUser(_user), _user.UserID);
            _mockUserDAO.Verify(userDAO => userDAO.AuthenticateUser(_user), Times.Once);
        }

        [Fact]
        public void RegisterUser_CallUserDAO_WithCorrectUser()
        {
            _mockUserDAO.Setup(userDAO => userDAO.RegisterUser(_user)).Returns(true);
            Assert.True(_sut.RegisterUser(_user));
            _mockUserDAO.Verify(userDAO => userDAO.RegisterUser(_user), Times.Once);
        }

        [Fact]
        public void GetUser_CallUserDAO_WithCorrectUserName()
        {
            _mockUserDAO.Setup(userDAO => userDAO.GetUser(_user.Username)).Returns(_user);
            Assert.Equal(_sut.GetUser(_user.Username), _user);
            _mockUserDAO.Verify(userDAO => userDAO.GetUser(_user.Username), Times.Once);
        }
    }
}
