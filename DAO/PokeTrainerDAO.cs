using MySql.Data.MySqlClient;
using PokeTrainerBackEnd.Entities;

namespace PokeTrainerBackEnd.DAO
{
    public class PokeTrainerDAO
    {
        private MySqlCommand _sql;
        private Connection _connection = new();

        public void Register(PokemonTrainer pokemonTrainer)
        {
            try
            {
                _connection.OpenConnection();
                _sql = new MySqlCommand("INSERT INTO pokemon_trainer() VALUES (DEFAULT,@Name)", _connection.con);
                _sql.Parameters.AddWithValue("@Name", pokemonTrainer.Name);
                foreach (var pokemon in pokemonTrainer.PokemonTeam)
                {
                    RegisterPokemonName(pokemon.Name, pokemonTrainer.Id);
                }
                _sql.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex);
            }
            finally
            {
                _connection.CloseConnection();
            }
        }

        public void RegisterPokemonName(string pokemonName, int id)
        {
            try
            {
                _connection.OpenConnection();
                _sql = new MySqlCommand("INSERT INTO pokemon_from_trainer() VALUES (@poke_trainer_id,@pokemon_name)", _connection.con);
                _sql.Parameters.AddWithValue("@poke_trainer_id", id);
                _sql.Parameters.AddWithValue("@pokemon_name", pokemonName);
                _sql.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex);
            }
            finally
            {
                _connection.CloseConnection();
            }
        }
    }
}