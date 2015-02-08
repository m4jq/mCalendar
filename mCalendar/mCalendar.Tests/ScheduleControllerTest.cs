using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Hosting;
using mCalendar.Controllers;
using mCalendar.Models;
using mCalendar.Models.Repositories;
using Moq;
using NUnit.Framework;

namespace mCalendar.Tests
{
    [TestFixture]
    public class ScheduleControllerTest
    {
        private Mock<IScheduleRepository> _scheduleRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _scheduleRepositoryMock = new Mock<IScheduleRepository>();
        }

        [Test]
        public void GetSchedules_Returns_AllSchedules_FromUser()
        {
            //Arrange
            IQueryable<Schedule> fakeSchedules = GetSchedules();
            _scheduleRepositoryMock.Setup(x => x.Schedules).Returns(fakeSchedules);

            var identity = new GenericIdentity("LoggedUser1");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            ScheduleController controller = new ScheduleController(_scheduleRepositoryMock.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            //Act
            var schedules = controller.GetSchedules();

            //Assert
            Assert.IsNotNull(schedules, "Result is null");
            Assert.IsInstanceOf(typeof(IEnumerable<ScheduleDto>), schedules, "Wrong Model");
            Assert.AreEqual(2, schedules.Count(), "Got wrong number of Schedules");
        }

        [Test]
        public void GetSchedules_Returns_Empty_NonExistingUser()
        {
            //Arrange
            IQueryable<Schedule> fakeSchedules = GetSchedules();
            _scheduleRepositoryMock.Setup(x => x.Schedules).Returns(fakeSchedules);

            var identity = new GenericIdentity("LoggedUser4");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            ScheduleController controller = new ScheduleController(_scheduleRepositoryMock.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            //Act
            var schedules = controller.GetSchedules();

            //Assert
            Assert.IsNotNull(schedules, "Result is null");
            Assert.IsInstanceOf(typeof(IEnumerable<ScheduleDto>), schedules, "Wrong Model");
            Assert.AreEqual(0, schedules.Count(), "Got wrong number of Schedules");
        }

        [Test]
        public void GetSchedule_Returns_One_ScheduleById()
        {
            //Arrange
            IQueryable<Schedule> fakeSchedules = GetSchedules();
            _scheduleRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(fakeSchedules.First(s => s.Id == 1));

            var identity = new GenericIdentity("LoggedUser1");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            ScheduleController controller = new ScheduleController(_scheduleRepositoryMock.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            //Act
            var schedule = controller.GetSchedule(1);

            //Assert
            Assert.IsNotNull(schedule, "Result is null");
            Assert.IsInstanceOf(typeof(ScheduleDto), schedule, "Wrong Type");
            Assert.AreEqual(1, schedule.ScheduleId, "Got wrong schedule. Wrong Id.");
            Assert.AreEqual("Test1", schedule.Title, "Got wrong schedule. Wrong Title.");
        }

        [Test]
        public void GetSchedule_Fails_ScheduleFromDifferentUser()
        {
            //Arrange
            IQueryable<Schedule> fakeSchedules = GetSchedules();
            _scheduleRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(fakeSchedules.First(s => s.Id == 3));

            var identity = new GenericIdentity("LoggedUser1");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            ScheduleController controller = new ScheduleController(_scheduleRepositoryMock.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            //Act, Assert
            var ex = Assert.Throws<HttpResponseException>(() => controller.GetSchedule(3));
            Assert.That(ex.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void GetSchedule_Fails_NullAsSchedule()
        {
            //Arrange
            _scheduleRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Schedule>(null);
            
            ScheduleController controller = new ScheduleController(_scheduleRepositoryMock.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            //Act, Assert
            var ex = Assert.Throws<HttpResponseException>(() => controller.GetSchedule(3));
            Assert.That(ex.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        private static IQueryable<Schedule> GetSchedules()
        {
            return new List<Schedule>
            {
                new Schedule{ Id = 1, Events = new List<Event>(), Title = "Test1", UserId = "LoggedUser1"},
                new Schedule{ Id = 2, Events = new List<Event>(), Title = "Test2", UserId = "LoggedUser1"},
                new Schedule{ Id = 3, Events = new List<Event>(), Title = "Test3", UserId = "LoggedUser3"},
                new Schedule{ Id = 4, Events = new List<Event>(), Title = "Test4", UserId = "LoggedUser2"},
                new Schedule{ Id = 5, Events = new List<Event>(), Title = "Test5", UserId = "LoggedUser2"},
                new Schedule{ Id = 6, Events = new List<Event>(), Title = "Test6", UserId = "LoggedUser2"}
            }.AsQueryable();
        }
    }
}
