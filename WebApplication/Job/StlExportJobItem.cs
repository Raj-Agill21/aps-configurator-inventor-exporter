using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication.Processing;

namespace WebApplication.Job
{
    internal class StlExportJobItem : JobItemBase
    {
        private readonly string _hash;
        private readonly LinkGenerator _linkGenerator;

        public StlExportJobItem(ILogger logger, string projectId, string hash, ProjectWork projectWork, LinkGenerator linkGenerator)
            : base(logger, projectId, projectWork)
        {
            _hash = hash;
            _linkGenerator = linkGenerator;
        }

        public override async Task ProcessJobAsync(IResultSender resultSender)
        {
            using var scope = Logger.BeginScope($"STL generation ({Id})");
            Logger.LogInformation($"ProcessJob (STL) {Id} for project {ProjectId} started.");

            (var stats, var reportUrl) = await ProjectWork.GenerateStlAsync(ProjectId, _hash);
            Logger.LogInformation($"ProcessJob (STL) {Id} for project {ProjectId} completed.");

            string stlUrl = _linkGenerator.GetPathByAction(controller: "Download",
                                                            action: "STL",
                                                            values: new { projectName = ProjectId, hash = _hash });

            await resultSender.SendSuccessAsync(stlUrl, stats, reportUrl);
        }
    }
}

