using Bussiness;
using Bussiness.Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Protocol;
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
            Response response = new Response();
            try
            {
                response.Message = Logic.Add(request.Attributes);
                response.Status = ProtocolMethods.Success;
             }
            catch(Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> UpdateGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.Update(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> BuyGame(RequestClient request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.Buy(request.Attributes, request.Name);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> EvaluateGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.Evaluate(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> Show(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.Show(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> Search(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.Search(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }


        public override Task<Response> ShowAllGames(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.GetAll();
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> ReviewsGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.GetReviews(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> DeleteGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.Delete(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
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
            Response response = new Response();
            try
            {
                response.Message = Logic.GetListBought(request.Name);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<ResponseClient> Login(Request request, ServerCallContext context)
        {
            ResponseClient response = new ResponseClient();
            try
            {
                response.Name = Logic.Login(request.Attributes).name;
                response.Status = ProtocolMethods.Success;
                response.Message = "Bienvenido al sistema nuevamente";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<ResponseClient> Register(Request request, ServerCallContext context)
        {
            ResponseClient response = new ResponseClient();
            try
            {
                response.Name = Logic.Register(request.Attributes).name;
                response.Status = ProtocolMethods.Success;
                response.Message = "Se creo un nuevo usuario";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> LogOut(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.LogOut(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> ShowAllUsers(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.ShowAllUsers();
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> DeleteUser(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.DeleteUser(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }
        public override Task<Response> UpdateUser(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                response.Message = Logic.UpdateUser(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

        public override Task<Response> UpdateRoute(Request request, ServerCallContext context)
        {
            Response response = new Response();
            try
            {
                Logic.UpdateRoute(request.Attributes);
                response.Status = ProtocolMethods.Success;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
            }
            return Task.FromResult(response);
        }

    }
}