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
    public async Task HasAccessToPersonIndex()
    {
        _personMenu.Click();
        
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_personController, _uri.AbsolutePath);
    }
}