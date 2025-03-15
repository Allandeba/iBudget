using iBudget.AutomatedTest.Item.Shared;

namespace iBudget.AutomatedTest.Item.Menu;

public class ItemMenuTest : ItemSharedTest
{
    [Fact]
    public void HasItemMenu()
    {
        Assert.NotNull(_itemMenu);
    }
    
    [Fact]
    public async Task HasAccessToItemIndex()
    {
        _itemMenu.Click();
        
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_itemController, _uri.AbsolutePath);
    }
}