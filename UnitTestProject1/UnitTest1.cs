using System;
using ClassLibrary1;
using ClassLibrary1.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnitTestProject1.Components;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mock = new Mock<IEntityObserver>();
            var entity = new EntityImplementation("entity");
            var testComponent = new TestComponent();

            mock.Setup(observer => observer.OnAdded(testComponent));
            mock.Setup(observer => observer.OnCompleted());

            Assert.IsNotNull(entity);
            Assert.IsInstanceOfType(entity, typeof(EntityImplementation));

            entity.Add(testComponent);

            using (var subscription = entity.Subscribe(mock.Object))
            {
                Assert.IsNotNull(subscription);
            }

            mock.Verify(observer => observer.OnAdded(testComponent), Times.Once);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mock = new Mock<IEntityObserver>();
            var entity = new EntityImplementation("entity");
            var testComponent = new TestComponent();

            mock.Setup(observer => observer.OnAdded(testComponent));
            mock.Setup(observer => observer.OnCompleted());

            Assert.IsNotNull(entity);
            Assert.IsInstanceOfType(entity, typeof(EntityImplementation));

            using (var subscription = entity.Subscribe(mock.Object))
            {
                Assert.IsNotNull(subscription);
                entity.Add(testComponent);
            }

            mock.Verify(observer => observer.OnAdded(testComponent), Times.Once);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var mock = new Mock<IEntityObserver<TestComponent>>();
            var root = new EntityImplementation("root");
            var testComponent = new TestComponent();
            var components = new List<TestComponent>();

            mock
                .Setup(observer => observer.OnAdded(testComponent))
                .Callback<TestComponent>(component =>
                {
                    components.Add(component);
                });

            mock
                .Setup(observer => observer.OnRemoved(testComponent))
                .Callback<TestComponent>(component =>
                {
                    components.Remove(component);
                });

            mock
                .Setup(observer => observer.OnCompleted())
                .Callback(() => { });

            var entity = new EntityImplementation("entity");

            entity.Add(testComponent);
            root.Children.Add(entity);

            using (root.Subscribe(mock.Object, true))
            {

                entity.Remove(testComponent);
            }

            mock.Verify(observer => observer.OnAdded(testComponent), Times.Once);
            mock.Verify(observer => observer.OnRemoved(testComponent), Times.Once);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var mock = new Mock<IEntityObserver<TestComponent>>();
            var root = new EntityImplementation("root");
            var testComponent = new TestComponent();

            mock.Setup(observer => observer.OnAdded(testComponent));
            mock.Setup(observer => observer.OnRemoved(testComponent));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, true))
            {
                var entity = new EntityImplementation("entity");

                entity.Add(testComponent);
                root.Children.Add(entity);
                entity.Remove(testComponent);
            }

            mock.Verify(observer => observer.OnAdded(testComponent), Times.Once);
            mock.Verify(observer => observer.OnRemoved(testComponent), Times.Once);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var mock = new Mock<IEntityObserver<TestComponent>>();
            var root = new EntityImplementation("root");

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, recursive:true))
            {
                root.Children.Add(new EntityImplementation("entity"));
            }

            mock.Verify(observer => observer.OnAdded(It.IsAny<TestComponent>()), Times.Never);
            mock.Verify(observer => observer.OnRemoved(It.IsAny<TestComponent>()), Times.Never);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [DataTestMethod]
        [DataRow(true, 1, 1, DisplayName = "Recursive")]
        [DataRow(false, 0, 0, DisplayName = "Local")]
        public void TestMethod6(bool recursive, int added, int removed)
        {
            var mock = new Mock<IEntityObserver<TestComponent>>();
            var root = new EntityImplementation("root");

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, recursive))
            {
                var child = new EntityImplementation("child");
                var component = new TestComponent();

                child.Add(component);
                root.Children.Add(child);

                child.Remove(component);
            }

            mock.Verify(observer => observer.OnAdded(It.IsAny<TestComponent>()), Times.Exactly(added));
            mock.Verify(observer => observer.OnRemoved(It.IsAny<TestComponent>()), Times.Exactly(removed));
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [DataTestMethod]
        [DataRow("//root/*")]
        [DataRow("//root/child")]
        public void TestMethod7(string path)
        {
            var mock = new Mock<IEntityObserver<TestComponent>>();
            var root = new EntityImplementation("root");
            var child = new EntityImplementation("child");
            var component = new TestComponent();

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(path, mock.Object))
            {
                root.Children.Add(child);
                child.Add(component);
                child.Remove(component);
            }

            mock.Verify(observer => observer.OnAdded(component), Times.Once);
            mock.Verify(observer => observer.OnRemoved(component), Times.Once);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var root = new EntityImplementation("root");
            var child = new EntityImplementation("child");
            var pathString = EntityPathString.Parse("//root/child");

            root.Children.Add(child);

            Assert.AreEqual(child.Path, pathString);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var root = new EntityImplementation("root");
            var child = new EntityImplementation("child");

            root.Children.Add(child);
            child.Add(new TestComponent());

            var state = root.GetState();

            Assert.IsNotNull(state);
        }

        [TestMethod]
        public void TestMethod10()
        {
            var expected = "//root/entity";
            var pathString = EntityPathString.Parse(expected);

            Assert.AreEqual(expected, (string) pathString);
        }

        [TestMethod]
        public void TestMethod11()
        {
            var expected = "//root/entity*";
            var pathString = EntityPathString.Parse(expected);

            Assert.AreEqual(expected, (string) pathString);
        }

        [TestMethod]
        public void TestMethod12()
        {
            var root = new EntityImplementation("root");
            var child1 = new EntityImplementation("child1");
            var child2 = new EntityImplementation("child2");

            root.Children.Add(child1);
            child1.Children.Add(child2);
            root.Children.Add(new EntityReference("child3", child2));

            child1.Add(new TestComponent());
            child2.Add(new TestComponent
            {
                TestProperty =
                {
                    Value = 10
                }
            });

            root.Add(new TestComponent
            {
                TestProperty =
                {
                    Value = 20
                }
            });

            var state = root.GetState();
            var text = new StringBuilder();

            using (var writer = XmlWriter.Create(text))
            {
                var serializer = new XmlSerializer(typeof(EntityState), new[]
                {
                    typeof(ComponentState)
                });

                serializer.Serialize(writer, state);
            }

            Console.WriteLine(text.ToString());

            Assert.IsNotNull(state);
        }
    }
}