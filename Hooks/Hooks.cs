using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using FIS_UI_Task.Drivers;

namespace FIS_UI_Task.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly WebDriverManager _webDriverManager;

        private static ExtentReports _extent;
        private ExtentTest _scenario;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _webDriverManager = new WebDriverManager(_scenarioContext);
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {

            string reportFolder = Path.Combine(Directory.GetCurrentDirectory(), "ExtentReports");
            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }

            string reportPath = Path.Combine(reportFolder, $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            var sparkReporter= new ExtentSparkReporter(reportPath);
            sparkReporter.Config.DocumentTitle = "Test Report";
            sparkReporter.Config.ReportName = "Automation Test Results";
            sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;

            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
        }

        [AfterTestRun]
        public static void FlushReport()
        {

            if (_extent != null)
            {
                _extent.Flush();
            }
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            var webDriver = _webDriverManager.GetWebDriver();
            await webDriver.Setup();

            _scenarioContext["webDriver"] = webDriver;

            _scenario = _extent.CreateTest(_scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            var webDriver = _webDriverManager.GetWebDriver();
            await webDriver.Cleanup();

            var scenarioStatus = _scenarioContext.ScenarioExecutionStatus;

            if (scenarioStatus != ScenarioExecutionStatus.OK)
            {
                var errorMessage = _scenarioContext.TestError?.Message;
                _scenario.Log(Status.Fail, errorMessage);
            }
        }

        [AfterStep]
        public async Task AfterStep()
        {
            await Task.Delay(3000);

            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepInfo = $"{stepType}: {_scenarioContext.StepContext.StepInfo.Text}";

            if (_scenarioContext.TestError == null)
            {
                _scenario.Log(Status.Pass, stepInfo);
            }
            else
            {
                var webDriver = _webDriverManager.GetWebDriver();
                string screenshotsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ExtentReports", "Screenshots");
                if (!Directory.Exists(screenshotsFolder))
                {
                    Directory.CreateDirectory(screenshotsFolder);
                }

                string screenshotPath = Path.Combine(screenshotsFolder, $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                await webDriver.TakeScreenshotAsync(screenshotPath);

                // Embed screenshot in report
                _scenario.Log(Status.Fail, stepInfo);
                _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);
                _scenario.AddScreenCaptureFromPath(screenshotPath);
            }
        }
    }
}
