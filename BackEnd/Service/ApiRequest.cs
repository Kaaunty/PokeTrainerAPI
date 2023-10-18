using Gdk;
using Newtonsoft.Json;
using PokeApiNet;
using System.Web;
using Task = System.Threading.Tasks.Task;
using Type = PokeApiNet.Type;

namespace PokeApi.BackEnd.Service
{
    public class ApiRequest
    {
#nullable disable
        private readonly PokeApiClient pokeClient = new PokeApiClient();

        public static class PokeList
        {
            public static List<Pokemon> pokemonList = new List<Pokemon>();
            public static List<Move> pokemonMoves = new List<Move>();
        }

        public async Task<Pokemon> GetPokemonAsync(string name)
        {
            Pokemon pokemon = await pokeClient.GetResourceAsync<Pokemon>(name);

            return pokemon;
        }

        public async Task<Type> GetTypeAsync(string name)
         {
            Type type = await pokeClient.GetResourceAsync<Type>(name);

            return type;
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

        public async Task<List<Move>> GetMoveLearnedByPokemon(Pokemon pokemon)
        {
            try
            {
                List<Move> allMoves = await pokeClient.GetResourceAsync(pokemon.Moves.Select(move => move.Move));
                return allMoves;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Move> GetPokemonMoveAsync(string name)
        {
            Move move = await pokeClient.GetResourceAsync<Move>(name);
            return move;
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
                pokemonList = pokemonList.Skip(currentpage).Take(25).ToList();
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
                pokemonList = pokemonList.Skip(currentPage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PokemonSpecies> GetPokemonSpecies(string pokemonName)
        {
            string lowerCasePokemonName = pokemonName.ToLower();
            try
            {
                PokemonSpecies pokemonSpecies = await pokeClient.GetResourceAsync<PokemonSpecies>(lowerCasePokemonName);
                return pokemonSpecies;
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
            if (pokemonName == "giratina-altered")
            {
                pokemonName = "giratina";
            }
            if (pokemonName == "deoxys-normal")
            {
                pokemonName = "deoxys";
            }
            if (pokemonName == "charizard-mega-x")
            {
                pokemonName = "charizard-megax";
            }
            if (pokemonName == "charizard-mega-y")
            {
                pokemonName = "charizard-megay";
            }

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

        public async Task GetPokemonShinyAnimatedSprite(string pokemonName)
        {
            if (pokemonName == "giratina-altered")
            {
                pokemonName = "giratina";
            }
            if (pokemonName == "deoxys-normal")
            {
                pokemonName = "deoxys";
            }
            if (pokemonName == "charizard-mega-x")
            {
                pokemonName = "charizard-megax";
            }
            if (pokemonName == "charizard-mega-y")
            {
                pokemonName = "charizard-megay";
            }

            string imageUrl = $"https://play.pokemonshowdown.com/sprites/ani-shiny/{pokemonName.ToLower()}.gif";
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

                    string nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimatedShiny.gif");

                    File.WriteAllBytes(nomeArquivo, gifBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar a imagem." + ex);
            }
        }

        public double GetProgress()
        {
            double progress = 0.0;
            int totalpokemoncount = PokeList.pokemonList.Count;

            if (totalpokemoncount == 0)
            {
                return progress;
            }
            if (totalpokemoncount > 0)
            {
                if (totalpokemoncount == 200)
                {
                    progress = 0.2;
                    return progress;
                }
                else if (totalpokemoncount == 400)
                {
                    progress = 0.4;
                    return progress;
                }
                else if (totalpokemoncount == 600)
                {
                    progress = 0.6;
                    return progress;
                }
                else if (totalpokemoncount == 800)
                {
                    progress = 0.8;
                    return progress;
                }
                else if (totalpokemoncount == 1000)
                {
                    progress = 0.85;
                    return progress;
                }
                else if (totalpokemoncount == 1200)
                {
                    progress = 0.9;
                    return progress;
                }
                else if (totalpokemoncount == 1292)
                {
                    progress = 1.0;
                    return progress;
                }
            }
            else
            {
                progress = 1.0;
                return progress;
            }
            return progress;
        }

        public async Task<EvolutionChain> GetEvolutionChain(string nextEvolution)
        {
            try
            {
                if (!string.IsNullOrEmpty(nextEvolution))
                {
                    string[] urlParts = nextEvolution.Split('/');
                    if (urlParts.Length >= 2)
                    {
                        if (int.TryParse(urlParts[urlParts.Length - 2], out int evolutionChainId))
                        {
                            var evolutionChain = await pokeClient.GetResourceAsync<EvolutionChain>(evolutionChainId);
                            return evolutionChain;
                        }
                        else
                        {
                            Console.WriteLine("Invalid evolution chain ID.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid URL format.");
                    }
                }
                else
                {
                    Console.WriteLine("nextEvolution is empty.");
                }

                return null;
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
                PokemonForm pokemonForm = await pokeClient.GetResourceAsync<PokemonForm>(name);
                return pokemonForm;
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
                Ability ability = await pokeClient.GetResourceAsync<Ability>(abilityName);
                if (ability != null)
                {
                    return ability;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public String Translate(string input)
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
                if (translation.Contains("brigando") || translation.Contains("Luta"))
                {
                    translation = translation.Replace("brigando", "lutador");
                    translation = translation.Replace("Luta", "Lutador");
                }
                if (translation.Contains("Solo") || translation.Contains("Chão"))
                {
                    translation = translation.Replace("Solo", "Terra");
                    translation = translation.Replace("Chão", "Terra");
                }
                if (translation.Contains("Bug"))
                {
                    translation = translation.Replace("Bug", "Inseto");
                }

                return translation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<String> TranslateLists(List<string> input)
        {
            var fromLanguage = "en";
            var toLanguage = "pt-BR";
            List<String> translatedList = new();
            foreach (var i in input)
            {
                var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(i.ToString())}";
                HttpClient httpClient = new HttpClient();
                var result = httpClient.GetStringAsync(url).Result;
                try
                {
                    var jsonData = JsonConvert.DeserializeObject<dynamic>(result);
                    var translation = jsonData[0][0][0].ToString();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return translatedList;
        }
    }
}