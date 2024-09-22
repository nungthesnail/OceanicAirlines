using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Api.Exceptions
{
    public class UserDoesntExistsApiException : ApiException
    {
        public UserDoesntExistsApiException(string? message = null)
            : base(message)
        { }
    }
}
