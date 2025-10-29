using Autodesk.Forge.DesignAutomation.Model;
using WebApplication.Definitions;

namespace WebApplication.Processing
{
    public class ExportDWG : ForgeAppBase
    {
        public override string Id => nameof(ExportDWG);
        public override string Description => "Export Inventor document to DWG format";
        protected override string OutputUrl(ProcessingArgs projectData) => projectData.DwgUrl;
        protected override string OutputName => "output.dwg";
        protected internal override ForgeRegistration Registration { get; } = ForgeRegistration.All;
        public ExportDWG(Publisher publisher) : base(publisher) {}
    }
}

