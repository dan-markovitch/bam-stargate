using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using Xunit;

namespace StargateAPI.Tests
{
    public class CreatePersonTests
    {
        private StargateContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new StargateContext(options);
        }

        [Fact]
        public async Task CreatePerson_Success_WhenPersonDoesNotExist()
        {
            var context = CreateInMemoryContext();
            var handler = new CreatePersonHandler(context);

            var result = await handler.Handle(new CreatePerson { Name = "John Glenn" }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task CreatePerson_PersonExistsInDatabase()
        {
            var context = CreateInMemoryContext();
            context.People.Add(new Person { Name = "John Glenn" });
            await context.SaveChangesAsync();

            var preProcessor = new CreatePersonPreProcessor(context);

            await Assert.ThrowsAsync<BadHttpRequestException>(() =>
                preProcessor.Process(new CreatePerson { Name = "John Glenn" }, CancellationToken.None));
        }

        [Fact]
        public async Task CreatePerson_StoresPerson_InDatabase()
        {
            var context = CreateInMemoryContext();
            var handler = new CreatePersonHandler(context);

            await handler.Handle(new CreatePerson { Name = "Buzz Aldrin" }, CancellationToken.None);

            var person = context.People.FirstOrDefault(p => p.Name == "Buzz Aldrin");
            Assert.NotNull(person);
        }
    }
}