using Newtonsoft.Json;
using PokeApi.BackEnd.Entities;
using PokeTrainerBackEnd;
using PokeTrainerBackEndTest.Entities;
using System.Net;

namespace PokeTrainerBackEndTest.Controller
{
    public class PokeApiNetController : IPokemonAPI
    {
#nullable disable

        private HttpClient client = new();

        public async Task<PokemonResponse> GetPokemon(int pokemonId)
        {
            try
            {
                PokemonResponse pokemonResponse = new();
                string urlPokemon = $"https://pokeapi.co/api/v2/pokemon/{pokemonId}";

                var responsePokemon = await client.GetAsync(urlPokemon);

                if (responsePokemon != null)
                {
                    if (responsePokemon.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if (responsePokemon.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay(1000);
                        return await GetPokemon(pokemonId);
                    }
                    else if (!responsePokemon.IsSuccessStatusCode)
                    {
                        throw new Exception("Falha ao carregar o pokemon");
                    }
                    else
                    {
                        var responseBodyPokemon = await responsePokemon.Content.ReadAsStringAsync();
                        pokemonResponse = JsonConvert.DeserializeObject<PokemonResponse>(responseBodyPokemon);
                    }
                }
                return pokemonResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Pokemon GetPokemonByName(string name)
        {
            var pokemon = Repository.Pokemon.FirstOrDefault(p => p.Name == name);
            return pokemon;
        }

        public async Task<PokemonSpecies> GetPokemonSpecies(string pokemonId)
        {
            try
            {
                PokemonSpecies pokemonSpecies = new();

                string urlSpecies = $"https://pokeapi.co/api/v2/pokemon-species/{pokemonId}";
                var responseSpecies = await client.GetAsync(urlSpecies);
                if (responseSpecies != null)
                {
                    if (responseSpecies.StatusCode == HttpStatusCode.Unused)
                    {
                    }

                    if (responseSpecies.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if (responseSpecies.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay(1000);
                        return await GetPokemonSpecies(pokemonId);
                    }
                    else if (!responseSpecies.IsSuccessStatusCode)
                    {
                        throw new Exception("Falha ao carregar a especie do pokemon");
                    }
                    else
                    {
                        var responseBodySpecies = await responseSpecies.Content.ReadAsStringAsync();
                        if (responseBodySpecies == null)
                        {
                            return null;
                        }
                        pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(responseBodySpecies);
                    }
                }
                return pokemonSpecies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<EvolutionChain> GetEvolution(string url)
        {
            string[] urlParts = url.ToString().Split('/');

            int EvolutionChainId = Convert.ToInt32(urlParts[urlParts.Length - 2]);
            string urlEvolution = $"https://pokeapi.co/api/v2/evolution-chain/{EvolutionChainId}";

            var responseEvolution = await client.GetAsync(urlEvolution);
            var responseBodyEvolution = await responseEvolution.Content.ReadAsStringAsync();
            var evolutionChain = JsonConvert.DeserializeObject<EvolutionChain>(responseBodyEvolution);
            return evolutionChain;
        }

        public async Task<List<AbilityWrapper>> GetAbilityListAsync()
        {
            string urlAbilityList = $"https://pokeapi.co/api/v2/ability?limit=100000&offset=0";
            var responseAbilityList = await client.GetAsync(urlAbilityList);
            var responseBodyAbilityList = await responseAbilityList.Content.ReadAsStringAsync();
            var abilityListResponse = JsonConvert.DeserializeObject<AbilityListResponse>(responseBodyAbilityList);
            var task = abilityListResponse.Results.Select(async ability => await GetAbilityDescription(ability.Name));
            var abilities = await Task.WhenAll(task);
            Repository.AbilityWrappers.AddRange(abilities);
            return Repository.AbilityWrappers;
        }

        public async Task<AbilityWrapper> GetAbilityDescription(string abilityName)
        {
            try
            {
                string urlAbility = $"https://pokeapi.co/api/v2/ability/{abilityName}";
                var responseAbility = await client.GetAsync(urlAbility);
                var responseBodyAbility = await responseAbility.Content.ReadAsStringAsync();
                var abilityWrapper = JsonConvert.DeserializeObject<AbilityWrapper>(responseBodyAbility);
                return abilityWrapper;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(abilityName);
                return null;
            }
        }

        public async Task<MoveWrapper> GetMove(string moveName)
        {
            string urlMove = $"https://pokeapi.co/api/v2/move/{moveName}";
            var responseMove = await client.GetAsync(urlMove);
            var responseBodyMove = await responseMove.Content.ReadAsStringAsync();

            var moveResponse = JsonConvert.DeserializeObject<MoveWrapper>(responseBodyMove);
            return moveResponse;
        }

        public async Task<PokemonFormResponse> GetPokemonForm(string pokemonName)
        {
            string urlPokemonForm = $"https://pokeapi.co/api/v2/pokemon-form/{pokemonName}";

            var responsePokemonForm = await client.GetAsync(urlPokemonForm);
            var responseBodyPokemonForm = await responsePokemonForm.Content.ReadAsStringAsync();
            var pokemonForm = JsonConvert.DeserializeObject<PokemonFormResponse>(responseBodyPokemonForm);
            return pokemonForm;
        }

        public async Task<List<MoveWrapper>> PopulateMoveList()
        {
            string urlMoveList = $"https://pokeapi.co/api/v2/move?limit=100000&offset=0";
            var responseMoveList = await client.GetAsync(urlMoveList);
            var responseBodyMoveList = await responseMoveList.Content.ReadAsStringAsync();
            var moveListResponse = JsonConvert.DeserializeObject<MoveListResponse>(responseBodyMoveList);
            var task = moveListResponse.Results.Select(async move => await GetMove(move.Name));
            var moves = await Task.WhenAll(task);
            Repository.Moves.AddRange(moves);
            return Repository.Moves;
        }

        public async Task<List<int>> LoadPokemonsListAll()
        {
            bool hasEmptyResults;
            int currentPage = 0;
            List<int> pokemonIdList = new();

            do
            {
                int totalpokemoncount = 200;
                string url = $"https://pokeapi.co/api/v2/pokemon?limit={totalpokemoncount}&offset={currentPage}";
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(1000);
                    return await LoadPokemonsListAll();
                }
                else if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Falha ao carregar a lista de pokemons");
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();

                    PokemonListResponse pokemonResponseList = JsonConvert.DeserializeObject<PokemonListResponse>(json);

                    foreach (var pokemon in pokemonResponseList.Results)
                    {
                        string[] urlparts = pokemon.Url.ToString().Split('/');
                        int id = Convert.ToInt32(urlparts[urlparts.Length - 2]);

                        pokemonIdList.Add(id);
                    }

                    hasEmptyResults = pokemonResponseList.Results.Count == 0;
                    currentPage += 200;
                }
            } while (!hasEmptyResults);

            return pokemonIdList;
        }

        public async Task<double> GetPokemonTotalCount()
        {
            try
            {
                string url = "https://pokeapi.co/api/v2/pokemon/";
                var response = await client.GetAsync(url);
                var test = response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var pokemonList = JsonConvert.DeserializeObject<PokemonListResponse>(json);
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
    }
}