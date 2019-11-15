using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EntityPath.Parsing
{
    [TestClass]
    public class LevelUpPathTests : ParsingContext
    {
        public override void Act()
        {
            EntityPath = LibraProgramming.Ecs.Core.Path.EntityPath.Parse("../../Test");
        }

        [TestMethod]
        public void CheckWildcardPath()
        {
            Assert.IsNotNull(EntityPath);
        }
    }
}