using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BlazorCrud.Server.Controllers
{
    public class PowerBIController : Controller
    {
        private readonly IConfiguration _configuration;
        public PowerBIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        //public asyn Task<ActionResult<EmbeddedViewMode>>GetPowerBi()
        public async Task<ActionResult<string>> GetADToken()
        {
            var tenantId = _configuration["AzureAppInfo:TenantId"];
            var clientId = _configuration["AzureAppInfo:ClientId"];
            var clientSecret = _configuration["AzureAppInfo: ClientSecret"];
            var authorityUri = new Uri($"https://login.microsoftonline.com/{tenantId}");
            // this is from Microsoft.Identity.client
            var app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(authorityUri)
                .Build();
            var powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";
            var scopes = new string[] { powerbiApiDefaultScope };

            try
            {
                var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                var tokenCredentials = new TokenCredentials(authResult.AccessToken, "Bearer");
                var urlPowerBiServiceApiRoot = "https://api.powerbi.com/";
                // this we get from Microsoft.PowerBi.Api
                var pbiClient = new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
                var workspaceId = new Guid("");
                var reportId = new Guid("");
                var report = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

                var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View, report.DatasetId);
                var embedTokenResponse = await pbiClient.Reports.GenerateTokenAsync(workspaceId, report);

                var reportViewModel = new EmbeddedReportViewModel(
                    report.Id.ToString(),
                    report.Name,
                    report.EmbedUrl,
                    embedTokenResponse.Token
                    );
                return Ok(reportViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

    }

}

