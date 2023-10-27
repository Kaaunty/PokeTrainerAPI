using Gdk;
using PokeApiNet;
using Type = PokeApiNet.Type;

namespace PokeApi.BackEnd.Service
{
    public interface IApiRequests
    {
        public Task<Pokemon> GetPokemon(string name);

        public Task<Type> GetTypeAsync(string name);

        public Task<List<Move>> GetMoveLearnedByPokemon(Pokemon pokemon);

        public Task<List<Pokemon>> GetPokemonsListAll();

        public List<Pokemon> GetPokemonListByTypePure(int currentpage, string type);

        public List<Pokemon> GetPokemonListByTypeAll(int currentpage, string type);

        public List<Pokemon> GetPokemonListByTypeHalfType(int currentpage, string type);

        public List<Pokemon> GetPokemonlistByHalfTypeSecondary(int currentPage, string type);

        public Task<PokemonSpecies> GetPokemonSpecies(string pokemonName);

        public Task<Pixbuf> LoadPokemonSprite(int id);

        public Task GetPokemonAnimatedSprite(string pokemonName, bool shiny);

        public Task GetPokemonStaticSprite(string pokemonName, bool shiny);

        public Task<EvolutionChain> GetEvolutionChain(string nextEvolution);

        public Task<PokemonForm> GetPokemonForm(string name);

        public Task<Ability> GetPokemonAbility(string abilityName);

        public Task<String> Translate(string input);

        public string TranslateType(string input);
    }
}