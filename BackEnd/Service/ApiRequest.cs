using GLib;
using Newtonsoft.Json;
using PokeApi.BackEnd.Entities;
using PokeApiNet;
using System.Drawing;
using System.Net;

namespace PokeApi.BackEnd.Service
{
    public class ApiRequest
    {
        private readonly PokeApiClient pokeApiClient = new PokeApiClient();

        private int current = 0;

        public Pokemon? GetPokemon(string pokename)
        {
            var client = new HttpClient();
            Pokemon? pokemon;
            string lowercase = pokename.ToLower();
            string url = "https://pokeapi.co/api/v2/pokemon/" + lowercase;
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = client.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                pokemon = JsonConvert.DeserializeObject<Pokemon>(json);
                return pokemon;
            }
            else
            {
                Console.WriteLine("Pokemon não encontrado!.");
                return null;
            }
        }

        public List<Pokemon> GetPokeList()
        {
            var client = new HttpClient();
            PokeList pokeList = new PokeList();
            List<Pokemon> result = new List<Pokemon>();
            string url = "https://pokeapi.co/api/v2/pokemon?limit=2&offset=" + current;
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = client.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                pokeList = JsonConvert.DeserializeObject<PokeList>(json);
                foreach (var pokemon in pokeList.result)
                {
                    var poke = GetPokemon(pokemon.Name);
                    result.Add(poke);
                }
                current += 1;
                return result;
            }
            else
            {
                Console.WriteLine("Pokemon não encontrado!.");
                return null;
            }
        }

        public byte[] GetPokemonSprite(int id)
        {
            using (var client = new HttpClient())
            {
                string url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + id + ".png";
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    Console.WriteLine("Pokémon não encontrado!");
                    return null;
                }
            }
        }
    }
}