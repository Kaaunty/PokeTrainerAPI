using PokeApi.BackEnd.Entities;
using PokeTrainerBackEnd;
using PokeTrainerBackEndTest.Controller;
using PokeTrainerBackEndTest.Entities;
using Type = PokeTrainerBackEndTest.Entities.GlobalInfo;

namespace PokeTrainerBackEndTest.Model
{
    public class PokemonModel
    {
#nullable disable
        private IPokemonAPI pokemonAPI = new PokeApiNetController();

        public async Task<Pokemon> GetPokemon(int pokemonId)
        {
            try
            {
                List<Ability> abilities = new();
                PokemonResponse pokemonResponse = await pokemonAPI.GetPokemon(pokemonId);

                PokemonSpecies pokemonSpecies = await pokemonAPI.GetPokemonSpecies(pokemonResponse.Species.Name);
                EvolutionChain evolutionChain = await pokemonAPI.GetEvolution(pokemonSpecies.EvolutionChain.Url.ToString());

                List<PokemonForm> pokemonForms = await GetPokemonForms(pokemonResponse.Forms);
                abilities = GetAbility(pokemonResponse.Abilities);
                List<Move> moves = GetMoves(pokemonResponse.Moves);

                Pokemon pokemon = new Pokemon(pokemonResponse, pokemonSpecies, evolutionChain, pokemonForms, abilities, moves);
                return pokemon;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<PokemonForm>> GetPokemonForms(List<Type> pokemonForms)
        {
            try
            {
                List<PokemonForm> pokemonFormResponsesList = new List<PokemonForm>();
                for (int i = 0; i < pokemonForms.Count;)
                {
                    var pokemonFormName = pokemonForms[i];

                    PokemonFormResponse pokemonFormResponse = await pokemonAPI.GetPokemonForm(pokemonFormName.Name);
                    PokemonForm pokemonForm = new PokemonForm(pokemonFormResponse);
                    pokemonFormResponsesList.Add(pokemonForm);
                    i++;
                }
                return pokemonFormResponsesList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Ability> GetAbility(List<AbilityResponse> abilities)
        {
            try
            {
                List<Ability> abilitiesDesc = new();
                Ability ability = new();

                foreach (var abilityResponse in abilities)
                {
                    if (!abilitiesDesc.Contains(abilitiesDesc.FirstOrDefault(ability => ability.Name == abilityResponse.Name)))
                    {
                        AbilityWrapper abilityWrapper = Repository.AbilityWrappers.FirstOrDefault(abilitywrapper => abilitywrapper.Name == abilityResponse.Name);
                        ability = new Ability(abilityWrapper, abilityResponse);
                        abilitiesDesc.Add(ability);
                    }
                }
                return abilitiesDesc;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
        }

        public List<Move> GetMoves(List<MoveResponse> moves)
        {
            try
            {
                List<Move> movesDesc = new List<Move>();

                foreach (var moveResponse in moves)
                {
                    var moveWrapper = Repository.Moves.FirstOrDefault(m => m.Name == moveResponse.MoveMove.Name);

                    Move move = new Move(moveWrapper, moveResponse);
                    movesDesc.Add(move);
                }

                return movesDesc;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Pokemon>> GetPokemonList()
        {
            try
            {
                List<Pokemon> pokemons = new List<Pokemon>();
                List<int> PokemonUrl = await pokemonAPI.LoadPokemonsListAll();

                await pokemonAPI.GetAbilityListAsync();
                await pokemonAPI.PopulateMoveList();

                var task = PokemonUrl.Select(async id => await GetPokemon(id));
                var pokemonList = await Task.WhenAll(task);
                Repository.Pokemon.AddRange(pokemonList);
                return Repository.Pokemon;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}