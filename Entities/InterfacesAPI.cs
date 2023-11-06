using Type = PokeApiNet.Type;
using PokeApiNet;
using Gdk;

namespace PokeApi.BackEnd.Entities
{
    public interface IPokemonAPI
    {
        public Task<double> GetPokemonTotalCount();

        public Task<Pokemon> GetPokemon(string name);

        public Pokemon GetPokemonByName(string pokemonName);

        public Task<Type> GetTypeAsync(string name);

        public Task<List<Move>> GetMoveLearnedByPokemon(Pokemon pokemon);

        public Task<List<Pokemon>> LoadPokemonsListAll();

        public List<Pokemon> GetPokemonListAll(int currentpage);

        public List<Pokemon> GetPokemonListByTypePure(int currentpage, string type);

        public List<Pokemon> GetPokemonListByTypeAll(int currentpage, string type);

        public List<Pokemon> GetPokemonListByTypeHalfType(int currentpage, string type);

        public List<Pokemon> GetPokemonlistByHalfTypeSecondary(int currentPage, string type);

        public Task<PokemonSpecies> GetPokemonSpecies(string pokemonName);

        public Task<EvolutionChain> GetEvolutionChain(string nextEvolution);

        public Task<PokemonForm> GetPokemonForm(string name);

        public Task<Ability> GetPokemonAbility(string abilityName);
    }

    public interface ITranslationAPI
    {
        public Task<string> Translate(string input);

        public string TranslateType(string input);
    }

    public interface IPokemonSpriteLoaderAPI
    {
        public Task<Pixbuf> LoadPokemonSprite(int id);

        public Task GetPokemonAnimatedSprite(string pokemonName, bool shiny);

        public Task GetPokemonStaticSprite(string pokemonName, bool shiny);
    }
}