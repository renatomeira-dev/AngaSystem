using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngaSystem.API.Helpers
{
    public static class Utils
    {
        ///// <summary>
        ///// Adiciona na URL uma api_key se ela existir na AbsoluteUri que a chamou
        ///// </summary>
        ///// <param name="url">url que será utilizada para chamar a API</param>
        ///// <param name="AbsoluteUri">AbsoluteUri que chamou o método que vai chamar a API</param>
        //public static void AjustaApiKey(ref string url, string AbsoluteUri)
        //{
        //    var apiKey = System.Configuration.ConfigurationManager.AppSettings["api_key"];
        //    if (AbsoluteUri.Contains($"api_key={apiKey}"))
        //    {
        //        if (url.Contains("?"))
        //        {
        //            url = url + $"&api_key={apiKey}";
        //        }
        //        else
        //        {
        //            url = url + $"?api_key={apiKey}";
        //        }
        //    }
        //}
    }
}