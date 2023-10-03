using Gdk;
using PokeApiNet;
using System.Drawing;
using Task = System.Threading.Tasks.Task;

namespace PokeApi.BackEnd.Service
{
    public class ApiRequest
    {
        private HttpClient client;
        private List<Pokemon> pokemons;

        public async Task<List<Pokemon>> LoadPokemonListAsync(int currentPage)
        {
            pokemons = new List<Pokemon>();
            var pokeClient = new PokeApiClient();

            int totalPokemonCount = 49;

            var page = await pokeClient.GetNamedResourcePageAsync<Pokemon>(totalPokemonCount, currentPage);
            var tasks = page.Results.Select(result => GetPokemonAsync(result.Name)).ToArray();
            await Task.WhenAll(tasks);
            pokemons.AddRange(tasks.Select(task => task.Result));

            return pokemons;
        }

        private async Task<Pokemon> GetPokemonAsync(string name)
        {
            var pokeClient = new PokeApiClient();
            return await pokeClient.GetResourceAsync<Pokemon>(name);
        }

        public async Task<byte[]> GetPokemonSprite(int id)
        {
            PokeApiClient pokeClient = new PokeApiClient();
            Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(id);
            return await GetPokemonSpriteAsync(poke.Id);
        }

        private readonly HttpClient _httpClient;
        private readonly Dictionary<int, byte[]> _imageCache;

        public ApiRequest()
        {
            _httpClient = new HttpClient();
            _imageCache = new Dictionary<int, byte[]>();
        }

        public async Task<byte[]> GetPokemonSpriteAsync(int id)
        {
            if (_imageCache.TryGetValue(id, out var cachedImage))
            {
                return cachedImage;
            }

            string url = pokemons[id].Sprites.FrontDefault;

            try
            {
                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    _imageCache[id] = imageBytes; // Armazena a imagem no cache.
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


        public async Task<List<byte[]>> GetPokemonImagesAsync(List<string> pokemonNames)
        {
            var client = new PokeApiClient();
            List<byte[]> pokemonImages = new List<byte[]>();

            foreach (var name in pokemonNames)
            {
                Pokemon pokemon = await client.GetResourceAsync<Pokemon>(name);
                string imageUrl = pokemon.Sprites.FrontDefault;

                using (HttpClient httpClient = new HttpClient())
                {
                    byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                    pokemonImages.Add(imageBytes);
                }
            }

            return pokemonImages;
        }

    }
}