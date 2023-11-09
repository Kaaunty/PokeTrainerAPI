using PokeTrainerBackEndTest.Entities;

namespace PokeTrainerBackEnd
{
    public static class Repository
    {
        public static List<Pokemon> pokemonsList = new List<Pokemon>();

        private static List<MoveWrapper> moves = new List<MoveWrapper>();

        private static List<AbilityWrapper> abilities = new List<AbilityWrapper>();

        public static Dictionary<int, Byte[]> pokemonImageCache = new Dictionary<int, Byte[]>();
        public static List<AbilityWrapper> AbilityWrappers { get => abilities; }
        public static List<MoveWrapper> Moves { get => moves; }
        public static List<Pokemon> Pokemon { get => pokemonsList; }
    }
}