/*using ClassLibrary1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core.Reactive.Collections;
using LibraProgramming.Ecs.Extensions;
using UnitTestProject1.Components;
using EntityPath = LibraProgramming.Ecs.Core.Path.EntityPath;
using IComponent = LibraProgramming.Ecs.IComponent;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mock = new Mock<ICollectionObserver<IComponent>>();
            var entity = new Entity("entity");
            var testComponent = new TestComponent();

            mock.Setup(observer => observer.OnAdded(testComponent));
            mock.Setup(observer => observer.OnCompleted());

            Assert.IsNotNull(entity);
            Assert.IsInstanceOfType(entity, typeof(Entity));

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
            var mock = new Mock<ICollectionObserver<IComponent>>();
            var entity = new Entity("entity");
            var testComponent = new TestComponent();

            mock.Setup(observer => observer.OnAdded(testComponent));
            mock.Setup(observer => observer.OnCompleted());

            Assert.IsNotNull(entity);
            Assert.IsInstanceOfType(entity, typeof(Entity));

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
            var mock = new Mock<ICollectionObserver<IComponent>>();
            var root = new Entity("_");
            var testComponent = new TestComponent();
            var components = new List<IComponent>();

            mock
                .Setup(observer => observer.OnAdded(testComponent))
                .Callback<IComponent>(component => components.Add(component));

            mock
                .Setup(observer => observer.OnRemoved(testComponent))
                .Callback<IComponent>(component => components.Remove(component));

            mock
                .Setup(observer => observer.OnCompleted())
                .Callback(() => { });

            var child = new Entity("child");

            child.Add(testComponent);
            root.Children.Add(child);

            using (root.Subscribe(mock.Object, true))
            {
                child.Remove(testComponent);
            }

            Assert.AreEqual(0, components.Count);

            mock.Verify(observer => observer.OnAdded(testComponent), Times.Once);
            mock.Verify(observer => observer.OnRemoved(testComponent), Times.Once);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var mock = new Mock<ICollectionObserver<IComponent>>();
            var root = new Entity("_");
            var testComponent = new TestComponent();

            mock.Setup(observer => observer.OnAdded(testComponent));
            mock.Setup(observer => observer.OnRemoved(testComponent));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, true))
            {
                var child = new Entity("child");

                child.Add(testComponent);
                root.Children.Add(child);
                child.Remove(testComponent);
            }

            mock.Verify(observer => observer.OnAdded(testComponent), Times.Once);
            mock.Verify(observer => observer.OnRemoved(testComponent), Times.Once);
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var mock = new Mock<ICollectionObserver<IComponent>>();
            var root = new Entity("_");

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, recursive:true))
            {
                root.Children.Add(new Entity("child"));
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
            var mock = new Mock<ICollectionObserver<IComponent>>();
            var root = new Entity("_");

            mock.Setup(observer => observer.OnAdded(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnRemoved(It.IsAny<TestComponent>()));
            mock.Setup(observer => observer.OnCompleted());

            using (root.Subscribe(mock.Object, recursive))
            {
                var child = new Entity("child");
                var component = new TestComponent();

                child.Add(component);
                root.Children.Add(child);

                child.Remove(component);
            }

            mock.Verify(observer => observer.OnAdded(It.IsAny<TestComponent>()), Times.Exactly(added));
            mock.Verify(observer => observer.OnRemoved(It.IsAny<TestComponent>()), Times.Exactly(removed));
            mock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        /*[DataTestMethod]
        [DataRow("//*", DisplayName = "TestAllFromRoot")]
        [DataRow("//child", DisplayName = "TestSingleChild")]
        public void TestMethod7(string path)*/

        [DataRow("//*", DisplayName = "TestAllFromRoot")]
        [DataRow("//child", DisplayName = "TestSingleChild")]
        [DataTestMethod]
        public void TestMethod7(string path)
        {
            var mock = new Mock<ICollectionObserver<IComponent>>();
            var root = new Entity("_");
            var child = new Entity("child");
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
        public void TestMethod8_1()
        {
            var root = new Entity("_");
            var child = new Entity("child");
            var pathString = EntityPath.Parse("//child");

            root.Children.Add(child);

            Assert.AreEqual(child.Path, pathString);
        }

        [TestMethod]
        public void TestMethod8_2()
        {
            var root = new Entity("_");
            var parent = new Entity("parent");
            var child = new Entity("child");
            var pathString = EntityPath.Parse("//parent/child");

            root.Children.Add(parent);
            parent.Children.Add(child);

            Assert.AreEqual(pathString, child.Path);
        }

        [TestMethod]
        public void TestMethod8_3()
        {
            var root = new Entity("_");
            var pathString = EntityPath.Parse("//");
            Assert.AreEqual(pathString, root.Path);
        }

        [TestMethod]
        public void TestMethod8_4()
        {
            var root = new Entity("_");
            Assert.AreEqual("//", (string) root.Path);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var root = new Entity("_");
            var child = new Entity("child");

            root.Children.Add(child);
            child.Add(new TestComponent());

            var state = root.GetState();

            Assert.IsNotNull(state);
        }

        [TestMethod]
        public void TestMethod10()
        {
            const string expected = "//root/parent/entity";
            var pathString = EntityPath.Parse(expected);

            Assert.AreEqual(expected, (string) pathString);
        }

        [TestMethod]
        public void TestMethod11()
        {
            const string expected = "//root/fail*";
            Assert.ThrowsException<Exception>(() => EntityPath.Parse(expected));
        }

        [TestMethod]
        public void TestMethod11_1()
        {
            const string expected = "//root/*fail";
            Assert.ThrowsException<Exception>(() => EntityPath.Parse(expected));
        }

        [TestMethod]
        public void TestMethod12()
        {
            var root = new Entity("_");
            var parent = new Entity("parent");
            var child = new Entity("child");

            root.Children.Add(parent);
            parent.Children.Add(child);
            root.Children.Add(new ReferencedEntity("reference", child));

            parent.Add(new TestComponent());
            child.Add(new TestComponent());
            root.Add(new TestComponent());

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

        /*[TestMethod]
        public void TestMethod13()
        {
            //var converter = new EnumConverter(typeof(TestEnum));
            var converter = new TestEnumConverter();
            Debug.WriteLine(converter.ConvertToString(TestEnum.Value1 | TestEnum.Value3));
        }*/
    }

    /*[TypeConverter(typeof(TestEnumConverter))]
    [Flags]
    public enum TestEnum
    {
        [DisplayName("value-one")] Value1 = 1,
        [DisplayName("value-two")] Value2 = 2,
        Value3 = 4
    }

    public sealed class TestEnumConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return typeof(string) == destinationType || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (typeof(string) != destinationType)
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            var enumType = value.GetType();
            var values = Enum.GetValues(enumType);
            var str = new StringBuilder();

            foreach (var val in values)
            {
                var name = Enum.GetName(enumType, val);
                var field = enumType.GetField(name, BindingFlags.Static | BindingFlags.Public);
                var attribute = field.GetCustomAttribute<DisplayNameAttribute>();

                if (0 < str.Length)
                {
                    str.Append(culture.TextInfo.ListSeparator);
                }

                str.Append(attribute?.DisplayName ?? name);
            }

            return str.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return typeof(TestEnum) == sourceType || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {

            return base.ConvertFrom(context, culture, value);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return base.GetProperties(context, value, attributes);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return base.IsValid(context, value);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return base.CreateInstance(context, propertyValues);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return base.GetStandardValues(context);
        }
    }
}*/