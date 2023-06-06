using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Przychodnia.Shared;
using Przychodnia.Webapi.Controllers;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using Przychodnia.Webapi.Services;

namespace Przychodnia.Tests.Unit
{

    public class OrderedTestAttribute : Attribute
    {
        public int Order { get; set; }

        public OrderedTestAttribute(int order)
        {
            this.Order = order;
        }
    }

    public class AuthTest
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ClinicDbTest").Options;

        ApplicationDbContext context;
        AuthController authController;
        IUserService<RegisterDto, LoginDto> patientService;
        IUserService<RegisterEmployeeDto, LoginDto> employeeService;
/*        Mock<UserManager<Patient>> patientManager = new();
        Mock<UserManager<Employee>> employeeManager = new();*/
       /* UserManager<Patient> patientManager;
        UserManager<Employee> employeeManager;*/

        [OneTimeSetUp]
        public void Setup()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();
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
            var employeeManager = new Mock<UserManager<Employee>>(
                new Mock<IUserStore<Employee>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Employee>>().Object,
                    new IUserValidator<Employee>[0],
                    new IPasswordValidator<Employee>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<Employee>>>().Object);
            employeeManager.Setup(userManager => userManager.CreateAsync(It.IsAny<Employee>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            employeeManager.Setup(userManager => userManager.AddToRoleAsync(It.IsAny<Employee>(), It.IsAny<string>()));
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "default")]).Returns("mock value");
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "AuthSettings:Key"))).Returns(mockConfSection.Object);
            var list = new List<IdentityRole>()
            {
                new IdentityRole("Staff"),
                new IdentityRole("Employee"),
                new IdentityRole("Patient")
            }.AsQueryable();
            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
            roleManager.Setup(r => r.Roles).Returns(list);
            
            patientService = new PatientService(patientManager.Object, configuration.Object, roleManager.Object);
            employeeService = new EmployeeService(employeeManager.Object, configuration.Object);
            authController = new AuthController(patientService, employeeService, patientManager.Object, employeeManager.Object);

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }


        [Test]
        public async Task RegisterTestReturnOk()
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
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
        }
    }
}