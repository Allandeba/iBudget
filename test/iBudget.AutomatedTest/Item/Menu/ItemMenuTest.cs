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
    public void HasAccessToItemIndex()
    {
        _itemMenu.Click();
        Assert.Equal(_itemController, _uri.AbsolutePath);
    }
}