using System;
using Nancy;
using MachineInformationApp.Interfaces;
using Nancy.ModelBinding;
using Nancy.Extensions;
using Newtonsoft.Json;

namespace TerminatorWebApi
{
    public class DashboardHistoryModule : NancyModule
    {
        private IDashboardDataService _dashboardDataService;

        public DashboardHistoryModule(IDashboardDataService dashboardDataService)
        {
            this._dashboardDataService = dashboardDataService;
            Post["/api/dashboardActivity"] = _ =>
            {
                var x = this.Request.Body.AsString();
                var dashboardExecution = JsonConvert.DeserializeObject<DashboardExecution>(x);
                var scriptQuery = this.Bind<ScriptQuery>();


                if (dashboardExecution == null)
                {
                    return HttpStatusCode.BadRequest;
                }
                dashboardExecution.ExecutionTime = DateTime.UtcNow;
                _dashboardDataService.GetInsertDetails(dashboardExecution);
                return HttpStatusCode.OK;
            };

            Get["/api/dashboardActivity"] = p =>
           {
               var getData = _dashboardDataService.GetDashboardDetails();
               return Negotiate
               .WithContentType("aplication/json")
               .WithModel(getData);
           };
        }
    }
}
