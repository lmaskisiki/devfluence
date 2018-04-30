using Nancy;

namespace TerminatorWebApi
{
    public class HealthModule : NancyModule
    {
        public HealthModule()
        {
            Get["/api/health"] = p => HttpStatusCode.OK;
        }
    }
}
