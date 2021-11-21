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
            string[] data = request.Attributes.Split("-");
            string log = request.Name + "-" + data[0] + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.Add(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Creo juego";
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> UpdateGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string[] data = request.Attributes.Split("-");
            string log = request.Name + "-" + data[0] + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.Update(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Actualizo juego";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> BuyGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + request.Attributes + "-" + DateTime.Today.ToString() + "-"; 
            try
            {
                response.Message = Logic.Buy(request.Attributes, request.Name);
                response.Status = ProtocolMethods.Success;
                log += "Compro juego";
               
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> EvaluateGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string[] data = request.Attributes.Split("-");
            string log = request.Name + "-" + data[0] + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.Evaluate(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Evaluo juego";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> Show(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + request.Attributes + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.Show(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Mostro juego";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> Search(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + "All" + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.Search(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Busco juego";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }


        public override Task<Response> ShowAllGames(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + "All" + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.GetAll();
                response.Status = ProtocolMethods.Success;
                log += "Vio todos los juegos";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> ReviewsGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + request.Attributes + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.GetReviews(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Vio las reviews de un juego";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> DeleteGame(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + request.Attributes + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.Delete(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Elimino juego";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
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

        public override Task<Response> BoughtGames(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + "All" + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.GetListBought(request.Name);
                response.Status = ProtocolMethods.Success;
                log += "Vio lista de juegos comprados";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<ResponseClient> Login(Request request, ServerCallContext context)
        {
            ResponseClient response = new ResponseClient();
            string[] data = request.Attributes.Split("-");
            string log = request.Name + "-" + data[0] + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Name = Logic.Login(request.Attributes).name;
                response.Status = ProtocolMethods.Success;
                response.Message = "Bienvenido al sistema nuevamente";
                log += "Se logueo en el sistema";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<ResponseClient> Register(Request request, ServerCallContext context)
        {
            ResponseClient response = new ResponseClient();
            string[] data = request.Attributes.Split("-");
            string log = request.Name + "-" + data[0] + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Name = Logic.Register(request.Attributes).name;
                response.Status = ProtocolMethods.Success;
                response.Message = "Se creo un nuevo usuario";
                log += "Se registro en el sistema";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> LogOut(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + request.Attributes + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.LogOut(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Salio del sistema";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> ShowAllUsers(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + "All" + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.ShowAllUsers();
                response.Status = ProtocolMethods.Success;
                log += "Vio todos los usuarios";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }

        public override Task<Response> DeleteUser(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string[] data = request.Attributes.Split("-");
            string log = request.Name + "-" + data[0] + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.DeleteUser(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Elimino un usuario";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
            return Task.FromResult(response);
        }
        public override Task<Response> UpdateUser(Request request, ServerCallContext context)
        {
            Response response = new Response();
            string log = request.Name + "-" + "All" + "-" + DateTime.Today.ToString() + "-";
            try
            {
                response.Message = Logic.UpdateUser(request.Attributes);
                response.Status = ProtocolMethods.Success;
                log += "Actualizo un usuario";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = ProtocolMethods.Error;
                log += "Error: " + e.Message;
            }
            LogConnection.PublishMessage(log);
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