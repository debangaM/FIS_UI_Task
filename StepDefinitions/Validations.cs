using Microsoft.Playwright;

namespace FIS_UI_Task.StepDefinitions
{
    [Binding]
    internal class Validations
    {
        IPage _page;
        public Validations(ScenarioContext scenarioContext)
        {
            _page = (IPage)scenarioContext["page"];
        }
        [Then(@"The page contains ""([^""]*)""")]
        public async Task ThenThePageContains(string elementReference)
        {
            await _page.Locator(elementReference).IsVisibleAsync();
        }

        [Then(@"I am on the page titled '([^']*)'")]
        public async Task ThenIAmOnThePageTitled(string expectedTitle)
        {
            var actualTitle = await _page.TitleAsync();
            Assert.AreEqual(expectedTitle, actualTitle, $"Expected page title to be '{expectedTitle}', but found '{actualTitle}'");
        }
    }
}
