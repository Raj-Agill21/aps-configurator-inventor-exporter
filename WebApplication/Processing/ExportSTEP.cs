using Autodesk.Forge.DesignAutomation.Model;
using WebApplication.Definitions;

namespace WebApplication.Processing
{
    public class ExportSTEP : ForgeAppBase
    {
        public override string Id => nameof(ExportSTEP);
        public override string Description => "Export Inventor document to STEP format";
        protected override string OutputUrl(ProcessingArgs projectData) => projectData.StepUrl;
        protected override string OutputName => "output.step";
        protected internal override ForgeRegistration Registration { get; } = ForgeRegistration.All;
        public ExportSTEP(Publisher publisher) : base(publisher) {}
    }
}

