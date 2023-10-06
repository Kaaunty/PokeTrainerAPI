using Gdk;
using Gtk;
using PokeApiNet;
using System.Net;
using ImageMagick;
using ImageMagickSharp;
using Task = System.Threading.Tasks.Task;
using GLib;
using System.Diagnostics;
using System;

namespace PokeApi.BackEnd.Service
{
    public class ApiRequest
    {
#nullable disable
        private readonly PokeApiClient pokeClient = new PokeApiClient();

        public static class PokeList
        {
            public static List<Pokemon> pokemonList = new List<Pokemon>();
        }

        public async Task<Pokemon> GetPokemonAsync(string name)
        {
            Pokemon pokemon = await pokeClient.GetResourceAsync<Pokemon>(name);

            return pokemon;
        }

        public async Task<List<Pokemon>> GetPokemonsListAll()
        {
            bool hasEmptyResults;
            int currentPage = 0;
            try
            {
                do
                {
                    int totalpokemoncount = 200;
                    var page = await pokeClient.GetNamedResourcePageAsync<Pokemon>(totalpokemoncount, currentPage);
                    var tasks = page.Results.Select(result => GetPokemonAsync(result.Name)).ToList();
                    hasEmptyResults = tasks.Count == 0;

                    if (!hasEmptyResults)
                    {
                        await Task.WhenAll(tasks);
                        PokeList.pokemonList.AddRange(tasks.Select(task => task.Result));
                        currentPage += 200;
                    }
                } while (!hasEmptyResults);
                return PokeList.pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonListByTypePure(int currentpage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = PokeList.pokemonList;

                pokemonList = pokemonList.Where(pokemon => pokemon.Types.TrueForAll(type => type.Slot == 1) && pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = pokemonList.Skip(currentpage).Take(49).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonListByTypeAll(int currentpage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = PokeList.pokemonList;

                pokemonList = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = pokemonList.Skip(currentpage).Take(49).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Pokemon> GetPokemonListByTypeHalfType(int currentpage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = PokeList.pokemonList;

                pokemonList = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype) && pokemon.Types.Any(type => type.Slot == 1)).ToList();
                pokemonList = pokemonList.Skip(currentpage).Take(49).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Pokemon> GetPokemonlistByHalfTypeSecondary(int currentPage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = PokeList.pokemonList;

                pokemonList = pokemonList.Where(pokemon => pokemon.Types.Count >= 2 && pokemon.Types[1].Type.Name == lowercasetype).ToList();
                pokemonList = pokemonList.Skip(currentPage).Take(49).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pixbuf LoadPokemonSprite(int id)
        {
            var poke = PokeList.pokemonList.FirstOrDefault(pokemon => pokemon.Id == id);

            string url = poke.Sprites.FrontDefault;
            if (url == null)
            {
                url = poke.Sprites.Versions.GenerationVIII.Icons.FrontDefault;
            }

            try
            {
                HttpClient httpClient = new HttpClient();
                var loader = new PixbufLoader();

                var result = httpClient.GetByteArrayAsync(url).Result;
                loader.Write(result);

                loader.Close();
                return loader.Pixbuf;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetPokemonAnimatedSprite(string pokemonName)
        {
            string imageUrl = $"https://play.pokemonshowdown.com/sprites/xyani/{pokemonName.ToLower()}.gif";
            string pastaDestino = "Images";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    byte[] gifBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    if (!Directory.Exists(pastaDestino))
                    {
                        Directory.CreateDirectory(pastaDestino);
                    }

                    string nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimated.gif");

                    File.WriteAllBytes(nomeArquivo, gifBytes);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double GetProgress()
        {
            double progress = 0.0;
            int totalpokemoncount = PokeList.pokemonList.Count;

            if (totalpokemoncount == 200)
            {
                progress = 0.2;
                return progress;
            }

            return 0.0;
        }
    }
}