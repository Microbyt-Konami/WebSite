using MicroBytKonamic.Commom.Data;

namespace MicroBytKonamic.Tests;

[TestClass]
public class IntegerIntervalsTest //: PageTest
{
    /*
    [TestMethod]
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
        await Page.GotoAsync("https://playwright.dev");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        // create a locator
        var getStarted = Page.Locator("text=Get Started");

        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the get started link.
        await getStarted.ClickAsync();

        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }
    */
    [TestMethod]
    public void IntervalsEmpty()
    {
        var intervals = new IntegerIntervals();
        var intervalsExpect = new IntegerIntervals(1);

        intervals.Add(1);
        Assert.AreEqual(intervalsExpect, intervals);
    }

    [TestMethod]
    public void Intervals1_5_2()
    {
        var intervals = new IntegerIntervals(1, 5);
        var intervalsExpect = new IntegerIntervals(1, 5);

        intervals.Add(2);
        Assert.AreEqual(intervalsExpect, intervals);
    }

    [TestMethod]
    public void Intervals1_2()
    {
        var intervals = new IntegerIntervals(1);
        var intervalsExpect = new IntegerIntervals(1, 2);

        intervals.Add(2);
        Assert.AreEqual(intervalsExpect, intervals);
    }

    [TestMethod]
    public void Intervals2_4_1()
    {
        var intervals = new IntegerIntervals(2, 4);
        var intervalsExpect = new IntegerIntervals(1, 4);

        intervals.Add(1);
        Assert.AreEqual(intervalsExpect, intervals);
    }

    [TestMethod]
    public void Intervals2_1_8_iter()
    {
        var intervals = new IntegerIntervals();
        var intervalsExpect = new IntegerIntervals(1, 8);

        intervals.Add(8);
        intervals.Add(6);
        intervals.Add(4);
        intervals.Add(2);
        intervals.Add(7);
        intervals.Add(1);
        intervals.Add(3);
        intervals.Add(5);
        Assert.AreEqual(intervalsExpect, intervals);
    }
}
