using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace RioScaffolding.OpenSpacePlanner.Common.Workers;

public abstract class PersistenceMigrationWorkerBase : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    protected static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}
