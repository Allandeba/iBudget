using iBudget.Framework;
using iBudget.Framework.Extensions;

namespace iBudget.UnitTest.Extensions;

public class StringExtensionsTest
{
    [Fact]
    public void ShouldFindAccentWord()
    {
        const string wordToFind = "acentuacoes";
        const string accentSentence = "Uma palavra com acentuações";

        var accentWord = accentSentence;
        var foundWord = accentWord.Contains(wordToFind);
        Assert.False(foundWord);

        var unaccentWord = accentWord.Unaccent();
        var foundWordUnaccent = unaccentWord.Contains(wordToFind);
        Assert.True(foundWordUnaccent);
    }

    [Fact]
    public void ShouldRemoveDots()
    {
        const string dotString = "123.d21-f32/8493%$#!@#&*()(_-+=_-`|32210";
        const string successReturning = "1232132849332210";

        var result = dotString.OnlyNumbers();
        Assert.Equal(result, successReturning);
    }
}
