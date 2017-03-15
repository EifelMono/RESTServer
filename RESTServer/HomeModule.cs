using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Swagger.Annotations.Attributes;
using Swagger.ObjectModel;

namespace RESTServer
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [Model("HOME")]
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            // Get("/", _ => Response.AsRedirect("/swagger-ui/dist/index.html"), null, "Home");

            Get("/users", _ => GetUsers(), null, "GetUsers");

            Post("/users", _ =>
            {
                var user = this.Bind<User>();
                return PostUser(user);
            });

            Post("/addusers", _ =>
            {
                var user = this.Bind<User>();
                return AddUser(user);
            });
        }

        public static List<User> Users = new List<User> { new User { Name = "Vincent Vega", Age = 45 } };
        [Route("GetUsers")]
        [Route(HttpMethod.Get, "/users")]
        [SwaggerResponse(HttpStatusCode.OK, Message = "OK", Model = typeof(IEnumerable<User>))]
        private IEnumerable<User> GetUsers()
        {
            return Users;
        }

        [Route(HttpMethod.Post, "/users")]
        [SwaggerResponse(HttpStatusCode.OK, Message = "OK", Model = typeof(User))]
        [SwaggerResponse(HttpStatusCode.NotFound, Message = "Error", Model = null)]
        [Route(Produces = new[] { "application/json" })]
        [Route(Consumes = new[] { "application/json" })]
        private User PostUser([RouteParam(ParameterIn.Body)] User user)
        {
            return user;
        }

        [Route(HttpMethod.Post, "/addusers")]
        [SwaggerResponse(HttpStatusCode.OK, Message = "OK", Model = typeof(bool))]
        [Route(Produces = new[] { "application/json" })]
        [Route(Consumes = new[] { "application/json" })]
        private bool AddUser([RouteParam(ParameterIn.Body)] User user)
        {
            Users.Add(user);
            return true;
        }
    }
}
