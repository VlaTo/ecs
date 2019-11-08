using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2.EntityPath.Parsing
{
    [TestClass]
    public class ParsingWildcardPathTests : ParsingContext
    {

        public override void Act()
        {
            EntityPath = LibraProgramming.Ecs.Core.Path.EntityPath.Parse("//**/*");
        }

        [TestMethod]
        public void CheckWildcardPath()
        {
            Assert.IsNotNull(EntityPath);
        }
    }
}