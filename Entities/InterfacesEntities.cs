using PokeTrainerBackEndTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrainerBackEnd.Entities
{
    public interface IPokemonTrainer
    {
        public List<Pokemon> AddPokemonToTeam(Pokemon pokemon);

        public List<Pokemon> RemovePokemonFromTeam(Pokemon pokemon);
    }
}