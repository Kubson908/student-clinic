using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Przychodnia.Shared;
using Przychodnia.Webapi.Controllers;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using Przychodnia.Webapi.Services;

namespace Przychodnia.Tests.Unit
{
    public class AuthTest
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ClinicDbTest").Options;

        ApplicationDbContext context;
        AuthController authController;
        IUserService<RegisterDto, LoginDto> patientService;
        IUserService<RegisterEmployeeDto, LoginDto> employeeService;
        UserManager<Patient> patientManager;
        UserManager<Employee> employeeManager;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();
            authController = new AuthController(patientService, employeeService, patientManager, employeeManager);

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }


        [Test]
        public async void RegisterTest()
        {
            var patient = new RegisterDto()
            {
                FirstName = "Jan",
                LastName = "Testowy",
                DateOfBirth = "1990-10-10",
                Pesel = "90101027259",
                Email = "jan.testowy@mail.com",
                PhoneNumber = "555444333",
                Password = "Test1234",
                ConfirmPassword = "Test1234"
            };
            IActionResult actionResult = await authController.RegisterAsync(patient);
            NUnit.Framework.Assert.That(actionResult, Is.TypeOf<OkResult>());
        }
    }
}