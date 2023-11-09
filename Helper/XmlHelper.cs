using PokeTrainerBackEndTest.Controller;
using PokeTrainerBackEndTest.Entities;
using PokeTrainerBackEndTest.Model;
using System.Xml.Serialization;

namespace PokeTrainerBackEnd.Helper
{
#nullable disable

    public class XmlHelper
    {
        private PokemonModel pokemonModel = new PokemonModel();

        public async Task XmlSerealizer()
        {
            try
            {
                List<Pokemon> pokemons = await pokemonModel.GetPokemonList();

                XmlSerializer xmlSerealizer = new XmlSerializer(typeof(List<Pokemon>));
                using (FileStream fs = new FileStream("Xml's/pokemonList.xml", FileMode.Create))
                {
                    xmlSerealizer.Serialize(fs, Repository.Pokemon);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void XmlDeserealizerPokemonList()
        {
            try
            {
                XmlSerializer xmlSerealizer = new XmlSerializer(typeof(List<Pokemon>));
                using (FileStream fs = new FileStream("Xml's/pokemonList.xml", FileMode.Open))
                {
                    Repository.pokemonsList = (List<Pokemon>)xmlSerealizer.Deserialize(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}