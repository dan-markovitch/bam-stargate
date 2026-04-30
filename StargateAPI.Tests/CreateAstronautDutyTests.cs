using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using Xunit;

namespace StargateAPI.Tests
{
    public class CreateAstronautDutyTests
    {
        private StargateContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new StargateContext(options);
        }

        [Fact]
        public async Task CreateAstronautDuty_ThrowsException_WhenPersonDoesNotExist()
        {
            var context = CreateInMemoryContext();
            var preProcessor = new CreateAstronautDutyPreProcessor(context);

            await Assert.ThrowsAsync<BadHttpRequestException>(() =>
                preProcessor.Process(new CreateAstronautDuty
                {
                    Name = "Nobody",
                    Rank = "Colonel",
                    DutyTitle = "Commander",
                    DutyStartDate = DateTime.UtcNow
                }, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAstronautDuty_ThrowsException_WhenDuplicateDutyExists()
        {
            var context = CreateInMemoryContext();
            var person = new Person { Name = "John Glenn" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var startDate = DateTime.UtcNow.Date;
            context.AstronautDuties.Add(new AstronautDuty
            {
                PersonId = person.Id,
                Rank = "Colonel",
                DutyTitle = "Commander",
                DutyStartDate = startDate
            });
            await context.SaveChangesAsync();

            var preProcessor = new CreateAstronautDutyPreProcessor(context);

            await Assert.ThrowsAsync<BadHttpRequestException>(() =>
                preProcessor.Process(new CreateAstronautDuty
                {
                    Name = "John Glenn",
                    Rank = "Colonel",
                    DutyTitle = "Commander",
                    DutyStartDate = startDate
                }, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAstronautDuty_SetsCareerEndDate_WhenRetired()
        {
            var context = CreateInMemoryContext();
            var person = new Person { Name = "Buzz Aldrin" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var detail = new AstronautDetail
            {
                PersonId = person.Id,
                CurrentRank = "Colonel",
                CurrentDutyTitle = "Commander",
                CareerStartDate = DateTime.UtcNow.AddYears(-5)
            };
            context.AstronautDetails.Add(detail);
            await context.SaveChangesAsync();

            var retireDate = DateTime.UtcNow.Date;
            context.AstronautDuties.Add(new AstronautDuty
            {
                PersonId = person.Id,
                Rank = "Colonel",
                DutyTitle = "Commander",
                DutyStartDate = retireDate.AddYears(-1)
            });
            await context.SaveChangesAsync();

            detail.CurrentDutyTitle = "RETIRED";
            detail.CurrentRank = "Colonel";
            detail.CareerEndDate = retireDate.AddDays(-1);
            context.AstronautDetails.Update(detail);
            await context.SaveChangesAsync();

            var updatedDetail = context.AstronautDetails.FirstOrDefault(d => d.PersonId == person.Id);
            Assert.NotNull(updatedDetail);
            Assert.Equal(retireDate.AddDays(-1), updatedDetail.CareerEndDate);
        }

        [Fact]
        public async Task CreateAstronautDuty_SetsPreviousDutyEndDate()
        {
            var context = CreateInMemoryContext();
            var person = new Person { Name = "Neil Armstrong" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var firstDutyStart = DateTime.UtcNow.AddYears(-2).Date;
            var existingDuty = new AstronautDuty
            {
                PersonId = person.Id,
                Rank = "Colonel",
                DutyTitle = "Pilot",
                DutyStartDate = firstDutyStart,
                DutyEndDate = null
            };
            context.AstronautDuties.Add(existingDuty);
            await context.SaveChangesAsync();

            var newDutyStart = DateTime.UtcNow.Date;
            existingDuty.DutyEndDate = newDutyStart.AddDays(-1);
            context.AstronautDuties.Update(existingDuty);
            await context.SaveChangesAsync();

            var updated = context.AstronautDuties.FirstOrDefault(d => d.DutyTitle == "Pilot");
            Assert.NotNull(updated);
            Assert.Equal(newDutyStart.AddDays(-1), updated.DutyEndDate);
        }
    }
}