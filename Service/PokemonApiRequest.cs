using Gdk;
using Newtonsoft.Json;
using PokeApi.BackEnd.Entities;
using PokeApiNet;
using System.Net;
using System.Xml.Linq;
using Task = System.Threading.Tasks.Task;
using Type = PokeApiNet.Type;

namespace PokeApi.BackEnd.Service
{
    public class PokemonApiRequest : IPokemonAPI
    {
#nullable disable

        private readonly PokeApiClient _pokeClient = new PokeApiClient();
        private readonly HttpClient _httpClient = new HttpClient();

        public static class PokeList
        {
            public static List<Pokemon> pokemonList = new List<Pokemon>();
            public static List<Pokemon> pokemonListAllType = new List<Pokemon>();
            public static List<Pokemon> pokemonListPureType = new List<Pokemon>();
            public static List<Pokemon> pokemonListHalfType = new List<Pokemon>();
            public static List<Pokemon> pokemonListHalfSecundaryType = new List<Pokemon>();
            public static Dictionary<int, Pixbuf> pokemonImageCache = new Dictionary<int, Pixbuf>();
            public static Dictionary<string, string> typeDamageRelations = new();
        }

        public async Task<double> GetPokemonTotalCount()
        {
            try
            {
                string url = "https://pokeapi.co/api/v2/pokemon/";
                var response = await _httpClient.GetAsync(url);
                var test = response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var pokemonList = JsonConvert.DeserializeObject<PokeApiNet.NamedApiResourceList<Pokemon>>(json);
                    return pokemonList.Count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Pokemon> GetPokemon(string name)
        {
            _pokeClient.ClearCache();
            _pokeClient.ClearResourceListCache();

            if (name == "mimikyu")
            {
                name = "mimikyu-disguised";
            }
            string url = $"https://pokeapi.co/api/v2/pokemon/{name}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(1000);
                    return await GetPokemon(name);
                }
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Pokemon pokemon = JsonConvert.DeserializeObject<Pokemon>(json);
                    return pokemon;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar o pokemon." + ex);
                return await GetPokemon(name);
                throw;
            }
        }

        public Pokemon GetPokemonByName(string pokemonName)
        {
            if (pokemonName == "mimikyu")
            {
                pokemonName = "mimikyu-disguised";
            }
            var pokemon = PokeList.pokemonList.Find(pokemon => pokemon.Name == pokemonName);
            return pokemon;
        }

        public async Task<Type> GetTypeAsync(string name)
        {
            #region if's

            if (name.Contains("fairy"))
            {
                name = "fairy";
            }
            else if (name.Contains("fighting"))
            {
                name = "fighting";
            }
            else if (name.Contains("normal"))
            {
                name = "normal";
            }
            else if (name.Contains("flying"))
            {
                name = "flying";
            }
            else if (name.Contains("poison"))
            {
                name = "poison";
            }
            else if (name.Contains("ground"))
            {
                name = "ground";
            }
            else if (name.Contains("rock"))
            {
                name = "rock";
            }
            else if (name.Contains("bug"))
            {
                name = "bug";
            }
            else if (name.Contains("ghost"))
            {
                name = "ghost";
            }
            else if (name.Contains("steel"))
            {
                name = "steel";
            }
            else if (name.Contains("fire"))
            {
                name = "fire";
            }
            else if (name.Contains("water"))
            {
                name = "water";
            }
            else if (name.Contains("grass"))
            {
                name = "grass";
            }
            else if (name.Contains("electric"))
            {
                name = "electric";
            }
            else if (name.Contains("psychic"))
            {
                name = "psychic";
            }
            else if (name.Contains("ice"))
            {
                name = "ice";
            }
            else if (name.Contains("dragon"))
            {
                name = "dragon";
            }
            else if (name.Contains("dark"))
            {
                name = "dark";
            }
            else if (name.Contains("fairy"))
            {
                name = "fairy";
            }
            else if (name.Contains("unknown"))
            {
                name = "unknown";
            }

            #endregion if's

            string url = $"https://pokeapi.co/api/v2/type/{name}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(1000);
                    return await GetTypeAsync(name);
                }
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Type type = JsonConvert.DeserializeObject<Type>(json);
                    return type;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PokeApiNet.Move>> GetMoveLearnedByPokemon(Pokemon pokemon)
        {
            try
            {
                List<PokeApiNet.Move> allMoves = await _pokeClient.GetResourceAsync(pokemon.Moves.Select(move => move.Move));
                return allMoves;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Pokemon>> LoadPokemonsListAll()
        {
            bool hasEmptyResults;
            int currentPage = 0;
            int maxRetries = 3;

            for (int retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    do
                    {
                        int totalpokemoncount = 200;
                        var page = await _pokeClient.GetNamedResourcePageAsync<Pokemon>(totalpokemoncount, currentPage);
                        var tasks = page.Results.Select(result => GetPokemon(result.Name)).ToList();
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
                catch (Exception ex)
                {
                    if (retry < maxRetries - 1)
                    {
                        Console.Write(ex.Message);
                        await Task.Delay(1000);
                    }
                }
            }

            throw new Exception("Falha após várias tentativas");
        }

        public List<Pokemon> GetPokemonListAll(int currentpage)
        {
            try
            {
                var pokemonList = PokeList.pokemonList;
                pokemonList = pokemonList.Skip(currentpage).Take(25).ToList();
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

                PokeList.pokemonListAllType = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = PokeList.pokemonListAllType.Skip(currentpage).Take(25).ToList();
                return pokemonList;
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

                PokeList.pokemonListPureType = pokemonList.Where(pokemon => pokemon.Types.TrueForAll(type => type.Slot == 1) && pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = PokeList.pokemonListPureType.Skip(currentpage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonListByTypeHalfType(int currentpage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = PokeList.pokemonList;

                PokeList.pokemonListHalfType = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype) && pokemon.Types.Any(type => type.Slot == 2)).ToList();
                pokemonList = PokeList.pokemonListHalfType.Skip(currentpage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonlistByHalfTypeSecondary(int currentPage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = PokeList.pokemonList;

                PokeList.pokemonListHalfSecundaryType = pokemonList.Where(pokemon => pokemon.Types.Count == 2 && pokemon.Types[1].Type.Name == lowercasetype).ToList();
                pokemonList = PokeList.pokemonListHalfSecundaryType.Skip(currentPage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PokemonSpecies> GetPokemonSpecies(string pokemonName)
        {
            try
            {
                string url = $"https://pokeapi.co/api/v2/pokemon-species/{pokemonName}";
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(1000);
                    return await GetPokemonSpecies(pokemonName);
                }
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    PokemonSpecies pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(json);
                    return pokemonSpecies;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EvolutionChain> GetEvolutionChain(string nextEvolution)
        {
            EvolutionChain evolutionChain = new();
            try
            {
                if (!string.IsNullOrEmpty(nextEvolution))
                {
                    string[] urlParts = nextEvolution.Split('/');
                    if (urlParts.Length >= 2)
                    {
                        if (int.TryParse(urlParts[urlParts.Length - 2], out int evolutionChainId))
                        {
                            string url = $"https://pokeapi.co/api/v2/evolution-chain/{evolutionChainId}";
                            var response = await _httpClient.GetAsync(url);
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                return null;
                            }
                            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                            {
                                await Task.Delay(1000);
                                return await GetEvolutionChain(nextEvolution);
                            }
                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                evolutionChain = JsonConvert.DeserializeObject<EvolutionChain>(json);
                                return evolutionChain;
                            }
                        }
                    }
                }

                return evolutionChain;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PokemonForm> GetPokemonForm(string name)
        {
            try
            {
                string url = $"https://pokeapi.co/api/v2/pokemon-form/{name}";
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(1000);
                    return await GetPokemonForm(name);
                }
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    PokemonForm pokemonForm = JsonConvert.DeserializeObject<PokemonForm>(json);
                    return pokemonForm;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PokeApiNet.Ability> GetPokemonAbility(string abilityName)
        {
            try
            {
                string url = $"https://pokeapi.co/api/v2/ability/{abilityName}";
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(1000);
                    return await GetPokemonAbility(abilityName);
                }
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    PokeApiNet.Ability ability = JsonConvert.DeserializeObject<PokeApiNet.Ability>(json);
                    return ability;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}