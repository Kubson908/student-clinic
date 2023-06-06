using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NuGet.Protocol;
using Przychodnia.Shared;
using Przychodnia.Webapi.Controllers;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;

namespace Przychodnia.Tests.Unit
{
    public class AppointmentControllerTest
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "ClinicDbTest").Options;

        ApplicationDbContext context;
        AppointmentController controller;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();
            SeedDatabase();
            var patientManager = new Mock<UserManager<Patient>>(
                new Mock<IUserStore<Patient>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Patient>>().Object,
                    new IUserValidator<Patient>[0],
                    new IPasswordValidator<Patient>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<Patient>>>().Object);
            patientManager.Setup(userManager => userManager.CreateAsync(It.IsAny<Patient>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            patientManager.Setup(userManager => userManager.AddToRoleAsync(It.IsAny<Patient>(), It.IsAny<string>()));
            controller = new AppointmentController(context, patientManager.Object);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {
            context.Patients.Add(new Patient()
            {
                Id = "aac0fdb0-8245-48f5-9488-816f2145571d",
                FirstName = "Jan",
                LastName = "Testowy",
                DateOfBirth = "1990-10-10",
                Pesel = "90101027259",
            });
            context.Patients.Add(new Patient()
            {
                Id = "555552d9-4a3a-4ac6-8a71-7bbbbccd9be9",
                FirstName = "Anna",
                LastName = "Testowa",
                DateOfBirth = "1995-03-03",
                Pesel = "95030334164",
            });
            context.Appointments.Add(new Appointment()
            {
                Id = 1,
                Date = DateTime.Parse("2023-06-16 11:30:00.0000000"),
                Specialization = (Specialization)0,
                Symptoms = "Ból brzucha",
                Medicines = "Apap",
                PatientId = "555552d9-4a3a-4ac6-8a71-7bbbbccd9be9"
            });
            context.Appointments.Add(new Appointment()
            {
                Id = 2,
                Date = DateTime.Parse("2023-06-15 12:30:00.0000000"),
                Specialization = (Specialization)2,
                Symptoms = "Ból oka",
                Medicines = "Apap",
                PatientId = "aac0fdb0-8245-48f5-9488-816f2145571d"
            });
            context.SaveChanges();
        }


        [Test]
        public void GetAppointmentById_ReturnOk()
        {

            IActionResult actionResult = controller.GetById(1).Result;
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetAppointmentById_ReturnNotFound()
        {
            IActionResult actionResult = controller.GetById(7).Result;
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }
    }
}