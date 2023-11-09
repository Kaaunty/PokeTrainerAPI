using PokeTrainerBackEndTest.Entities;

namespace PokeApi.BackEnd.Entities
{
    public interface IPokemonAPI
    {
        public Task<PokemonResponse> GetPokemon(int pokemonId);

        public Task<PokemonSpecies> GetPokemonSpecies(string pokemonId);

        public Task<EvolutionChain> GetEvolution(string url);

        public Task<List<AbilityWrapper>> GetAbilityListAsync();

        public Task<AbilityWrapper> GetAbilityDescription(string abilityName);

        public Task<MoveWrapper> GetMove(string moveName);

        public Task<PokemonFormResponse> GetPokemonForm(string pokemonName);

        public Task<List<MoveWrapper>> PopulateMoveList();

        public Task<List<int>> LoadPokemonsListAll();

        public Task<double> GetPokemonTotalCount();

        Pokemon GetPokemonByName(string name);
    }

    public interface ITranslationAPI
    {
        public Task<string> Translate(string input);

        public string TranslateType(string input);
    }

    public interface IPokemonSpriteLoaderAPI
    {
        public Task<Byte[]> LoadPokemonSprite(int id);

        public void GetPokemonAnimatedSprite(string pokemonName, bool shiny);

        public void GetPokemonStaticSprite(string pokemonName, bool shiny);
    }
}