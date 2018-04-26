using Nancy;

namespace TerminatorWebApi
{
    public class TerminatorApiModule : NancyModule
    {
        public TerminatorApiModule()
        {
            Get["/health"] = _ => Negotiate
                .WithStatusCode(HttpStatusCode.OK)
                .WithModel(new {Name="Lizo",Age=12});
        }
    }
}