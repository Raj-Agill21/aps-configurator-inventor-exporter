using Autodesk.Forge.DesignAutomation.Model;
using WebApplication.Definitions;

namespace WebApplication.Processing
{
    public class ExportIGES : ForgeAppBase
    {
        public override string Id => nameof(ExportIGES);
        public override string Description => "Export Inventor document to IGES format";
        protected override string OutputUrl(ProcessingArgs projectData) => projectData.IgesUrl;
        protected override string OutputName => "output.iges";
        protected internal override ForgeRegistration Registration { get; } = ForgeRegistration.All;
        public ExportIGES(Publisher publisher) : base(publisher) {}
    }
}

