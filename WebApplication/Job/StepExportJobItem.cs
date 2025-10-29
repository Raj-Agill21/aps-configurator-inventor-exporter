using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication.Processing;

namespace WebApplication.Job
{
    internal class StepExportJobItem : JobItemBase
    {
        private readonly string _hash;
        private readonly LinkGenerator _linkGenerator;

        public StepExportJobItem(ILogger logger, string projectId, string hash, ProjectWork projectWork, LinkGenerator linkGenerator)
            : base(logger, projectId, projectWork)
        {
            _hash = hash;
            _linkGenerator = linkGenerator;
        }

        public override async Task ProcessJobAsync(IResultSender resultSender)
        {
            using var scope = Logger.BeginScope($"STEP generation ({Id})");
            Logger.LogInformation($"ProcessJob (STEP) {Id} for project {ProjectId} started.");

            (var stats, var reportUrl) = await ProjectWork.GenerateStepAsync(ProjectId, _hash);
            Logger.LogInformation($"ProcessJob (STEP) {Id} for project {ProjectId} completed.");

            string stepUrl = _linkGenerator.GetPathByAction(controller: "Download",
                                                            action: "STEP",
                                                            values: new { projectName = ProjectId, hash = _hash });

            await resultSender.SendSuccessAsync(stepUrl, stats, reportUrl);
        }
    }
}

