using PokeTrainerBackEndTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrainerBackEnd.Entities
{
#nullable disable

    public class PokemonTrainer
    {
        private int _id;
        private string _name;
        private List<Pokemon> _pokemonTeam = new();

        public PokemonTrainer()
        {
        }

        public PokemonTrainer(int id, string name, List<Pokemon> pokemonTeam)
        {
            _id = id;
            _name = name;
            _pokemonTeam = pokemonTeam;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public List<Pokemon> PokemonTeam { get => _pokemonTeam; set => _pokemonTeam = value; }
    }
}