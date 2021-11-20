using Bussiness;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAdmin
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<Response> CreateGame(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Add(request.Attributes)
            });
            ;
        }

        public override Task<Response> UpdateGame(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Update(request.Attributes)
            });
            ;
        }

        public override Task<Response> BuyGame(RequestClient request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Buy(request.Attributes, request.Name)
            });
            ;
        }

        public override Task<Response> EvaluateGame(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Evaluate(request.Attributes)
            });
            ;
        }

        public override Task<Response> Show(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Show(request.Attributes)
            });
            ;
        }
        public override Task<Response> Search(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Search(request.Attributes)
            });
            ;
        }


        public override Task<Response> ShowAllGames(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.GetAll()
            });
            ;
        }

        public override Task<Response> ReviewsGame(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.GetReviews(request.Attributes)
            });
            ;
        }

        public override Task<Response> DeleteGame(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Delete(request.Attributes)
            });
            ;
        }

        public override Task<Response> ReceiveImage(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Add(request.Attributes)
            });
            ;
        }

        public override Task<Response> SendImage(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.Add(request.Attributes)
            });
            ;
        }

        public override Task<Response> BoughtGames(RequestClient request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.GetListBought(request.Name)
            });
            ;
        }

        public override Task<ResponseClient> Login(Request request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseClient
            {
                Name = Logic.Login(request.Attributes).name,
                Message = "Bienvenido al sistema nuevamente"


            });
            ;
        }

        public override Task<ResponseClient> Register(Request request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseClient
            {
                Name = Logic.Register(request.Attributes).name,
                Message = "Se creo un nuevo usuario"
            });
            ;
        }

        public override Task<Response> LogOut(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.LogOut(request.Attributes)
            });
            ;
        }

        public override Task<Response> ShowAllUsers(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.ShowAllUsers()
            });
            ;
        }

        public override Task<Response> DeleteUser(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.DeleteUser(request.Attributes)
            });
            ;
        }
        public override Task<Response> UpdateUser(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response
            {
                Message = Logic.UpdateUser(request.Attributes)
            });
            ;
        }

    }
}