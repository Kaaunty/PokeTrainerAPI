using Gdk;
using Newtonsoft.Json;
using PokeApiNet;
using System.Net;
using System.Web;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;
using Type = PokeApiNet.Type;

namespace PokeApi.BackEnd.Service
{
    public class ApiRequest : IApiRequests
    {
#nullable disable

        private readonly PokeApiClient _pokeClient = new PokeApiClient();
        private HttpClient _httpClient = new HttpClient();
        private HttpClientHandler _handler = new HttpClientHandler();

        public static class PokeList
        {
            public static List<Pokemon> pokemonList = new List<Pokemon>();
            public static List<Pokemon> pokemonListAll = new List<Pokemon>();
            public static List<Pokemon> pokemonListPureType = new List<Pokemon>();
            public static List<Pokemon> pokemonListHalfType = new List<Pokemon>();
            public static List<Pokemon> pokemonListHalfSecundaryType = new List<Pokemon>();
            public static List<Move> pokemonMoves = new List<Move>();
            public static Dictionary<int, Pixbuf> _pokemonImageCache = new Dictionary<int, Pixbuf>();
            public static Dictionary<string, string> TypeDamageRelations = new();
            public static List<Move> pokemonMoveList = new List<Move>();
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
                _httpClient = new HttpClient(_handler);

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
                throw;
            }
        }

        public async Task<Type> GetTypeAsync(string name)
        {
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

        public async Task<List<Move>> GetMoveLearnedByPokemon(Pokemon pokemon)
        {
            try
            {
                List<Move> allMoves = await _pokeClient.GetResourceAsync(pokemon.Moves.Select(move => move.Move));
                return allMoves;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Pokemon>> GetPokemonsListAll()
        {
            _handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, errors) => { return true; };
            _httpClient = new HttpClient(_handler);
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

                PokeList.pokemonListAll = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = PokeList.pokemonListAll.Skip(currentpage).Take(25).ToList();
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

                PokeList.pokemonListHalfSecundaryType = pokemonList.Where(pokemon => pokemon.Types.Count >= 2 && pokemon.Types[1].Type.Name == lowercasetype).ToList();
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

        public async Task<Pixbuf> LoadPokemonSprite(int id)
        {
            string pokemonName = PokeList.pokemonList.FirstOrDefault(pokemon => pokemon.Id == id).Name;
            try
            {
                if (PokeList._pokemonImageCache.ContainsKey(id))
                {
                    return PokeList._pokemonImageCache[id];
                }

                var poke = PokeList.pokemonList.FirstOrDefault(pokemon => pokemon.Id == id);

                string url = poke.Sprites.FrontDefault;
                url ??= poke.Sprites.Versions.GenerationVIII.Icons.FrontDefault;
                url ??= poke.Sprites.Versions.GenerationVII.Icons.FrontDefault;
                url ??= $"https://play.pokemonshowdown.com/sprites/gen5/{pokemonName}.png";
                if (url == "")
                {
                    if (pokemonName.Contains("koraidon"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/koraidon.png";
                    }
                    else if (pokemonName.Contains("-totem") && pokemonName != "kommo-o-totem")
                    {
                        url = $"https://play.pokemonshowdown.com/sprites/gen5/{pokemonName.Replace("-totem", "")}.png";
                    }
                    else if (pokemonName.Contains("kommo-o-totem"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/kommoo.png";
                    }
                    else if (pokemonName == "pikachu-world-cap")
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/pikachu-world.png";
                    }
                    else if (pokemonName.Contains("miraidon"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/miraidon.png";
                    }
                    else if (pokemonName.Contains("koraidon"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/koraidon.png";
                    }
                    else if (pokemonName.Contains("ogerpon-wellspring-mask"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/ogerpon-wellspring.png";
                    }
                    else if (pokemonName == "mimikyu-totem-disguised")
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/mimikyu.png";
                    }
                }

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.IsSuccessStatusCode)
                {
                    var loader = new PixbufLoader();
                    response.EnsureSuccessStatusCode();

                    var result = await _httpClient.GetByteArrayAsync(url);

                    if (result != null)
                    {
                        loader.Write(result);
                        loader.Close();
                        var pokemonImage = loader.Pixbuf;
                        PokeList._pokemonImageCache[id] = pokemonImage;
                        return pokemonImage;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar a imagem." + ex + "Pokemon:" + pokemonName);
                return null;
            }
        }

        public async Task GetPokemonAnimatedSprite(string pokemonName, bool shiny)
        {
            #region if's

            if (pokemonName == "giratina-altered")
            {
                pokemonName = "giratina";
            }
            else if (pokemonName.Contains("mega-y"))
            {
                pokemonName = pokemonName.Replace("mega-y", "megay");
            }
            else if (pokemonName.Contains("mega-x"))
            {
                pokemonName = pokemonName.Replace("mega-x", "megax");
            }
            else if (pokemonName.Contains("-natural"))
            {
                pokemonName = pokemonName.Replace("-natural", "");
            }
            else if (pokemonName.Contains("-normal"))
            {
                pokemonName = pokemonName.Replace("-normal", "");
            }
            else if (pokemonName.Contains("amped-gmax"))
            {
                pokemonName = pokemonName.Replace("amped-gmax", "gmax");
            }
            else if (pokemonName.Contains("-low-key"))
            {
                pokemonName = pokemonName.Replace("-low-key", "-lowkey");
            }
            else if (pokemonName.Contains("toxtricity-amped"))
            {
                pokemonName = pokemonName.Replace("toxtricity-amped", "toxtricity");
            }
            else if (pokemonName.Contains("furfrou-la-reine"))
            {
                pokemonName = pokemonName.Replace("la-reine", "lareine");
            }
            else if (pokemonName.Contains("necrozma-dawn"))
            {
                pokemonName = pokemonName.Replace("dawn", "dawnwings");
            }
            else if (pokemonName.Contains("necrozma-dusk"))
            {
                pokemonName = pokemonName.Replace("dusk", "duskmane");
            }
            else if (pokemonName.Contains("oinkologne-female"))
            {
                pokemonName = pokemonName.Replace("-female", "-f");
            }
            else if (pokemonName.Contains("plumage"))
            {
                pokemonName = pokemonName.Replace("-plumage", "");
            }
            else if (pokemonName.Contains("family-of-three"))
            {
                pokemonName = pokemonName.Replace("family-of-three", "four");
            }
            else if (pokemonName.Contains("disguised"))
            {
                pokemonName = pokemonName.Replace("-disguised", "");
            }
            else if (pokemonName.Contains("totem-busted"))
            {
                pokemonName = pokemonName.Replace("totem-busted", "busted-totem");
            }
            else if (pokemonName.Contains("nidoran-m"))
            {
                pokemonName = pokemonName.Replace("-m", "");
            }

            #endregion if's

            string imageUrl = "";

            if (shiny)
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/ani-shiny/{pokemonName.ToLower()}.gif";
            }
            else
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/xyani/{pokemonName.ToLower()}.gif";
            }

            string pastaDestino = "Images";
            string nomeArquivo = "";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    byte[] gifBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    if (!Directory.Exists(pastaDestino))
                    {
                        Directory.CreateDirectory(pastaDestino);
                    }
                    if (shiny)
                    {
                        nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimatedShiny.gif");
                    }
                    else
                    {
                        nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimated.gif");
                    }

                    File.WriteAllBytes(nomeArquivo, gifBytes);
                }
            }
            catch (Exception)
            {
                await GetPokemonStaticSprite(pokemonName, shiny);
            }
        }

        public async Task GetPokemonStaticSprite(string pokemonName, bool shiny)
        {
            #region if's name validation

            string imageUrl = "";

            if (pokemonName == "giratina-altered")
            {
                pokemonName = "giratina";
            }
            if (pokemonName.Contains("mega-y"))
            {
                pokemonName = pokemonName.Replace("mega-y", "megay");
            }
            if (pokemonName.Contains("mega-x"))
            {
                pokemonName = pokemonName.Replace("mega-x", "megax");
            }
            if (pokemonName.Contains("-natural"))
            {
                pokemonName = pokemonName.Replace("-natural", "");
            }
            if (pokemonName.Contains("-normal"))
            {
                pokemonName = pokemonName.Replace("-normal", "");
            }
            if (pokemonName.Contains("koraidon"))
            {
                pokemonName = pokemonName.Replace(pokemonName, "koraidon");
            }
            if (pokemonName.Contains("miraidon"))
            {
                pokemonName = pokemonName.Replace(pokemonName, "miraidon");
            }
            if (pokemonName == "tauros-paldea-combat-breed")
            {
                pokemonName = "tauros-paldeacombat";
            }
            if (pokemonName == "tauros-paldea-blaze-breed")
            {
                pokemonName = "tauros-paldeablaze";
            }
            if (pokemonName == "tauros-paldea-aqua-breed")
            {
                pokemonName = "tauros-paldeaaqua";
            }
            if (pokemonName == "roaring-moon")
            {
                pokemonName = "roaringmoon";
            }

            #endregion if's name validation

            if (shiny)
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5/{pokemonName.ToLower()}.png";
            }
            else
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5-shiny/{pokemonName.ToLower()}.png";
            }

            string pastaDestino = "Images";
            string nomeArquivo = "";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    byte[] gifBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    if (!Directory.Exists(pastaDestino))
                    {
                        Directory.CreateDirectory(pastaDestino);
                    }
                    if (shiny)
                    {
                        nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimatedShiny.gif");
                    }
                    else
                    {
                        nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimated.gif");
                    }
                    File.WriteAllBytes(nomeArquivo, gifBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar a imagem." + ex);
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
                //PokemonForm pokemonForm = await pokeClient.GetResourceAsync<PokemonForm>(name);
                //return pokemonForm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Ability> GetPokemonAbility(string abilityName)
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
                    Ability ability = JsonConvert.DeserializeObject<Ability>(json);
                    return ability;
                }
                return null;
                //Ability ability = await pokeClient.GetResourceAsync<Ability>(abilityName);
                //return ability;
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                return translation;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}