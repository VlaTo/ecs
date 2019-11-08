/*using System.Linq;
using ClassLibrary1;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestProject1.Components;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var entity = new Entity("entity");

            entity.Add(new TestComponent());

            using (var temp = entity.Subscribe<TestComponent>("//"))
            {
                foreach (var component in temp)
                {
                    ;
                }
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var root = new Entity("root");
            var parent = new Entity("parent");
            var child1 = new Entity("child1");
            var child2 = new Entity("child2");

            root.Children.Add(parent);
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            child1.Add(new TestComponent());

            using (var temp = child1.Subscribe<TestComponent>("//parent/*"))
            {
                foreach (var component in temp)
                {
                    ;
                }
            }
        }


        [TestMethod]
        public void TestMethod3()
        {
            var root = new Entity("root");
            var parent = new Entity("parent");
            var child1 = new Entity("child1");
            var child2 = new Entity("child2");

            root.Children.Add(parent);
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            child1.Add(new TestComponent());

            using (var observer = child1.Subscribe<TestComponent>("//parent/*"))
            {
                Assert.AreEqual(1, observer.Count());

                child2.Add(new TestComponent());

                Assert.AreEqual(2, observer.Count());
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            var root = new Entity("root");
            var parent = new Entity("parent");
            var child1 = new Entity("child1");
            var child2 = new Entity("child2");

            root.Children.Add(parent);
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            child1.Add(new TestComponent());

            using (var observer = child1.Subscribe<TestComponent>("//parent/*"))
            {
                Assert.AreEqual(1, observer.Count());

                var testComponent = new TestComponent();

                child2.Add(testComponent);

                Assert.AreEqual(2, observer.Count());

                child2.Remove(testComponent);

                Assert.AreEqual(1, observer.Count());
            }
        }
    }
}*/