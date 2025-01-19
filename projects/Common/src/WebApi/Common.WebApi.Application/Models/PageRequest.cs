namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Models;

public record class PageRequest(int? Page, int? PageSize = 10);
