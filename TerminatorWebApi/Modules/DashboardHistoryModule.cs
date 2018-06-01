using System;
using Nancy;
using MachineInformationApp.Interfaces;
using Nancy.ModelBinding;
namespace TerminatorWebApi
{
    public class DashboardHistoryModule : NancyModule
    {
        private IDashboardDataService _dashboardDataService;

        public DashboardHistoryModule(IDashboardDataService dashboardDataService)
        {
            this._dashboardDataService = dashboardDataService;
            Post["/api/dashboardActivity"] = p =>
            {
                DashboardExecution dashboardExecution = this.Bind<DashboardExecution>();
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
