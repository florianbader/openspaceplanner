using RioScaffolding.OpenSpacePlanner.Common.Workers;
using RioScaffolding.OpenSpacePlanner.Persistence;

namespace RioScaffolding.OpenSpacePlanner.Workers.PersistenceMigrationWorker;

public class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    : PersistenceMigrationWorkerBase<TenantApplicationDbContext>(serviceProvider, hostApplicationLifetime) { }
