using Newtonsoft.Json;
using PokeApi.BackEnd.Entities;
using PokeTrainerBackEnd.Entities;
using PokeTrainerBackEnd.Model;
using PokeTrainerBackEndTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrainerBackEnd.Controller
{
    public class PokemonTrainerController : IPokemonTrainer
    {
        private PokemonTrainerModel _pokemonTrainerModel = new();

        public List<Pokemon> AddPokemonToTeam(Pokemon pokemon)
        {
            var pokemonTeam = new List<Pokemon>();
            pokemonTeam.Add(pokemon);
            return pokemonTeam;
        }

        public List<Pokemon> RemovePokemonFromTeam(Pokemon pokemon)
        {
            var pokemonTeam = new List<Pokemon>();
            pokemonTeam.Remove(pokemon);
            return pokemonTeam;
        }
    }
}