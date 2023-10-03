using Gdk;
using Gtk;
using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.BackEnd.Service
{
    public class Methods
    {
        private readonly ApiRequest _apiRequest = new ApiRequest();

        public async Task Test(EventBox fix, int currentpage)
        {
            List<Pokemon> pokemons = await _apiRequest.LoadPokemonListAsync(currentpage);
            List<string> pokemonNames = new List<string>();
            foreach (Pokemon pokemon in pokemons)
            {
                Console.WriteLine(pokemon.Name);
                pokemonNames.Add(pokemon.Name);
            }
            List<byte[]> pokemonImages = await _apiRequest.GetPokemonImagesAsync(pokemonNames);
            Console.WriteLine(pokemonImages.Count);
            Console.WriteLine("Fim");
        }

        public async Task ModifyButton(Fixed fix, int currentPage)
        {
            List<Pokemon> pokemonList = await _apiRequest.LoadPokemonListAsync(currentPage);

            foreach (var button in fix.AllChildren)
            {
                if (button is Button)
                {
                    Button btn = (Button)button;
                    if (VerifyButtonName(btn.Name))
                    {
                        Console.WriteLine(btn.Name);
                    }
                }
            }
            Console.WriteLine("Fim");
        }

        public async Task ModifyAll2(EventBox fix, int currentPage)
        {
            List<Pokemon> pokemonList = await _apiRequest.LoadPokemonListAsync(currentPage);

            foreach (Fixed fixs in fix.AllChildren)
            {
                foreach (var button in fixs.AllChildren)
                {
                    if (button is Button)
                    {
                        Button btn = (Button)button;
                        if (VerifyButtonName(btn.Name))
                        {
                        }
                    }
                }
            }

            Console.WriteLine("Fim");
        }

        public async Task ModifyAll(EventBox fix, int currentPage)
        {
            List<Pokemon> pokemonList = await _apiRequest.LoadPokemonListAsync(currentPage);

            int buttonIndex = 0;

            foreach (Fixed fixs in fix.AllChildren)
            {
                foreach (var button in fixs.AllChildren)
                {
                    if (button is Button)
                    {
                        Button btn = (Button)button;
                        if (VerifyButtonName(btn.Name))
                        {
                            if (buttonIndex < pokemonList.Count)
                            {
                                var pokemon = pokemonList[buttonIndex];
                                Console.WriteLine(buttonIndex + pokemon.Id);
                                UpdateButtonImages(btn, pokemon.Id);
                            }
                            buttonIndex++;
                        }
                    }
                }
            }

            Console.WriteLine("Fim");
        }

        private async Task UpdateButtonImages(Button button, int id)
        {
            Image pokemonImage = GetImage(id).Result;

            if (pokemonImage != null)
            {
                button.Image = pokemonImage;
            }
        }

        private async Task<Image> GetImage(int id)
        {
            Image pokemonImage = new Image();
            var imageBytes = await _apiRequest.GetPokemonSpriteAsync(id);

            if (imageBytes != null)
            {
                var pixbuf = new Gdk.Pixbuf(imageBytes);
                pixbuf = pixbuf.ScaleSimple(40, 40, Gdk.InterpType.Bilinear);

                if (pixbuf != null)
                {
                    pokemonImage.Pixbuf = pixbuf;
                    pokemonImage.SetSizeRequest(40, 40);
                    return pokemonImage;
                }
                else
                {
                    Console.WriteLine("Erro ao criar o Pixbuf da imagem.");
                }
            }
            else
            {
                Console.WriteLine("Pokémon não encontrado.");
            }
            return pokemonImage;
        }

        private bool VerifyButtonName(string name)
        {
            if (name == "pokemon1")
            {
                return true;
            }
            if (name == "pokemon2")
            {
                return true;
            }
            if (name == "pokemon3")
            {
                return true;
            }
            if (name == "pokemon4")
            {
                return true;
            }
            if (name == "pokemon5")
            {
                return true;
            }
            if (name == "pokemon6")
            {
                return true;
            }
            if (name == "pokemon7")
            {
                return true;
            }
            if (name == "pokemon8")
            {
                return true;
            }
            if (name == "pokemon9")
            {
                return true;
            }
            if (name == "pokemon10")
            {
                return true;
            }
            if (name == "pokemon11")
            {
                return true;
            }
            if (name == "pokemon12")
            {
                return true;
            }
            if (name == "pokemon13")
            {
                return true;
            }
            if (name == "pokemon14")
            {
                return true;
            }
            if (name == "pokemon15")
            {
                return true;
            }
            if (name == "pokemon16")
            {
                return true;
            }
            if (name == "pokemon17")
            {
                return true;
            }
            if (name == "pokemon18")
            {
                return true;
            }
            if (name == "pokemon19")
            {
                return true;
            }
            if (name == "pokemon20")
            {
                return true;
            }
            if (name == "pokemon21")
            {
                return true;
            }
            if (name == "pokemon22")
            {
                return true;
            }
            if (name == "pokemon23")
            {
                return true;
            }
            if (name == "pokemon24")
            {
                return true;
            }
            if (name == "pokemon25")
            {
                return true;
            }
            if (name == "pokemon26")
            {
                return true;
            }
            if (name == "pokemon27")
            {
                return true;
            }
            if (name == "pokemon28")
            {
                return true;
            }
            if (name == "pokemon29")
            {
                return true;
            }
            if (name == "pokemon30")
            {
                return true;
            }
            if (name == "pokemon31")
            {
                return true;
            }
            if (name == "pokemon32")
            {
                return true;
            }
            if (name == "pokemon33")
            {
                return true;
            }
            if (name == "pokemon34")
            {
                return true;
            }
            if (name == "pokemon35")
            {
                return true;
            }
            if (name == "pokemon36")
            {
                return true;
            }
            if (name == "pokemon37")
            {
                return true;
            }
            if (name == "pokemon38")
            {
                return true;
            }
            if (name == "pokemon39")
            {
                return true;
            }
            if (name == "pokemon40")
            {
                return true;
            }
            if (name == "pokemon41")
            {
                return true;
            }
            if (name == "pokemon42")
            {
                return true;
            }
            if (name == "pokemon43")
            {
                return true;
            }
            if (name == "pokemon44")
            {
                return true;
            }
            if (name == "pokemon45")
            {
                return true;
            }
            if (name == "pokemon46")
            {
                return true;
            }
            if (name == "pokemon47")
            {
                return true;
            }
            if (name == "pokemon48")
            {
                return true;
            }
            if (name == "pokemon49")
            {
                return true;
            }

            return false;
        }
    }
}