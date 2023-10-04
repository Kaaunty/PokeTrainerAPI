using Gdk;
using Gtk;
using PokeApiNet;
using System.Drawing;
using System.Net.Http;
using System.Security.Authentication;
using Task = System.Threading.Tasks.Task;

namespace PokeApi.BackEnd.Service
{
    public class ApiRequest
    {
        private PokeApiClient pokeClient = new PokeApiClient();

        public static class PokeList
        {
            public static List<Pokemon> pokemonList = new List<Pokemon>();
        }

        private async Task<Pokemon> GetPokemonAsync(string name)
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
                    int totalpokemoncount = 1500;
                    var page = await pokeClient.GetNamedResourcePageAsync<Pokemon>(totalpokemoncount, currentPage);
                    var tasks = page.Results.Select(result => GetPokemonAsync(result.Name)).ToArray();
                    hasEmptyResults = tasks.Length == 0;

                    if (!hasEmptyResults)
                    {
                        await Task.WhenAll(tasks);
                        PokeList.pokemonList.AddRange(tasks.Select(task => task.Result));
                        currentPage += 1500;
                    }
                } while (!hasEmptyResults);
                return PokeList.pokemonList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Pokemon>> GetPokemonListByTypePure(int currentpage, string type)
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
                return null;
            }
        }

        public async Task<List<Pokemon>> GetPokemonListByTypeAll(int currentpage, string type)
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

        public async Task<List<Pokemon>> GetPokemonListByTypeHalfType(int currentpage, string type)
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

        public Pixbuf LoadPokemonSprite(int id)
        {
            string url = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png";
            try
            {
                var loader = new PixbufLoader();
                loader.Write(new System.Net.WebClient().DownloadData(url));
                loader.Close();

                return loader.Pixbuf;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter o sprite do Pokémon: {ex.Message}" + id);
                return null;
            }
        }

        public async Task<byte[]> GetPokemonSpriteAsync(int id)
        {
            HttpClient _httpClient = new HttpClient();
            Dictionary<int, byte[]> _imageCache = new Dictionary<int, byte[]>();

            // Verifique se a imagem já está em cache
            if (_imageCache.TryGetValue(id, out var cachedImage))
            {
                return cachedImage;
            }

            string url = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    _imageCache[id] = imageBytes;
                    return imageBytes;
                }
                else
                {
                    Console.WriteLine("Pokémon não encontrado!");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter o sprite do Pokémon: {ex.Message}");
                return null;
            }
        }
    }
}