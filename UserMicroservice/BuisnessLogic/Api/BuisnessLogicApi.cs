using BuisnessLogic.Handlers;
using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Api.Exceptions;
using BuisnessLogic.Models;
using Microsoft.Extensions.DependencyInjection;


namespace BuisnessLogic.Api
{
    public class BuisnessLogicApi(IServiceProvider serviceProvider) : IBuisnessLogicApi
    {
        public async Task<ResponseUserModel> Create(RequestUserModel source)
        {
            try
            {
                var requestHandler = serviceProvider.GetRequiredService<CreateRequestHandler>();

                return await requestHandler.Handle(source);
            }
            catch (UserAlreadyExistsException)
            {
                throw new UserAlreadyExistsApiException();
            }
        }

        public async Task<ResponseUserModel> Update(RequestUserModel source)
        {
            try
            {
                var requestHandler = serviceProvider.GetRequiredService<UpdateRequestHandler>();

                return await requestHandler.Handle(source);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
        }

        public async Task<ResponseUserModel> Delete(Guid id)
        {
            try
            {
                var requestHandler = serviceProvider.GetRequiredService<DeleteRequestHandler>();

                return await requestHandler.Handle(id);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
        }

        public async Task<ResponseUserModel> Delete(string userName)
        {
            try
            {
                var requestHandler = serviceProvider.GetRequiredService<DeleteRequestHandler>();

                return await requestHandler.Handle(userName);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
        }

        public ResponseUserModel Get(Guid id)
        {
            try
            {
                var requestHandler = serviceProvider.GetRequiredService<GetRequestHandler>();

                return requestHandler.Handle(id);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
        }

        public ResponseUserModel Get(string userName)
        {
            try
            {
                var requestHandler = serviceProvider.GetRequiredService<GetRequestHandler>();

                return requestHandler.Handle(userName);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
        }

        public BoolResponseModel UserExists(Guid userId)
        {
            var requestHandler = serviceProvider.GetRequiredService<UserExistsRequestHandler>();

            return requestHandler.Handle(userId);
        }

        public BoolResponseModel UserExists(string userName)
        {
            var requestHandler = serviceProvider.GetRequiredService<UserExistsRequestHandler>();

            return requestHandler.Handle(userName);
        }
    }
}
