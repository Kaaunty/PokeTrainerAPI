using PokeTrainerBackEndTest.Entities;

namespace PokeTrainerBackEnd
{
    public static class Repository
    {
        private static List<AbilityWrapper> abilities = new();
        private static List<MoveWrapper> moves = new();

        private static Dictionary<string, string> pokemonNameCorrection = new();

        public static Dictionary<int, Byte[]> pokemonImageCache = new();

        public static List<Pokemon> pokemonList = new();
        public static List<Pokemon> pokemonListAllType = new();
        public static List<Pokemon> pokemonListPureType = new();
        public static List<Pokemon> pokemonListHalfType = new();
        public static List<Pokemon> pokemonListHalfSecundaryType = new();

        public static List<AbilityWrapper> AbilityWrappers { get => abilities; }
        public static List<Pokemon> Pokemon { get => pokemonList; }
        public static List<MoveWrapper> Moves { get => moves; }

        public static Dictionary<string, string> PokemonNameCorrection { get => pokemonNameCorrection; }
        public static Dictionary<string, string> typeDamageRelations = new();
    }

    public static class RepositoryPopulation
    {
        public static void PopulateTypeDamageRelationDictionary()
        {
            Repository.typeDamageRelations.Add("normal", "Dano Sofrido Pouco Efetivo: Nenhum\nPouco Efetivo Contra: Rocha, Aço\nDano Sofrido Super Efetivo: Lutador\nSuper Efetivo Contra: Nenhum\nImune: Fantasma\nNenhum Dano a: Fantasma");
            Repository.typeDamageRelations.Add("dark", "Dano Sofrido Pouco Efetivo: Fantasma, Sombrio\nPouco Efetivo Contra: Lutador, Sombrio, Fada\nDano Sofrido Super Efetivo: Lutador, Inseto, Fada\nSuper Efetivo Contra: Fantasma, Psíquico\nImune: Psíquico\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("bug", "Dano Sofrido Pouco Efetivo: Lutador, Terra, Grama\nPouco Efetivo Contra: Lutador, Voador, Venenoso, Fantasma, Aço, Fogo, Fada\nDano Sofrido Super Efetivo: Voador, Pedra, Fogo\nSuper Efetivo Contra: Grama, Psíquico, Sombrio\nImune: Nenhum\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("dragon", "Dano Sofrido Pouco Efetivo: Fogo, Água, Grama, Elétrica\nPouco Efetivo Contra: Aço\nDano Sofrido Super Efetivo: Gelo, Dragão, Fada\nSuper Efetivo Contra: Dragão\nImune: Nenhum\nNenhum Dano a: Fada");
            Repository.typeDamageRelations.Add("electric", "Dano Sofrido Pouco Efetivo: Voador, Aço, elétrico\nPouco Efetivo Contra: Grama, Elétrica, Dragão\nDano Sofrido Super Efetivo: Terra\nSuper Efetivo Contra: Voador, Água\nImune: Nenhum\nNenhum Dano a: Terra");
            Repository.typeDamageRelations.Add("fighting", "Dano Sofrido Pouco Efetivo: Rocha, Inseto, Sombrio\nPouco Efetivo Contra: Voador, Venenososo, Inseto, Psíquico, Fada\nDano Sofrido Super Efetivo: Voador, Psíquico, Fada\nSuper Efetivo Contra: Normal, Pedra, Aço, Gelo, Sombrio\nImune: Nenhum\nNenhum Dano a: Fantasma");
            Repository.typeDamageRelations.Add("fire", "Dano Sofrido Pouco Efetivo: Inseto, Aço, Fogo, Grama, Gelo, Fada\nPouco Efetivo Contra: Pedra, Fogo, Água, Dragão\nDano Sofrido Super Efetivo: Terra, Pedra, Água\nSuper Efetivo Contra: Inseto, aço, grama, gelo\nImune: Nenhum\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("flying", "Dano Sofrido Pouco Efetivo: Lutador, Inseto, Grama\nPouco Efetivo Contra: Rocha, aço, elétrica\nDano Sofrido Super Efetivo: Rock, elétrico, gelo\nSuper Efetivo Contra: Lutador, Inseto, Grama\nImune: Terra\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("ghost", "Dano Sofrido Pouco Efetivo: Venenososo, Inseto\nPouco Efetivo Contra: Sombrio\nDano Sofrido Super Efetivo: Fantasma, Sombrio\nSuper Efetivo Contra: Fantasma, Psíquico\nImune: Normal, Lutador\nNenhum Dano a: Normal");
            Repository.typeDamageRelations.Add("grass", "Dano Sofrido Pouco Efetivo: Terra, Água, Grama, Elétrica\nPouco Efetivo Contra: Voador, Venenososo, Inseto, Aço, Fogo, Grama, Dragão\nDano Sofrido Super Efetivo: Voador, Venenososo, Inseto, Fogo, Gelo\nSuper Efetivo Contra: Terra, Pedra, Água\nImune: Nenhum\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("ground", "Dano Sofrido Pouco Efetivo: Venenososo, Pedra\nPouco Efetivo Contra: Inseto, grama\nDano Sofrido Super Efetivo: Água, grama, gelo\nSuper Efetivo Contra: Venenoso, Pedra, Aço, Fogo, Elétrico\nImune: Elétrico\nNenhum Dano a: Voador");
            Repository.typeDamageRelations.Add("ice", "Dano Sofrido Pouco Efetivo: Gelo\nPouco Efetivo Contra: Aço, Fogo, Água, Gelo\nDano Sofrido Super Efetivo: Lutador, Pedra, Aço, Fogo\nSuper Efetivo Contra: Voador, Terra, Grama, Dragão\nImune: Nenhum\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("poison", "Dano Sofrido Pouco Efetivo: Lutador, Psíquico\nPouco Efetivo Contra: Aço, Psíquico\nDano Sofrido Super Efetivo: Inseto, Fantasma, Sombrio\nSuper Efetivo Contra: Lutador, Venenoso\nImune: Nenhum\nNenhum Dano a: Sombrio");
            Repository.typeDamageRelations.Add("psychic", "Dano Sofrido Pouco Efetivo: Lutador, Psíquico\nPouco Efetivo Contra: Aço, Psíquico\nDano Sofrido Super Efetivo: Inseto, Fantasma, Sombrio\nSuper Efetivo Contra: Lutador, Venenoso\nImune: Nenhum\nNenhum Dano a: Sombrio");
            Repository.typeDamageRelations.Add("rock", "Dano Sofrido Pouco Efetivo: Normal, Voador, Venenoso, Fogo\nPouco Efetivo Contra: Lutador,Terra,Aço\nDano Sofrido Super Efetivo: Lutador, Terra, Aço, Água, Grama\nSuper Efetivo Contra: Voador, Inseto, Fogo, Gelo\nImune: Nenhum\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("steel", "Dano Sofrido Pouco Efetivo: Normal, Voador, Pedra, Inseto, Aço, Grama, Psíquico, Gelo, Dragão, Fada\nPouco Efetivo Contra: Aço, Fogo, Água, Elétrica\nDano Sofrido Super Efetivo: Lutador,Terra,Fogo\nSuper Efetivo Contra: Pedra, Gelo, Fada\nImune: Tóxico\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("water", "Dano Sofrido Pouco Efetivo: Aço, Fogo, Água, Gelo\nPouco Efetivo Contra: Água, Grama, Dragão\nDano Sofrido Super Efetivo: Grama, elétrica\nSuper Efetivo Contra: Terra, Pedra, Fogo\nImune: Nenhum\nNenhum Dano a: Nenhum");
            Repository.typeDamageRelations.Add("fairy", "Dano Sofrido Pouco Efetivo: Lutador, Inseto, Sombrio\nPouco Efetivo Contra: Venenoso, Aço, Fogo\nDano Sofrido Super Efetivo: Venenoso, Aço\nSuper Efetivo Contra: Lutador, Dragão, Sombrio\nImune: Dragão\nNenhum Dano a: Nenhum");
        }

        public static void PokemonSpritesCorrection()
        {
            Repository.PokemonNameCorrection.Add("giratina-altered", "giratina");
            Repository.PokemonNameCorrection.Add("furfrou-la-reine", "furfrou-lareine");
            Repository.PokemonNameCorrection.Add("necrozma-dawn", "necrozma-dawnwings");
            Repository.PokemonNameCorrection.Add("necrozma-dusk", "necrozma-duskmane");
            Repository.PokemonNameCorrection.Add("oinkologne-female", "oinkologne-f");
            Repository.PokemonNameCorrection.Add("nidoran-m", "nidoran");
            Repository.PokemonNameCorrection.Add("charizard-mega-y", "charizard-megay");
            Repository.PokemonNameCorrection.Add("charizard-mega-x", "charizard-megax");
            Repository.PokemonNameCorrection.Add("tauros-paldea-combat-breed", "tauros-paldeacombat");
            Repository.PokemonNameCorrection.Add("tauros-paldea-blaze-breed", "tauros-paldeablaze");
            Repository.PokemonNameCorrection.Add("tauros-paldea-aqua-breed", "tauros-paldeaaqua");
            Repository.PokemonNameCorrection.Add("roaring-moon", "roaringmoon");
            Repository.PokemonNameCorrection.Add("koraidon-sprinting-build", "koraidon");
            Repository.PokemonNameCorrection.Add("koraidon-swimming-build", "koraidon");
            Repository.PokemonNameCorrection.Add("koraidon-gliding-build", "koraidon");
            Repository.PokemonNameCorrection.Add("miraidon-drive-mode", "miraidon");
            Repository.PokemonNameCorrection.Add("miraidon-aquatic-mode", "miraidon");
            Repository.PokemonNameCorrection.Add("miraidon-glide-mode", "miraidon");
            Repository.PokemonNameCorrection.Add("squawkabilly-blue-plumage", "squawkabilly-blue");
            Repository.PokemonNameCorrection.Add("squawkabilly-yellow-plumage", "squawkabilly-yellow");
            Repository.PokemonNameCorrection.Add("squawkabilly-white-plumage", "squawkabilly-white");
            Repository.PokemonNameCorrection.Add("minior-orange-meteor", "minior-orange");
            Repository.PokemonNameCorrection.Add("minior-blue-meteor", "minior-blue");
            Repository.PokemonNameCorrection.Add("minior-green-meteor", "minior-green");
            Repository.PokemonNameCorrection.Add("minior-indigo-meteor", "minior-indigo");
            Repository.PokemonNameCorrection.Add("minior-meteor", "minior-meteor");
            Repository.PokemonNameCorrection.Add("minior-violet-meteor", "minior-violet");
            Repository.PokemonNameCorrection.Add("minior-yellow-meteor", "minior-yellow");
        }
    }
}