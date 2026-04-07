using AngaSystem.API.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using WebApi.Security;

namespace AngaSystem.API.Security
{
    public class AppSecurityService : AbstractAutorizeService
    {
        private readonly AppSettings _appSettings;
        private string userName;
        private string password;
        TipoAplicativo app;

        public AppSecurityService(TipoAplicativo app, AppSettings appSettings)
        {
            this.app = app;
            _appSettings = appSettings;
            this.userName = getUserName();
            this.password = getPassword();
        }

        public override bool CheckUser(string userName, string password)
        {
            return this.userName.Equals(userName) && this.password.Equals(password)/* || userName.Equals("pacheco") && password.Equals("gRX6BtWdDFWt")*/;
        }

        public override bool CheckUserPermission(string userName, string password, string actionPermission)
        {
            return BuscaPermissoes(userName, password, actionPermission);
        }

        public override string GetClientCredential()
        {
            string credential = this.userName + ":" + this.password;

            byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(credential);

            return Convert.ToBase64String(toEncodeAsBytes); ;
        }

        #region metodos para buscar as configurações por tipo de token
        /// <summary>
        /// Busca o usuário para validar de acordo com o tipo de aplicativo
        /// </summary>
        /// <returns></returns>
        private string getUserName()
        {
            string retorno = "";

            switch (this.app)
            {
                case TipoAplicativo.AppCliente:
                    retorno = "unimed";
                    if (!string.IsNullOrEmpty(_appSettings.TokenAppClienteUser))
                    {
                        retorno = _appSettings.TokenAppClienteUser;
                    }
                    break;
                case TipoAplicativo.AppCooperado:
                    retorno = "unimed";
                    if (!string.IsNullOrEmpty(_appSettings.TokenAppClienteUser))
                    {
                        retorno = _appSettings.TokenAppClienteUser;
                    }
                    break;
                default:
                    retorno = "";
                    break;
            }

            return retorno;
        }

        /// <summary>
        /// Busca a senha para validar de acordo com o tipo de aplicativo
        /// </summary>
        /// <returns></returns>
        private string getPassword()
        {
            string retorno = "";

            switch (this.app)
            {
                case TipoAplicativo.AppCliente:
                    retorno = "unimed";
                    if (!string.IsNullOrEmpty(_appSettings.TokenAppClientePwd))
                    {
                        retorno = _appSettings.TokenAppClientePwd;
                    }
                    break;
                case TipoAplicativo.AppCooperado:
                    retorno = "unimed";
                    if (!string.IsNullOrEmpty(_appSettings.TokenAppClientePwd))
                    {
                        retorno = _appSettings.TokenAppClientePwd;
                    }
                    break;
                default:
                    retorno = "";
                    break;
            }

            return retorno;
        }
        #endregion

        #region classe interna para simular a permissao da api para um usuario
        class Permissoes
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Action { get; set; }
        }

        private bool BuscaPermissoes(string userName, string password, string actionPermission)
        {
            //adiciona os itens fixos
            List<Permissoes> permissoes = new List<Permissoes>();
            permissoes = ListaPermissoes();

            bool retorno = false;

            retorno = permissoes.Any(x => x.UserName == userName && x.Password == password && (x.Action == actionPermission || x.Action == "*"));

            return retorno;
        }

        private List<Permissoes> ListaPermissoes()
        {
            List<Permissoes> permissoes = new List<Permissoes>
            {
                new Permissoes{UserName = getUserName(), Password = getPassword(), Action = "*"},
                new Permissoes{UserName = "infobip", Password = "F&ger0ERrz3P", Action = "BuscarCartaoV1.0"},
                new Permissoes{UserName = "infobip", Password = "F&ger0ERrz3P", Action = "BuscarPrestadoresEspecialidadesV1.0"},
                new Permissoes{UserName = "carefy", Password = "qJ)b;#,d2N3w", Action = "AutorizacaoBuscarProrrogacoesPorInternacaoV1.0"},
                new Permissoes{UserName = "drmobile", Password = "n+0Ne^K$;oE%", Action = "ConsultaDadosCooperadoV1.0"},
            };

            return permissoes;
        }
        #endregion

    }
}