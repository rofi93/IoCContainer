using System.Composition;

namespace IoCContainer.Tests.Models
{
    [Export(typeof(ModelWithoutContractName))]
    [Export(typeof(IModel))]
    public class ModelWithoutContractName : IModel
    {
        [Import] public ModelWithExport Model { get; set; }
    }
}