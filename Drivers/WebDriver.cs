using Microsoft.Playwright;

namespace FIS_UI_Task.Drivers
{
    public class WebDriverManager
    {
        private readonly ScenarioContext _scenarioContext;

        public WebDriverManager(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        public WebDriver GetWebDriver()
        {
            if (_scenarioContext.ContainsKey("webDriver"))
            {
                return (WebDriver)_scenarioContext["webDriver"];
            }

            var webDriver = new WebDriver(_scenarioContext);
            _scenarioContext["webDriver"] = webDriver;
            return webDriver;
        }
    }
    public class WebDriver
    {
        public IPage _page;
        IBrowser _browser;
        internal int width = 1920;
        internal int height = 1080;
        internal string _browserType = "chromium";
        private readonly ScenarioContext _scenarioContext;

        public WebDriver(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        internal async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            switch (_browserType)
            {
                case "chromium": _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }); break;
                case "firefox": _browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }); break;
                case "webkit": _browser = await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }); break;
            }
            var _browserContext = await _browser.NewContextAsync(new BrowserNewContextOptions { ViewportSize = new ViewportSize { Width = width, Height = height } });
            _page = await _browserContext.NewPageAsync();
            _page.SetDefaultTimeout(10000);

            _scenarioContext["page"] = _page;
            _scenarioContext["browser"] = _browser;
            _scenarioContext["context"] = _browserContext;
        }
        public async Task Cleanup()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
        }

        internal async Task TakeScreenshotAsync(string screenshotPath)
        {
            await _page.ScreenshotAsync(new() { Path = screenshotPath });
        }
    }
}
