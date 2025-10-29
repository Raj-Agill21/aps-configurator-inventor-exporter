using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication.Processing;

namespace WebApplication.Job
{
    internal class DwgExportJobItem : JobItemBase
    {
        private readonly string _hash;
        private readonly LinkGenerator _linkGenerator;

        public DwgExportJobItem(ILogger logger, string projectId, string hash, ProjectWork projectWork, LinkGenerator linkGenerator)
            : base(logger, projectId, projectWork)
        {
            _hash = hash;
            _linkGenerator = linkGenerator;
        }

        public override async Task ProcessJobAsync(IResultSender resultSender)
        {
            using var scope = Logger.BeginScope($"DWG generation ({Id})");
            Logger.LogInformation($"ProcessJob (DWG) {Id} for project {ProjectId} started.");

            (var stats, var reportUrl) = await ProjectWork.GenerateDwgAsync(ProjectId, _hash);
            Logger.LogInformation($"ProcessJob (DWG) {Id} for project {ProjectId} completed.");

            string dwgUrl = _linkGenerator.GetPathByAction(controller: "Download",
                                                            action: "DWG",
                                                            values: new { projectName = ProjectId, hash = _hash });

            await resultSender.SendSuccessAsync(dwgUrl, stats, reportUrl);
        }
    }
}

