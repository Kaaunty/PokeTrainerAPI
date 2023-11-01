using Newtonsoft.Json;
using PokeApi.BackEnd.Entities;
using System.Web;

namespace PokeApi.BackEnd.Service
{
    public class GoogleTranslationApi : ITranslationAPI
    {
        public async Task<String> Translate(string input)
        {
            var fromLanguage = "en";
            var toLanguage = "pt-BR";
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(url);

            try
            {
                var jsonData = JsonConvert.DeserializeObject<dynamic>(result);
                var translation = jsonData[0][0][0].ToString();

                string removebreaklines = translation.Replace("\n", " ");
                translation = removebreaklines.Replace(". ", ".");
                return translation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string TranslateType(string input)
        {
            var fromLanguage = "en";
            var toLanguage = "pt-BR";
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetStringAsync(url).Result;
            try
            {
                var jsonData = JsonConvert.DeserializeObject<dynamic>(result);
                var translation = jsonData[0][0][0].ToString();

                string removebreaklines = translation.Replace("\n", " ");
                translation = removebreaklines.Replace(". ", ".");
                if (translation == "Chão")
                {
                    translation = "Terra";
                }
                else if (translation == "Aço")
                {
                    translation = "Metal";
                }
                else if (translation == "Brigando")
                {
                    translation = "Lutador";
                }
                else if (translation == "Escuro")
                {
                    translation = "Noturno";
                }
                else if (translation == "Vôo")
                {
                    translation = "Voador";
                }
                else if (translation == "Bug")
                {
                    translation = "Inseto";
                }
                return translation;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}