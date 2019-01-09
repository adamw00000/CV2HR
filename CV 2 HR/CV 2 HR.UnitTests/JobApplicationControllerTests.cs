using System;
using Xunit;
using CV2HR.Controllers;
using CV2HR.Models;
using CV2HR.Services;
using Moq;
using System.Threading.Tasks;
using Shouldly;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace CV2HR.UnitTests
{
    public class JobApplicationControllerTests
    {
        readonly JobOffer testOffer = new JobOffer
        {
            Id = 1,
            CompanyId = 1,
            Created = new DateTime(2012, 1, 1),
            ValidUntil = DateTime.Now.AddDays(3),
            Description = new string('a', 1000),
            JobTitle = "Student",
            Location = "Warsaw",
            UserId = "offer author"
        };

        JobOffer GetTestOffer()
        {
            return new JobOffer
            {
                Id = 1,
                CompanyId = 1,
                Created = new DateTime(2012, 1, 1),
                ValidUntil = DateTime.Now.AddDays(3),
                Description = new string('a', 1000),
                JobTitle = "Student",
                Location = "Warsaw",
                UserId = "offer author"
            };
        }

        JobApplication GetTestApplication()
        {
            JobApplication testApplication = new JobApplication()
            {
                Id = 1,
                ContactAgreement = true,
                CvUri = "https://google.com",
                EmailAddress = "aaa@aaa.aaa",
                FirstName = "Jan",
                LastName = "Kowalski",
                OfferId = 1,
                PhoneNumber = "111111111",
                UserId = "user",
                Offer = testOffer
            };

            return testApplication;
        }

        JobApplicationCreateViewModel GetTestApplicationCreateViewModel()
        {
            var testApplication = GetTestApplication();

            var viewModel = new JobApplicationCreateViewModel
            {
                ContactAgreement = testApplication.ContactAgreement,
                CvUri = testApplication.CvUri,
                EmailAddress = testApplication.EmailAddress,
                FirstName = testApplication.FirstName,
                LastName = testApplication.LastName,
                Id = testApplication.Id,
                Offer = testOffer,
                OfferId = testApplication.OfferId,
                PhoneNumber = testApplication.PhoneNumber,
                UserId = testApplication.UserId
            };
            return viewModel;
        }

        [Fact]
        public async Task AddGet_WhenOfferDoesNotExist_ReturnsNotFound()
        {
            int id = -1;
            var mockJobOfferService = new Mock<IJobOfferService>();
            mockJobOfferService
                .Setup(service => service.GetOfferAsync(id))
                .ReturnsAsync((JobOffer)null);
            JobApplicationController controller = new JobApplicationController(null, mockJobOfferService.Object, null, null);

            var result = await controller.Add(id);

            result.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AddGet_WhenOfferExpired_ReturnsBadRequest()
        {
            int id = -1;
            var mockJobOfferService = new Mock<IJobOfferService>();
            var offer = GetTestOffer();
            offer.ValidUntil = DateTime.Now.AddDays(-2);
            mockJobOfferService
                .Setup(service => service.GetOfferAsync(id))
                .ReturnsAsync(offer);
            JobApplicationController controller = new JobApplicationController(null, mockJobOfferService.Object, null, null);

            var result = await controller.Add(id);

            result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AddGet_WhenOfferExists_ReturnsAddView()
        {
            int id = 1;
            var mockJobOfferService = new Mock<IJobOfferService>();
            mockJobOfferService
                .Setup(service => service.GetOfferAsync(id))
                .ReturnsAsync(testOffer);
            JobApplicationController controller = new JobApplicationController(null, mockJobOfferService.Object, null, null);

            var result = await controller.Add(id);

            var viewResult = result.ShouldBeOfType<ViewResult>();
            var viewModel = viewResult.Model.ShouldBeOfType<JobApplicationCreateViewModel>();
            viewModel.Offer.ShouldBe(testOffer);
            viewModel.OfferId.ShouldBe(testOffer.Id);
        }

        [Fact]
        public async Task AddPost_WhenModelIsNull_ThrowsNullReferenceException()
        {
            JobApplicationCreateViewModel viewModel = null;

            var controller = new JobApplicationController(null, null, null, null);

            var result = await controller.Add(viewModel).ShouldThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task AddPost_WhenModelIsNotValid_ReturnsAddView()
        {
            int id = 1;
            var viewModel = GetTestApplicationCreateViewModel();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns(It.IsAny<string>());
            var mockBlobService = new Mock<IBlobService>();
            mockBlobService
                .Setup(service => service.ValidateFile(viewModel.CvFile, It.IsAny<ModelStateDictionary>()));
            var mockJobOfferService = new Mock<IJobOfferService>();
            mockJobOfferService
                .Setup(service => service.GetOfferAsync(id))
                .ReturnsAsync(testOffer);
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new JobApplicationController(null, mockJobOfferService.Object, mockBlobService.Object, mockUserManager.Object)
            {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("test", "test");

            var result = await controller.Add(viewModel);

            var view = result.ShouldBeOfType<ViewResult>();
            var model = view.Model.ShouldBeOfType<JobApplicationCreateViewModel>();
            model.ShouldBe(viewModel);
            model.Offer.ShouldBe(testOffer);
        }

        [Fact]
        public async Task AddPost_WhenBlobFails_ReturnsServerError()
        {
            var blobName = "1.pdf";
            var viewModel = GetTestApplicationCreateViewModel();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns(It.IsAny<string>());
            var mockBlobService = new Mock<IBlobService>();
            mockBlobService
                .Setup(service => service.GetFileName(viewModel))
                .Returns(blobName);
            mockBlobService
                .Setup(service => service.AddFile(viewModel.CvFile, blobName))
                .ReturnsAsync((Uri)null);
            mockBlobService
                .Setup(service => service.ValidateFile(viewModel.CvFile, new ModelStateDictionary()));
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new JobApplicationController(null, null, mockBlobService.Object, mockUserManager.Object)
            {
                ObjectValidator = objectValidator.Object
            };

            var result = await controller.Add(viewModel);

            var statusCode = result.ShouldBeOfType<StatusCodeResult>();
            statusCode.StatusCode.ShouldBe(500);
        }

        [Fact]
        public async Task AddPost_WhenDbFails_DeletesFileFromBlobAndReturnsServerError()
        {
            var blobName = "1.pdf";
            var uri = new Uri("https://google.com");
            var viewModel = GetTestApplicationCreateViewModel();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns(It.IsAny<string>());
            var mockBlobService = new Mock<IBlobService>();
            mockBlobService
                .Setup(service => service.GetFileName(viewModel))
                .Returns(blobName);
            mockBlobService
                .Setup(service => service.AddFile(viewModel.CvFile, blobName))
                .ReturnsAsync(uri);
            mockBlobService
                .Setup(service => service.ValidateFile(viewModel.CvFile, It.IsAny<ModelStateDictionary>()));
            mockBlobService
                .Setup(service => service.RemoveFile(blobName))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var mockJobApplicationService = new Mock<IJobApplicationService>();
            mockJobApplicationService
                .Setup(service => service.AddJobApplicationAsync(It.IsAny<JobApplication>()))
                .ReturnsAsync(false);
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new JobApplicationController(mockJobApplicationService.Object, null, 
                mockBlobService.Object, mockUserManager.Object)
            {
                ObjectValidator = objectValidator.Object
            };

            var result = await controller.Add(viewModel);

            var statusCode = result.ShouldBeOfType<StatusCodeResult>();
            statusCode.StatusCode.ShouldBe(500);
            mockBlobService.Verify();
        }

        [Fact]
        public async Task AddPost_IfThereAreNoErrors_AddsApplicationAndRedirectsToDetails()
        {
            var blobName = "1.pdf";
            var uri = new Uri("https://google.com");
            var viewModel = GetTestApplicationCreateViewModel();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns(It.IsAny<string>());
            var mockBlobService = new Mock<IBlobService>();
            mockBlobService
                .Setup(service => service.GetFileName(viewModel))
                .Returns(blobName);
            mockBlobService
                .Setup(service => service.AddFile(viewModel.CvFile, blobName))
                .ReturnsAsync(uri);
            mockBlobService
                .Setup(service => service.ValidateFile(viewModel.CvFile, new ModelStateDictionary()));
            var mockJobApplicationService = new Mock<IJobApplicationService>();
            mockJobApplicationService
                .Setup(service => service.AddJobApplicationAsync(It.IsAny<JobApplication>()))
                .ReturnsAsync(true)
                .Verifiable();
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new JobApplicationController(mockJobApplicationService.Object, null,
                mockBlobService.Object, mockUserManager.Object)
            {
                ObjectValidator = objectValidator.Object
            };

            var result = await controller.Add(viewModel);

            var redirect = result.ShouldBeOfType<RedirectToActionResult>();
            redirect.ActionName = "Details";
            redirect.ControllerName = "JobApplication";
            redirect.RouteValues["Id"].ShouldBe(viewModel.Id);
            mockJobApplicationService.Verify();
        }

        [Fact]
        public async Task DetailsGet_WhenIdIsNull_ReturnsBadRequest()
        {
            int? id = null;
            JobApplicationController controller = new JobApplicationController(null, null, null, null);

            var result = await controller.Details(id);

            result.ShouldBeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DetailsGet_WhenApplicationDoesNotExist_ReturnsBadRequest()
        {
            int? id = 1;
            var mockJobApplicationService = new Mock<IJobApplicationService>();
            mockJobApplicationService
                .Setup(service => service.GetJobApplicationAsync(id.Value))
                .ReturnsAsync((JobApplication)null);
            JobApplicationController controller = new JobApplicationController(mockJobApplicationService.Object, null, null, null);

            var result = await controller.Details(id);

            result.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DetailsGet_WhenUserIsApplicationAuthor_ReturnsDetailsView()
        {
            int? id = 1;
            var application = GetTestApplication();
            var mockJobApplicationService = new Mock<IJobApplicationService>();
            mockJobApplicationService
                .Setup(service => service.GetJobApplicationAsync(id.Value))
                .ReturnsAsync(application);
            ClaimsPrincipal user = new ClaimsPrincipal();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUser())
                .Returns(user);
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns("user");
            JobApplicationController controller =
                new JobApplicationController(mockJobApplicationService.Object, null, null, mockUserManager.Object);

            var result = await controller.Details(id);

            var view = result.ShouldBeOfType<ViewResult>();
            view.Model.ShouldBe(application);
        }

        [Fact]
        public async Task DetailsGet_WhenUserIsAdmin_ReturnsDetailsView()
        {
            int? id = 1;
            var application = GetTestApplication();
            var mockJobApplicationService = new Mock<IJobApplicationService>();
            mockJobApplicationService
                .Setup(service => service.GetJobApplicationAsync(id.Value))
                .ReturnsAsync(application);
            ClaimsPrincipal user = new ClaimsPrincipal();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUser())
                .Returns(user);
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns("not a creator");
            mockUserManager
                .Setup(manager => manager.AuthorizeUserAsync("Admin"))
                .ReturnsAsync(AuthorizationResult.Success());
            mockUserManager
                .Setup(manager => manager.AuthorizeUserAsync("Manager"))
                .ReturnsAsync(AuthorizationResult.Failed());
            JobApplicationController controller = 
                new JobApplicationController(mockJobApplicationService.Object, null, null, mockUserManager.Object);

            var result = await controller.Details(id);

            var view = result.ShouldBeOfType<ViewResult>();
            view.Model.ShouldBe(application);
        }

        [Fact]
        public async Task DetailsGet_WhenUserIsOfferAuthorManager_ReturnsDetailsView()
        {
            int? id = 1;
            var application = GetTestApplication();
            var mockJobApplicationService = new Mock<IJobApplicationService>();
            mockJobApplicationService
                .Setup(service => service.GetJobApplicationAsync(id.Value))
                .ReturnsAsync(application);
            ClaimsPrincipal user = new ClaimsPrincipal();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUser())
                .Returns(user);
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns("offer author");
            mockUserManager
                .Setup(manager => manager.AuthorizeUserAsync("Admin"))
                .ReturnsAsync(AuthorizationResult.Failed());
            mockUserManager
                .Setup(manager => manager.AuthorizeUserAsync("Manager"))
                .ReturnsAsync(AuthorizationResult.Success());
            JobApplicationController controller =
                new JobApplicationController(mockJobApplicationService.Object, null, null, mockUserManager.Object);

            var result = await controller.Details(id);

            var view = result.ShouldBeOfType<ViewResult>();
            view.Model.ShouldBe(application);
        }

        [Fact]
        public async Task DetailsGet_WhenUserIsNotAssociatedWithApplication_ReturnsDeniedView()
        {
            int? id = 1;
            var application = GetTestApplication();
            var mockJobApplicationService = new Mock<IJobApplicationService>();
            mockJobApplicationService
                .Setup(service => service.GetJobApplicationAsync(id.Value))
                .ReturnsAsync(application);
            ClaimsPrincipal user = new ClaimsPrincipal();
            var mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(manager => manager.GetUser())
                .Returns(user);
            mockUserManager
                .Setup(manager => manager.GetUserId())
                .Returns("random user");
            mockUserManager
                .Setup(manager => manager.AuthorizeUserAsync("Admin"))
                .ReturnsAsync(AuthorizationResult.Failed());
            mockUserManager
                .Setup(manager => manager.AuthorizeUserAsync("Manager"))
                .ReturnsAsync(AuthorizationResult.Failed());
            JobApplicationController controller =
                new JobApplicationController(mockJobApplicationService.Object, null, null, mockUserManager.Object);

            var result = await controller.Details(id);

            var redirect = result.ShouldBeOfType<RedirectToActionResult>();
            redirect.ActionName.ShouldBe("Denied");
            redirect.ControllerName.ShouldBe("Session");
        }
    }
}
