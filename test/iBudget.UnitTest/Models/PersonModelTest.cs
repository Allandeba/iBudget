using iBudget.DAO.Entities;

namespace iBudget.UnitTest.Models;

public class PersonModelTest
{
    [Fact]
    public void ShouldSetLastAddedImageAsMainImage()
    {
        PersonModel person =
            new()
            {
                Document = new DocumentModel(),
                Contact = new ContactModel(),
                FirstName = "Primeiro",
                LastName = "Segundo"
            };

        Assert.Equal("Primeiro Segundo", person.PersonName);
    }
}