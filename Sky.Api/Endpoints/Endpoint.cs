namespace Sky.Api.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("");

            endpoints.MapGroup("v1/users")
                .WithTags("Usuários")
                .MapEndpoint<Users.Register>()
                .MapEndpoint<Users.Login>()
                .MapEndpoint<Users.RefreshToken>();

            endpoints.MapGroup("v1/courses")
                .WithTags("Cursos")
                .MapEndpoint<Courses.Create>()
                .MapEndpoint<Courses.GetAll>();
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
           where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }       
    }
    public interface IEndpoint
    {
        static abstract void Map(IEndpointRouteBuilder app);
    }
}
