using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication.Processing;

namespace WebApplication.Job
{
    internal class IgesExportJobItem : JobItemBase
    {
        private readonly string _hash;
        private readonly LinkGenerator _linkGenerator;

        public IgesExportJobItem(ILogger logger, string projectId, string hash, ProjectWork projectWork, LinkGenerator linkGenerator)
            : base(logger, projectId, projectWork)
        {
            _hash = hash;
            _linkGenerator = linkGenerator;
        }

        public override async Task ProcessJobAsync(IResultSender resultSender)
        {
            using var scope = Logger.BeginScope($"IGES generation ({Id})");
            Logger.LogInformation($"ProcessJob (IGES) {Id} for project {ProjectId} started.");

            (var stats, var reportUrl) = await ProjectWork.GenerateIgesAsync(ProjectId, _hash);
            Logger.LogInformation($"ProcessJob (IGES) {Id} for project {ProjectId} completed.");

            string igesUrl = _linkGenerator.GetPathByAction(controller: "Download",
                                                            action: "IGES",
                                                            values: new { projectName = ProjectId, hash = _hash });

            await resultSender.SendSuccessAsync(igesUrl, stats, reportUrl);
        }
    }
}

