using Microsoft.AspNetCore.Mvc;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Topics;

[Route("{__tenant__}/api/sessions/{sessionId}/[controller]/")]
public class TopicsControllerBase : ApiControllerBase;
