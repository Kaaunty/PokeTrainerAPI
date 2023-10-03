using Newtonsoft.Json;
using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.BackEnd.Entities
{
    public class PokeList
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("results")]
        public List<Pokemon> result { get; set; }

        public List<PokemonType> type { get; set; }
    }
}