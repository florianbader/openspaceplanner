using Microsoft.AspNetCore.Mvc;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;

[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("{__tenant__}/api/[controller]/")]
[Consumes("application/json")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase;
