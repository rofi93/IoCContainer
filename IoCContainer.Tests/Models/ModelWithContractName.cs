using System.Composition;

namespace IoCContainer.Tests.Models
{
    [Export("ModelWithContractName", typeof(ModelWithContractName))]
    [Export(typeof(IModel))]
    public class ModelWithContractName : IModel
    {
        [Import] public ModelWithExport Model { get; set; }
    }
}