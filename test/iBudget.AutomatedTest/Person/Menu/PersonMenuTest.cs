using iBudget.AutomatedTest.Person.Shared;

namespace iBudget.AutomatedTest.Person.Menu;

public class PersonMenuTest : PersonSharedTest
{
    [Fact]
    public void HasPersonMenu()
    {
        Assert.NotNull(_personMenu);
    }
    
    [Fact]
    public void HasAccessToPersonIndex()
    {
        _personMenu.Click();
        Assert.Equal(_personController, _uri.AbsolutePath);
    }
}