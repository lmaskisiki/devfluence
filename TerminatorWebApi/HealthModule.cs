using Nancy;

namespace TerminatorWebApi
{
    public class HealthModule : NancyModule
    {
        public HealthModule()
        {
            Get["/health"] = p => HttpStatusCode.OK;
        }
    }
}
