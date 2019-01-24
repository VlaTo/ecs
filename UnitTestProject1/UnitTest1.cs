using System;
using System.Collections.Generic;
using ClassLibrary1;
using ClassLibrary1.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            var entity = Entity.CreateEntity("entity");
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
            var entity = Entity.CreateEntity("entity");
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
            var root = Entity.CreateEntity("root");
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

            var entity = Entity.CreateEntity("entity");

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
            var root = Entity.CreateEntity("root");
            var testComponent = new TestComponent();

            mock.Setup(observer => observer.OnAdded(testComponent));
            mock.Setup(observer => observer.OnRemoved(testComponent));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, true))
            {
                var entity = Entity.CreateEntity("entity");

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
            var root = Entity.CreateEntity("root");

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, recursive:true))
            {
                root.Children.Add(Entity.CreateEntity("entity"));
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
            var root = Entity.CreateEntity("root");

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, recursive))
            {
                var child = Entity.CreateEntity("entity");
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
        [DataRow("")]
        [DataRow("")]
        public void TestMethod7(string path)
        {
            var mock = new Mock<IEntityObserver<TestComponent>>();
            var root = Entity.CreateEntity("root");

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(path, mock.Object))
            {
                var child = Entity.CreateEntity("entity");
                var component = new TestComponent();

                child.Add(component);
                root.Children.Add(child);

                child.Remove(component);
            }

            //mock.Verify(observer => observer.OnAdded(It.IsAny<TestComponent>()), Times.Exactly(added));
            //mock.Verify(observer => observer.OnRemoved(It.IsAny<TestComponent>()), Times.Exactly(removed));
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var root = Entity.CreateEntity("root");
            var child = Entity.CreateEntity("entity");

            root.Children.Add(child);

            Assert.AreEqual(child.Path, new EntityPathString("/root/entity"));
        }
    }
}