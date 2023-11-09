using PokeTrainerBackEndTest.Entities;

namespace PokeApi.BackEnd.Service
{
    public class PokemonApiRequest
    {
#nullable disable

        public static class PokeList
        {
            public static List<Pokemon> pokemonList = new List<Pokemon>();
            public static List<Pokemon> pokemonListAllType = new List<Pokemon>();
            public static List<Pokemon> pokemonListPureType = new List<Pokemon>();
            public static List<Pokemon> pokemonListHalfType = new List<Pokemon>();
            public static List<Pokemon> pokemonListHalfSecundaryType = new List<Pokemon>();

            public static Dictionary<string, string> typeDamageRelations = new();
        }
    }
}