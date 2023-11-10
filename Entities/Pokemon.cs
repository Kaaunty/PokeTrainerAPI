using Newtonsoft.Json;
using System.Text.Json;
using System.Xml.Serialization;

namespace PokeTrainerBackEndTest.Entities
{
#nullable disable

    #region Pokemon

    [Serializable]
    public class PokemonResponse

    {
        public PokemonResponse()
        { }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("types")]
        public List<PokemonType> Types { get; set; }

        [JsonProperty("abilities")]
        public List<AbilityResponse> Abilities { get; set; }

        [JsonProperty("stats")]
        public List<Stat> Stats { get; set; }

        [JsonProperty("forms")]
        public List<GlobalInfo> Forms { get; set; }

        [JsonProperty("moves")]
        public List<MoveResponse> Moves { get; set; }

        [JsonProperty("species")]
        public GlobalInfo Species { get; set; }

        [JsonProperty("sprites")]
        public Sprites Sprites { get; set; }
    }

    public partial class Sprites
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }
    }

    public partial class MoveResponse
    {
        [JsonProperty("move")]
        public GlobalInfo MoveMove { get; set; }

        [JsonProperty("version_group_details")]
        public List<VersionGroupDetail> VersionGroupDetails { get; set; }
    }

    public class Move
    {
        public Move()
        { }

        public Move(MoveWrapper moveWrapper, MoveResponse moveResponse)
        {
            Name = moveResponse.MoveMove.Name;

            Type = moveWrapper.Type.Name;
            LevelLearnedAt = moveResponse.VersionGroupDetails.Last().LevelLearnedAt;
            MoveLearnMethod = moveResponse.VersionGroupDetails.Last().MoveLearnMethod.Name;
        }

        public string Name { get; set; }

        public long LevelLearnedAt { get; set; }

        public string MoveLearnMethod { get; set; }

        public string Type { get; set; }
    }

    public class AbilityListResponse
    {
        [JsonProperty("results")]
        public List<GlobalInfo> Results { get; set; }
    }

    public partial class MoveWrapper
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public GlobalInfo Type { get; set; }
    }

    public partial class VersionGroupDetail
    {
        [JsonProperty("level_learned_at")]
        public long LevelLearnedAt { get; set; }

        [JsonProperty("move_learn_method")]
        public GlobalInfo MoveLearnMethod { get; set; }
    }

    public partial class Stat
    {
        [JsonProperty("base_stat")]
        public long BaseStat { get; set; }

        [JsonProperty("stat")]
        private GlobalInfo StatStat { get; set; }
    }

    public partial class PokemonSpecies
    {
        public PokemonSpecies()
        { }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("capture_rate")]
        public long CaptureRate { get; set; }

        [JsonProperty("egg_groups")]
        public List<GlobalInfo> EggGroups { get; set; }

        [JsonProperty("gender_rate")]
        public long GenderRate { get; set; }

        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonProperty("is_mythical")]
        public bool IsMythical { get; set; }

        [JsonProperty("varieties")]
        public List<Variety> Varieties { get; set; }

        [JsonProperty("evolution_chain")]
        public EvolutionChainResponse EvolutionChain { get; set; }
    }

    public partial class Variety
    {
        public Variety()
        { }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("pokemon")]
        public GlobalInfo Pokemon { get; set; }
    }

    public partial class PokemonType
    {
        public PokemonType()
        {
        }

        [JsonProperty("slot")]
        public long Slot { get; set; }

        [JsonProperty("type")]
        public GlobalInfo Type { get; set; }
    }

    #endregion Pokemon

    #region EvolutionChain

    public class EvolutionChain
    {
        [JsonProperty("chain")]
        public Chain Chain { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public partial class Chain
    {
        [JsonProperty("evolution_details")]
        public List<EvolutionDetail> EvolutionDetails { get; set; }

        [JsonProperty("evolves_to")]
        public List<Chain> EvolvesTo { get; set; }

        [JsonProperty("species")]
        public GlobalInfo Species { get; set; }
    }

    public partial class EvolutionDetail
    {
        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("held_item")]
        public GlobalInfo HeldItem { get; set; }

        [JsonProperty("item")]
        public GlobalInfo Item { get; set; }

        [JsonProperty("min_affection")]
        public string MinAffection { get; set; }

        [JsonProperty("min_beauty")]
        public string MinBeauty { get; set; }

        [JsonProperty("min_happiness")]
        public string MinHappiness { get; set; }

        [JsonProperty("needs_overworld_rain")]
        public bool NeedsOverworldRain { get; set; }

        [JsonProperty("time_of_day")]
        public string TimeOfDay { get; set; }

        [JsonProperty("min_level")]
        public long? MinLevel { get; set; }

        [JsonProperty("trigger")]
        public GlobalInfo Trigger { get; set; }
    }

    public partial class EvolutionChainResponse
    {
        [XmlIgnore]
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    #endregion EvolutionChain

    #region Ability

    public partial class AbilityResponse
    {
        [JsonProperty("ability")]
        public GlobalInfo AbilityAbility { get; set; }

        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        public AbilityResponse(GlobalInfo abilityAbility)
        {
            Name = abilityAbility.Name;
        }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Ability
    {
        public Ability()
        { }

        public Ability(AbilityWrapper abilityWrapper, AbilityResponse abilityResponse)
        {
            if (abilityWrapper.EffectEntries.Count == 0)
            {
                Effect = "No Description";
            }
            else
            {
                Effect = abilityWrapper.EffectEntries.Single(n => n.Language.Name == "en").Effect;
            }
            Name = abilityResponse.Name;
            isHidden = abilityResponse.IsHidden;
        }

        public string Name { get; set; }
        public bool isHidden { get; set; }
        public string Effect { get; set; }
    }

    public partial class AbilityWrapper
    {
        [JsonProperty("effect_entries")]
        public List<PokemonEffectEntry> EffectEntries { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class PokemonEffectEntry
    {
        [JsonProperty("effect")]
        public string Effect { get; set; }

        [JsonProperty("language")]
        public GlobalInfo Language { get; set; }
    }

    #endregion Ability

    public partial class GlobalInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlIgnore]
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public class PokemonForm
    {
        public PokemonForm()
        { }

        public PokemonForm(PokemonFormResponse pokemonFormResponse)
        {
            Name = pokemonFormResponse.Name;
            IsMega = pokemonFormResponse.IsMega;
            IsDefault = pokemonFormResponse.IsDefault;
            Types = pokemonFormResponse.Types;
        }

        public string Name { get; set; }

        public bool IsDefault { get; set; }
        public bool IsMega { get; set; }
        public List<PokemonType> Types { get; set; }
    }

    public partial class PokemonFormResponse
    {
        [JsonProperty("form_name")]
        public string FormName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_mega")]
        public bool IsMega { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("types")]
        public List<PokemonType> Types { get; set; }

        [JsonProperty("pokemon")]
        public GlobalInfo Pokemon { get; set; }
    }

    [Serializable]
    public class Pokemon
    {
        public Pokemon()
        { }

        public Pokemon(PokemonResponse pokemonResponse, PokemonSpecies pokemonSpecies, EvolutionChain evolutionChain, List<PokemonForm> pokemonForm, List<Ability> abilities, List<Move> moves)
        {
            Name = pokemonResponse.Name;
            Id = pokemonResponse.Id;
            CaptureRate = pokemonSpecies.CaptureRate;
            Types = pokemonResponse.Types;
            EggGroups = pokemonSpecies.EggGroups;
            GenderRate = pokemonSpecies.GenderRate;
            Stats = pokemonResponse.Stats;
            isLegendary = pokemonSpecies.IsLegendary;
            isMythical = pokemonSpecies.IsMythical;
            Varieties = pokemonSpecies.Varieties;
            Moves = moves;
            SpriteUrl = pokemonResponse.Sprites.FrontDefault;
            EvolutionChain = evolutionChain;
            PokemonForms = pokemonForm;
            Abilities = abilities;
        }

        public string Name { get; set; }

        public string SpriteUrl { get; set; }
        public int Id { get; set; }
        public long CaptureRate { get; set; }
        public List<PokemonType> Types { get; set; }

        public List<Ability> Abilities { get; set; }

        public List<Stat> Stats { get; set; }

        public List<PokemonForm> PokemonForms { get; set; }

        public List<Move> Moves { get; set; }

        public bool isLegendary { get; set; }
        public bool isMythical { get; set; }

        public EvolutionChain EvolutionChain { get; set; }

        public List<Variety> Varieties { get; set; }
        public List<GlobalInfo> EggGroups { get; set; }

        public long GenderRate { get; set; }
    }

    public class PokemonListResponse
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("results")]
        public List<GlobalInfo> Results { get; set; }
    }

    public class MoveListResponse
    {
        [JsonProperty("results")]
        public List<GlobalInfo> Results { get; set; }
    }
}