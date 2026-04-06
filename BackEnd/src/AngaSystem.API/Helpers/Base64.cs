using System;
using System.Linq;
using System.Net.Http;

namespace AngaSystem.API.Helpers
{
    public class Base64
    {
        private static readonly HttpClient clientDownload = new HttpClient();
        public static string Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch
            {
                throw new Exception("erro na conversão");
            }
        }

        public static string Decode(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return string.Empty;
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch
            {
                throw new Exception("erro na conversão");
            }
        }

        public static string Criptografar(string textoPlano)
        {
            return Encode(Encode(Encode(textoPlano)));
        }

        public static string Descriptografar(string textoCriptografado)
        {
            return Decode(Decode(Decode(textoCriptografado)));
        }
        public static string ObterBase64FromUrl(string url)
        {
            var bytes = clientDownload.GetByteArrayAsync(url);

            if (bytes.Result != null && bytes.Result.Count() > 0)
            {
                var base64String = Convert.ToBase64String(bytes.Result);

                return base64String;
            }

            return null;
        }
    }
}
