using System.Collections.Generic;
using Newtonsoft.Json;

public class TypeResponse
{
    [JsonProperty("pokemon")]
    public List<PokemonEntry> Pokemon { get; set; }
}

public class PokemonEntry
{
    [JsonProperty("pokemon")]
    public PokemonInfo Pokemon { get; set; }
}

public class PokemonInfo
{
    [JsonProperty("name")]
    public string Name { get; set; }

    // Você pode adicionar mais propriedades aqui, se necessário
}