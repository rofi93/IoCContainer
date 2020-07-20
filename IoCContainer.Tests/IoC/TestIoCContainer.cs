using System.Composition.Hosting;
using System.Linq;
using IoCContainer.Tests.Models;
using NUnit.Framework;

namespace IoCContainer.Tests.IoC
{
    public class TestIoCContainer
    {
        [Test]
        public void TestNoExport()
        {
            var ex = Assert.Throws<CompositionFailedException>(() =>
                IoCContainer.IoC.IoCContainer.Instance.Resolve<ModelWithoutExport>());
            Assert.AreEqual("No export was found for the contract 'ModelWithoutExport'.", ex.Message);
        }

        [Test]
        public void TestExportWithoutContractName()
        {
            var model = IoCContainer.IoC.IoCContainer.Instance.Resolve<ModelWithoutContractName>();
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.Model);
        }

        [Test]
        public void TestModelWithContractName()
        {
            var model = IoCContainer.IoC.IoCContainer.Instance.Resolve<ModelWithContractName>("ModelWithContractName");
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.Model);
        }

        [Test]
        public void TestModelWithType()
        {
            var models = IoCContainer.IoC.IoCContainer.Instance.ResolveMany<IModel>().ToList();
            Assert.IsNotEmpty(models);
            Assert.AreEqual(2, models.Count);
        }

        [Test]
        public void TestModelWithTypeAndContractName()
        {
            var models = IoCContainer.IoC.IoCContainer.Instance
                .ResolveMany<ModelWithContractName>("ModelWithContractName").ToList();
            Assert.IsNotEmpty(models);
            Assert.AreEqual(1, models.Count);
        }
    }
}