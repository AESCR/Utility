using Common.Utility.Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.UnitTest.Code
{
    [TestClass]
    public class CSharpTest
    {
        [TestMethod]
        public void TestCSharp()
        {
            {
                CSharpTemplate cSharpTemplate = new CSharpTemplate();
                cSharpTemplate.ImportNamespace().SetNamespace("TestName").SetClass("TestClass");
                cSharpTemplate .SetInterfaceMethod("Test","string","Edit");
                cSharpTemplate.SetInterfaceMethod("Test1","void","Edit");
                cSharpTemplate.SetProperty("string", "name");
                cSharpTemplate.Save("test");
            }

            ;
        }
    }
}