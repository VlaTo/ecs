using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EntityPath.Parsing
{
    [TestClass]
    public class EntityNameTests : ParsingContext
    {
        public override void Act()
        {
            EntityPath = LibraProgramming.Ecs.Core.Path.EntityPath.Parse("//Waves/1");
        }

        [TestMethod]
        public void CheckWildcardPath()
        {
            Assert.IsNotNull(EntityPath);
        }
    }
}