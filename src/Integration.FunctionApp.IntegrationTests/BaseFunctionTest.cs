using JoachimDalen.AzureFunctions.TestUtils;
using JoachimDalen.AzureFunctions.TestUtils.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration.FunctionApp.IntegrationTests
{
    [TestClass]
    public class BaseFunctionTest : FunctionTestClass
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var settings = new TestUtilsSettings
            {
                FuncAppPath = "../../../../Integration.FunctionApp/bin/Debug/netcoreapp3.1",
                UseAzuriteStorage = true,
                PersistAzureContainers = false,
                ClearStorageAfterRun = true,
                WriteLog = true,
                RunAzurite = false
            };
            AssemblyInitialize(context, settings);
        }

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void TestFixtureSetup(TestContext context)
        {
            ClassInitialize(context);
        }

        [TestInitialize]
        public void Setup()
        {
            TestInitialize();
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void TestFixtureTearDown()
        {
            ClassCleanup();
        }

        [TestCleanup]
        public void TearDown()
        {
            TestCleanup();
        }
    }
}