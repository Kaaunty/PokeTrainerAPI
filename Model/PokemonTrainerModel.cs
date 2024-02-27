using PokeTrainerBackEnd.Controller;
using PokeTrainerBackEnd.DAO;
using PokeTrainerBackEnd.Entities;

namespace PokeTrainerBackEnd.Model
{
    public class PokemonTrainerModel
    {
        private PokeTrainerDAO pokeTrainerDAO = new();

        public void Register(PokemonTrainer pokemonTrainer)
        {
            try
            {
                pokeTrainerDAO.Register(pokemonTrainer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}