using Autodesk.Forge.DesignAutomation.Model;
using WebApplication.Definitions;

namespace WebApplication.Processing
{
    public class ExportSTL : ForgeAppBase
    {
        public override string Id => nameof(ExportSTL);
        public override string Description => "Export Inventor document to STL format";
        protected override string OutputUrl(ProcessingArgs projectData) => projectData.StlUrl;
        protected override string OutputName => "output.stl";
        protected internal override ForgeRegistration Registration { get; } = ForgeRegistration.All;
        public ExportSTL(Publisher publisher) : base(publisher) {}
    }
}

