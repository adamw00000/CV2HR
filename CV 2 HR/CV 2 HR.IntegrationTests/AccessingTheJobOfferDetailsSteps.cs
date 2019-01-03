using CV2HR.Controllers;
using CV2HR.Models;
using CV2HR.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CV2HR.IntegrationTests
{
    [Binding]
    public class AccessingTheJobOfferDetailsSteps
    {
        readonly Mock<IJobOfferService> mockJobOfferService = new Mock<IJobOfferService>();
        List<JobOffer> offers = new List<JobOffer>();
        int id;
        readonly JobOfferController controller;
        object result;

        public AccessingTheJobOfferDetailsSteps()
        {
            controller = new JobOfferController(mockJobOfferService.Object, null, null);
        }

        [Given(@"when there are following offers in the database")]
        public void GivenWhenThereAreFollowingOffersInTheDatabase(Table table)
        {
            foreach (var row in table.Rows)
            {
                JobOffer newOffer = ParseRow(row);
                offers.Add(newOffer);
            }

            mockJobOfferService
                .Setup(service => service.GetOfferAsync(It.IsInRange(0, 2, Range.Inclusive)))
                .ReturnsAsync((int id) => offers[id]);
            mockJobOfferService
                .Setup(service => service.GetOfferAsync(It.Is<int>(i => i > 2 || i < 0)))
                .ReturnsAsync((JobOffer)null);
        }

        private static JobOffer ParseRow(TableRow row)
        {
            return new JobOffer
            {
                Created = DateTime.Parse(row["Created"]),
                CompanyId = int.Parse(row["CompanyId"]),
                Description = row["Description"],
                Id = int.Parse(row["Id"]),
                JobTitle = row["JobTitle"],
                Location = row["Location"],
                SalaryFrom = row["SalaryFrom"] == "" ? (decimal?)null : decimal.Parse(row["SalaryFrom"]),
                SalaryTo = row["SalaryTo"] == "" ? (decimal?)null : decimal.Parse(row["SalaryTo"]),
                UserId = row["UserId"],
                ValidUntil = row["ValidUntil"] == "" ? (DateTime?)null : DateTime.Parse(row["ValidUntil"])
            };
        }

        [When(@"I want to see details of job offer (.*)")]
        public async Task WhenIWantToSeeDetailsOfJobOffer(int id)
        {
            this.id = id;
            result = await controller.Details(id);
        }
        
        [Then(@"I get following offer on the screen")]
        public void ThenIGetFollowingOfferOnTheScreen(Table table)
        {
            JobOffer expectedOffer = ParseRow(table.Rows[0]);

            var view = result.ShouldBeOfType<ViewResult>();
            view.Model.ShouldBe(expectedOffer);
        }

        [Then(@"I get NotFound error")]
        public void ThenIGetNotFoundError()
        {
            var view = result.ShouldBeOfType<NotFoundResult>();
        }

    }
}
