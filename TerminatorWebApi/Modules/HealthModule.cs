using Nancy;

namespace TerminatorWebApi.Modules
{
    public class HealthModule : NancyModule
    {
        public HealthModule()
        {
            Get["/api/health"] = p => HttpStatusCode.OK;
        }
    }
}
