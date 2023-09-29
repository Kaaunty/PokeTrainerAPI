using Gtk;
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

        private void ModifyAllButtons(Fixed fix)
        {
            foreach (var button in fix.AllChildren)
            {
                if (button is Button)
                {
                    var btn = (Button)button;

                    if (btn.Name == "BtnTest" || btn.Name == "BtnTest2")
                    {
                        var pokemonList = _apiRequest.GetPokeList();

                        foreach (var pokemon in pokemonList)
                        {
                            Console.WriteLine(pokemon.Name);
                            UpdateButtonImages(btn, pokemon.Id);
                        }
                    }
                }
            }
        }

        private void UpdateButtonImages(Button button, int id)
        {
            Image pokemonImage = GetImage(id);

            if (pokemonImage != null)
            {
                button.Image = pokemonImage;
            }
        }

        private Image GetImage(int id)
        {
            Image pokemonImage = new Image();
            var imageBytes = _apiRequest.GetPokemonSprite(id);
            if (imageBytes != null)
            {
                var pixbuf = new Gdk.Pixbuf(imageBytes);
                if (pixbuf != null)
                {
                    pokemonImage.Pixbuf = pixbuf;
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
    }
}