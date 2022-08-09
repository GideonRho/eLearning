using System.Net;
using NUnit.Framework;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Responses;

namespace WebAPI.Tests
{
    public class UserControllerTest : AbstractTest
    {

        [Test]
        public void General()
        {
            Setup();
            
            Assert.True(HasUserRights());
            client.Logout();
            
            Register();
            Authenticate();
            Logout();
            
            Finish();
        }

        private UserApi Authenticate()
        {
            
            Assert.False(HasUserRights());
            
            var result = client.Authenticate(new Authenticate
            {
                Username = "Test",
                Password = "123"
            });

            Assert.True(result.Id > 0);
            Assert.AreEqual("Test", result.Name);
            Assert.True(HasUserRights());
            
            return result;
        }

        private void Register()
        {
            client.Register(new CreateUser
            {
                Username = "Test",
                Password = "123"
            });
        }

        private void Logout()
        {
            client.Logout();
            Assert.False(HasUserRights());
        }

        private bool HasUserRights()
        {
            var result = client.Get("/course/active");

            return result.StatusCode == HttpStatusCode.OK;
        }
        
    }
}