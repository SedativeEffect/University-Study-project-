using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using module_10;
using module_10.DAL.University;
using module_10.DAL.University.Entities;
using module_10.DL.Models;
using module_10.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Homework = module_10.DAL.University.Entities.Homework;

namespace module_10_BLL.IntegrationTests
{
    [TestFixture]
    public class HomeworkControllerTests
    {
        private WebApplicationFactory<Startup> _webHost;

        [SetUp]
        public void Setup()
        {
            _webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<UniversityContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<UniversityContext>(options =>
                    {
                        options.UseInMemoryDatabase("UniversityInMemory");
                    });
                });
            });
        }
        [TearDown]
        public void Teardown()
        {
            _webHost.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor =
                        services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<UniversityContext>));
                    services.Remove(dbContextDescriptor);
                });
            });
        }
        //[Test]
        //public void GetAllHomeworks_Return_Homeworks()
        //{
        //    using var dbContext = new UniversityContext(_options);
        //    var homework = new Homework()
        //    {
        //        Id = Guid.NewGuid()
        //    };
        //    var lecture = new Lecture()
        //    {
        //        Id = Guid.NewGuid(),
        //        Homework = homework
        //    };

        //    dbContext.Homeworks.Add(homework);
        //    dbContext.Lectures.Add(lecture);
        //    dbContext.SaveChanges();
        //    var configBll = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile<BusinessLogicMapperProfile>();
        //    });
        //    var configDal = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile<DataAccessMapperProfile>();
        //    });
        //    var serviceProvider = new ServiceCollection()
        //        .AddLogging()
        //        .BuildServiceProvider();

        //    var factory = serviceProvider.GetService<ILoggerFactory>();

        //    var loggerHomework = factory.CreateLogger<HomeworkService>();
        //    var loggerLecture = factory.CreateLogger<LectureService>();

        //    var lectureRepository = new LecturesRepository(dbContext, new Mapper(configBll));
        //    var lectureService = new LectureService(lectureRepository, loggerLecture);

        //    var homeworkRepository = new HomeworksRepository(dbContext, new Mapper(configDal));
        //    var homeworkService = new HomeworkService(homeworkRepository, loggerHomework, lectureService);

        //    var result = homeworkService.GetAllHomeworks();
        //    Assert.That(result.FirstOrDefault().Id, Is.EqualTo(homework.Id));
        //}
        [Test]
        public async Task GetAllHomeworks_Return_Homeworks()
        {
            var dbContext = _webHost.Services.CreateScope().ServiceProvider.GetService<UniversityContext>();

            var homeworks = new List<Homework> { new() { Id = Guid.NewGuid() }, new() { Id = Guid.NewGuid() }, new() { Id = Guid.NewGuid() } };

            await dbContext.AddRangeAsync(homeworks);
            await dbContext.SaveChangesAsync();

            var httpClient = _webHost.CreateClient();

            var response = await httpClient.GetAsync("homework");
            List<HomeworkOutput> homeworksResult = new();

            var responseBody = await response.Content.ReadAsStringAsync();
            homeworksResult = JsonConvert.DeserializeObject<List<HomeworkOutput>>(responseBody);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(homeworksResult.Count, homeworks.Count);

        }
        [Test]
        public async Task GetHomework_Return_Homework()
        {
            var dbContext = _webHost.Services.CreateScope().ServiceProvider.GetService<UniversityContext>();
            var guid = Guid.NewGuid();
            var homeworks = new List<Homework> { new() { Id = guid, Task = "Job" }, new() { Id = Guid.NewGuid() }, new() { Id = Guid.NewGuid() } };

            await dbContext.AddRangeAsync(homeworks);
            await dbContext.SaveChangesAsync();

            using var httpClient = _webHost.CreateClient();

            var response = await httpClient.GetAsync($"homework/{guid}");


            var responseBody = await response.Content.ReadAsStringAsync();
            var homeworkResult = JsonConvert.DeserializeObject<HomeworkOutput>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(homeworkResult.Id, homeworks.Find(x => x.Id == guid)?.Id);
        }

        [Test]
        public async Task CreateHomework_Return_Homework()
        {
            var dbContext = _webHost.Services.CreateScope().ServiceProvider.GetService<UniversityContext>();
            var guid = Guid.NewGuid();
            var lecture = new module_10.DAL.University.Entities.Lecture()
            {
                Id = guid
            };
            await dbContext.AddAsync(lecture);
            await dbContext.SaveChangesAsync();

            var homework = new HomeworkToLectureInput()
            {
                LectureId = guid,
                Task = "Do job"
            };

            var jsonHomework = JsonConvert.SerializeObject(homework);

            var content = new StringContent(jsonHomework, Encoding.UTF8, MediaTypeNames.Application.Json);
            using var httpClient = _webHost.CreateClient();

            var response = await httpClient.PostAsync($"homework/{guid}", content);


            var responseBody = await response.Content.ReadAsStringAsync();
            var homeworkResult = JsonConvert.DeserializeObject<HomeworkOutput>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(homeworkResult.Task, homework.Task);
        }

        [Test]
        public async Task UpdateHomework_Return_Homework()
        {
            var dbContext = _webHost.Services.CreateScope().ServiceProvider.GetService<UniversityContext>();
            var guid = Guid.NewGuid();


            var homeworks = new List<Homework> { new() { Id = guid, Task = "Job" }, new() { Id = Guid.NewGuid() }, new() { Id = Guid.NewGuid() } };

            await dbContext.AddRangeAsync(homeworks);
            await dbContext.SaveChangesAsync();

            var homework = new HomeworkToUpdate()
            {
                Task = "AnotherJob"
            };

            var jsonHomework = JsonConvert.SerializeObject(homework);

            var content = new StringContent(jsonHomework, Encoding.UTF8, MediaTypeNames.Application.Json);
            using var httpClient = _webHost.CreateClient();

            var response = await httpClient.PutAsync($"homework/{guid}", content);


            var responseBody = await response.Content.ReadAsStringAsync();
            var homeworkResult = JsonConvert.DeserializeObject<HomeworkOutput>(responseBody);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreNotEqual(homeworkResult.Task, homework.Task);
        }

        [Test]
        public async Task DeleteHomework_Return_Ok()
        {
            var dbContext = _webHost.Services.CreateScope().ServiceProvider.GetService<UniversityContext>();
            var guid = Guid.NewGuid();

            var homeworks = new List<Homework> { new() { Id = guid, Task = "Job" }, new() { Id = Guid.NewGuid() }, new() { Id = Guid.NewGuid() } };

            await dbContext.AddRangeAsync(homeworks);
            await dbContext.SaveChangesAsync();

            using var httpClient = _webHost.CreateClient();

            var response = await httpClient.DeleteAsync($"homework/{guid}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
