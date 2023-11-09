using PokeTrainerBackEndTest.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PokeTrainerBackEnd.Helper
{
    public class DirectoryHelper
    {
        private XmlHelper xmlHelper = new XmlHelper();

        public async Task ValidateXmlArchive()
        {
            if (Directory.Exists("Xml's"))
            {
                if (File.Exists("Xml's/pokemonList.xml"))
                {
                    await CheckUpdate();
                    xmlHelper.XmlDeserealizerPokemonList();
                }
                else
                {
                    await xmlHelper.XmlSerealizer();
                }
            }
            else
            {
                Directory.CreateDirectory("Xml's");
                if (!File.Exists("Xml's/pokemonList.xml"))
                {
                    await xmlHelper.XmlSerealizer();
                }
                else
                {
                    xmlHelper.XmlDeserealizerPokemonList();
                }
            }
        }

        public async Task CheckUpdate()
        {
            DateTime dtFile = File.GetCreationTime("Xml's/pokemonList.xml");
            DateTime dtNow = DateTime.Now;
            TimeSpan dtDiff = dtNow - dtFile;
            Console.WriteLine($"Ultima Mudança feita a {dtDiff.Days} Dias, {dtDiff.Hours} Horas, {dtDiff.Minutes} Minutos");

            if (dtDiff.Days >= 2)
            {
                Console.WriteLine("Atualizando Lista de Pokemon");
                await xmlHelper.XmlSerealizer();
            }
        }
    }
}