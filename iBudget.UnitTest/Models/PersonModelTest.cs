using iBudget.Models;

namespace iBudget.UnitTest.Models
{
    public class PersonModelTest
    {
        [Fact]
        public void ShouldSetLastAddedImageAsMainImage()
        {
            PersonModel person =
                new()
                {
                    Document = new(),
                    Contact = new(),
                    FirstName = "Primeiro",
                    LastName = "Segundo"
                };

            Assert.Equal("Primeiro Segundo", person.PersonName);
        }
    }
}
