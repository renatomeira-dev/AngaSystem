using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace AngaSystem.API.Helpers
{
    public class Constantes
    {
        //public static string ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        public static class Mensagens
        {
            public static class Comuns
            {
                public const string ErroNaoTratado = "Ocorreu um erro não tratado ao processar sua requisição";
                public const string ErroWsV1_0Indisponivel = "Webservice do módulo de V1.0 indisponível";
            }
        }
    }
}
