using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Security
{
    public abstract class AbstractAutorizeService
    {
        public virtual Boolean CheckUser(String userName, String password)
        {
            return false;
        }

        public virtual Boolean CheckUserPermission(String userName, String password, String actionPermission)
        {
            return false;
        }

        public virtual string GetClientCredential()
        {
            return "";
        }
    }
}