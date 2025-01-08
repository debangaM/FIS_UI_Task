using FIS_UI_Task.Utilities;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FIS_UI_Task.StepDefinitions
{
    [Binding]
    internal class PageInteractions
    {
        IBrowser _browser;
        IBrowserContext _context;
        IPage _page;
        JsonHandler _jsonHandler;
        IPage _tempPage;
        public PageInteractions(ScenarioContext scenarioContext, JsonHandler jsonHandler)
        {
            _browser=(IBrowser)scenarioContext["browser"];
            _context = (IBrowserContext)scenarioContext["context"];
            _page = (IPage)scenarioContext["page"];
            _jsonHandler = jsonHandler;
        }
        [Given(@"I navigate to url '([^']*)'")]
        public async Task GivenINavigateToUrl(string url)
        {
            await _page.GotoAsync(url);
        }

        [When(@"I type ""([^""]*)"" into ""([^""]*)""")]
        public async Task WhenITypeInto(string text, string elementReference)
        {
            string elementXpath = _jsonHandler.GetValue(elementReference);
            await _page.Locator(elementXpath).FillAsync(text);
        }

        [When(@"I click on ""([^""]*)""")]
        public async Task WhenIClickOn(string elementReference)
        {
            string elementXpath = _jsonHandler.GetValue(elementReference);
            await _page.Locator(elementXpath).ClickAsync();
        }

        [When(@"I click on ""([^""]*)"" and focus on new tab")]
        public async Task WhenIClickOnAndFocusOnNewTab(string elementReference)
        {
            var newPage = await _context.RunAndWaitForPageAsync(async () =>
            {
                string elementXpath = _jsonHandler.GetValue(elementReference);
                await _page.Locator(elementXpath).ClickAsync();
            });
            _tempPage = _page;
            _page = newPage;
        }

        [Given(@"I load all elements of ""([^""]*)""")]
        public void GivenILoadAllElementsOf(string pageName)
        {
            _jsonHandler.LoadJson(pageName);
        }

        [When(@"I close current page and focus on previous tab")]
        public async Task WhenICloseCurrentPageAndFocusOnPreviousTab()
        {
            await _page.CloseAsync();
            _page = _tempPage;
        }        
    }
}
