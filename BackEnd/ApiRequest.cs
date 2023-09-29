using PokeApiNet;

namespace NowComesGtk.BackEnd
{
    internal class ApiRequest
    {
        private PokeApiClient pokeApiClient = new PokeApiClient();

        public async Task<Pokemon> GetPokemon(string pokemonName)
        {
            Pokemon pokemon = await pokeApiClient.GetResourceAsync<Pokemon>(pokemonName);
            return pokemon;
        }

        public async Task<PokemonSpecies> GetPokemonSpecies(string pokemonName)
        {
            PokemonSpecies pokemonSpecies = await pokeApiClient.GetResourceAsync<PokemonSpecies>(pokemonName);
            return pokemonSpecies;
        }
    }
}