using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AngaSystem.API.Helpers
{
    public static class StringHelper
    {
        public static string RemoveAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            string textoNormalizado = texto.Normalize(NormalizationForm.FormD);
            var textoSemAcento = new StringBuilder();

            foreach (char c in textoNormalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    textoSemAcento.Append(c);
            }

            return textoSemAcento.ToString();
        }
    }
}