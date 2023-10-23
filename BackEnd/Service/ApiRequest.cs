using Gtk;

using Task = System.Threading.Tasks.Task;
using Type = PokeApiNet.Type;
using Newtonsoft.Json;
using PokeApiNet;
using System.Web;
using Gdk;

namespace PokeApi.BackEnd.Service
{
    public class ApiRequest
    {
#nullable disable

        private readonly PokeApiClient pokeClient = new PokeApiClient();
        private Move move;

        public static class PokeList
        {
            public static List<Pokemon> pokemonList = new List<Pokemon>();

            public static List<Move> pokemonMoves = new List<Move>();
            public static Dictionary<int, Pixbuf> _pokemonImageCache = new Dictionary<int, Pixbuf>();
            public static Dictionary<string, string> TypeDamageRelations = new();
            public static List<Move> pokemonMoveList = new List<Move>();
        }

        public async Task<Pokemon> GetPokemonAsync(string name)
        {
            try
            {
                Pokemon pokemon = await pokeClient.GetResourceAsync<Pokemon>(name);

                return pokemon;
            }
            catch (Exception)
            {
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
            Type type = await pokeClient.GetResourceAsync<Type>(name);
            return type;
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

                pokemonList = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = pokemonList.Skip(currentpage).Take(25).ToList();
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
                if (url == null)
                {
                    url = poke.Sprites.Versions.GenerationVIII.Icons.FrontDefault;
                }
                if (url == null)
                {
                    url = poke.Sprites.Versions.GenerationVII.Icons.FrontDefault;
                }
                if (pokemonName.Contains("ogerpon"))
                {
                    if (pokemonName.Contains("wellspring-mask"))
                    {
                        url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/1017.png";
                    }
                    else if (pokemonName.Contains("hearthflame-mask"))
                    {
                        url = "https://archives.bulbagarden.net/media/upload/e/eb/HOME1017H.png";
                    }
                    else if (pokemonName.Contains("cornerstone-mask"))
                    {
                        url = "https://archives.bulbagarden.net/media/upload/9/92/HOME1017W.png";
                    }
                }

                try
                {
                    HttpClient httpClient = new HttpClient();
                    var loader = new PixbufLoader();

                    var result = await httpClient.GetByteArrayAsync(url);
                    loader.Write(result);

                    loader.Close();
                    var pokemonImage = loader.Pixbuf;
                    PokeList._pokemonImageCache[id] = pokemonImage;

                    return pokemonImage;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar a imagem." + ex + "Pokemon:" + pokemonName);
                return null;
            }
        }

        public async Task GetPokemonAnimatedSprite(string pokemonName)
        {
            string imageUrl;
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

            imageUrl = $"https://play.pokemonshowdown.com/sprites/xyani/{pokemonName.ToLower()}.gif";

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

        public async Task<MoveLearnMethod> GetMoveLearnMethod(string moveName)
        {
            try
            {
                MoveLearnMethod moveLearnMethod = await pokeClient.GetResourceAsync<MoveLearnMethod>(moveName);
                if (moveLearnMethod != null)
                {
                    return moveLearnMethod;
                }
                return null;
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

        public void PopulateTypeDamageRelationDictionary()
        {
            PokeList.TypeDamageRelations.Add("normal", "Dano Sofrido Pouco Efetivo: Nenhum\nPouco Efetivo Contra: Rocha, Aço\nDano Sofrido Super Efetivo: Lutador\nSuper Efetivo Contra: Nenhum\nImune: Fantasma\nNenhum Dano a: Fantasma");
            PokeList.TypeDamageRelations.Add("dark", "Dano Sofrido Pouco Efetivo: Fantasma, Sombrio\nPouco Efetivo Contra: Lutador, Sombrio, Fada\nDano Sofrido Super Efetivo: Lutador, Inseto, Fada\nSuper Efetivo Contra: Fantasma, Psíquico\nImune: Psíquico\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("bug", "Dano Sofrido Pouco Efetivo: Lutador, Terra, Grama\nPouco Efetivo Contra: Lutador, Voador, Venenoso, Fantasma, Aço, Fogo, Fada\nDano Sofrido Super Efetivo: Voador, Pedra, Fogo\nSuper Efetivo Contra: Grama, Psíquico, Sombrio\nImune: Nenhum\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("dragon", "Dano Sofrido Pouco Efetivo: Fogo, Água, Grama, Elétrica\nPouco Efetivo Contra: Aço\nDano Sofrido Super Efetivo: Gelo, Dragão, Fada\nSuper Efetivo Contra: Dragão\nImune: Nenhum\nNenhum Dano a: Fada");
            PokeList.TypeDamageRelations.Add("electric", "Dano Sofrido Pouco Efetivo: Voador, Aço, elétrico\nPouco Efetivo Contra: Grama, Elétrica, Dragão\nDano Sofrido Super Efetivo: Terra\nSuper Efetivo Contra: Voador, Água\nImune: Nenhum\nNenhum Dano a: Terra");
            PokeList.TypeDamageRelations.Add("fighting", "Dano Sofrido Pouco Efetivo: Rocha, Inseto, Sombrio\nPouco Efetivo Contra: Voador, Venenososo, Inseto, Psíquico, Fada\nDano Sofrido Super Efetivo: Voador, Psíquico, Fada\nSuper Efetivo Contra: Normal, Pedra, Aço, Gelo, Sombrio\nImune: Nenhum\nNenhum Dano a: Fantasma");
            PokeList.TypeDamageRelations.Add("fire", "Dano Sofrido Pouco Efetivo: Inseto, Aço, Fogo, Grama, Gelo, Fada\nPouco Efetivo Contra: Pedra, Fogo, Água, Dragão\nDano Sofrido Super Efetivo: Terra, Pedra, Água\nSuper Efetivo Contra: Inseto, aço, grama, gelo\nImune: Nenhum\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("flying", "Dano Sofrido Pouco Efetivo: Lutador, Inseto, Grama\nPouco Efetivo Contra: Rocha, aço, elétrica\nDano Sofrido Super Efetivo: Rock, elétrico, gelo\nSuper Efetivo Contra: Lutador, Inseto, Grama\nImune: Terra\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("ghost", "Dano Sofrido Pouco Efetivo: Venenososo, Inseto\nPouco Efetivo Contra: Sombrio\nDano Sofrido Super Efetivo: Fantasma, Sombrio\nSuper Efetivo Contra: Fantasma, Psíquico\nImune: Normal, Lutador\nNenhum Dano a: Normal");
            PokeList.TypeDamageRelations.Add("grass", "Dano Sofrido Pouco Efetivo: Terra, Água, Grama, Elétrica\nPouco Efetivo Contra: Voador, Venenososo, Inseto, Aço, Fogo, Grama, Dragão\nDano Sofrido Super Efetivo: Voador, Venenososo, Inseto, Fogo, Gelo\nSuper Efetivo Contra: Terra, Pedra, Água\nImune: Nenhum\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("ground", "Dano Sofrido Pouco Efetivo: Venenososo, Pedra\nPouco Efetivo Contra: Inseto, grama\nDano Sofrido Super Efetivo: Água, grama, gelo\nSuper Efetivo Contra: Venenoso, Pedra, Aço, Fogo, Elétrico\nImune: Elétrico\nNenhum Dano a: Voador");
            PokeList.TypeDamageRelations.Add("ice", "Dano Sofrido Pouco Efetivo: Gelo\nPouco Efetivo Contra: Aço, Fogo, Água, Gelo\nDano Sofrido Super Efetivo: Lutador, Pedra, Aço, Fogo\nSuper Efetivo Contra: Voador, Terra, Grama, Dragão\nImune: Nenhum\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("poison", "Dano Sofrido Pouco Efetivo: Lutador, Psíquico\nPouco Efetivo Contra: Aço, Psíquico\nDano Sofrido Super Efetivo: Inseto, Fantasma, Sombrio\nSuper Efetivo Contra: Lutador, Venenoso\nImune: Nenhum\nNenhum Dano a: Sombrio");
            PokeList.TypeDamageRelations.Add("psychic", "Dano Sofrido Pouco Efetivo: Lutador, Psíquico\nPouco Efetivo Contra: Aço, Psíquico\nDano Sofrido Super Efetivo: Inseto, Fantasma, Sombrio\nSuper Efetivo Contra: Lutador, Venenoso\nImune: Nenhum\nNenhum Dano a: Sombrio");
            PokeList.TypeDamageRelations.Add("rock", "Dano Sofrido Pouco Efetivo: Normal, Voador, Venenoso, Fogo\nPouco Efetivo Contra: Lutador,Terra,Aço\nDano Sofrido Super Efetivo: Lutador, Terra, Aço, Água, Grama\nSuper Efetivo Contra: Voador, Inseto, Fogo, Gelo\nImune: Nenhum\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("steel", "Dano Sofrido Pouco Efetivo: Normal, Voador, Pedra, Inseto, Aço, Grama, Psíquico, Gelo, Dragão, Fada\nPouco Efetivo Contra: Aço, Fogo, Água, Elétrica\nDano Sofrido Super Efetivo: Lutador,Terra,Fogo\nSuper Efetivo Contra: Pedra, Gelo, Fada\nImune: Tóxico\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("water", "Dano Sofrido Pouco Efetivo: Aço, Fogo, Água, Gelo\nPouco Efetivo Contra: Água, Grama, Dragão\nDano Sofrido Super Efetivo: Grama, elétrica\nSuper Efetivo Contra: Terra, Pedra, Fogo\nImune: Nenhum\nNenhum Dano a: Nenhum");
            PokeList.TypeDamageRelations.Add("fairy", "Dano Sofrido Pouco Efetivo: Lutador, Inseto, Sombrio\nPouco Efetivo Contra: Venenoso, Aço, Fogo\nDano Sofrido Super Efetivo: Venenoso, Aço\nSuper Efetivo Contra: Lutador, Dragão, Sombrio\nImune: Dragão\nNenhum Dano a: Nenhum");
        }

        public string GetTypeDamageRelation(string type)
        {
            return PokeList.TypeDamageRelations[type];
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

        public async Task<MoveLearnMethod> GetMoveLearnMethodAsync(string activeText)
        {
            MoveLearnMethod moveLearnMethod = await pokeClient.GetResourceAsync<MoveLearnMethod>(activeText);
            return moveLearnMethod;
        }

        //public List<String> TranslateLists(List<string> input)
        //{
        //    var fromLanguage = "en";
        //    var toLanguage = "pt-BR";
        //    List<String> translatedList = new();
        //    foreach (var i in input)
        //    {
        //        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(i.ToString())}";
        //        HttpClient httpClient = new HttpClient();
        //        var result = httpClient.GetStringAsync(url).Result;
        //        try
        //        {
        //            var jsonData = JsonConvert.DeserializeObject<dynamic>(result);
        //            var translation = jsonData[0][0][0].ToString();
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    return translatedList;
        //}
    }
}