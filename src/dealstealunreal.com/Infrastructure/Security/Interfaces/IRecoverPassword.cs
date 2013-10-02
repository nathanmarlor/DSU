using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dealstealunreal.com.Infrastructure.Security.Interfaces
{
    public interface IRecoverPassword
    {
        void ResetPassword(string userId);
    }
}