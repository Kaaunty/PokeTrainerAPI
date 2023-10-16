using Gdk;
using Gtk;
using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.BackEnd.Service
{
    public class Methods
    {
#nullable disable
        private readonly ApiRequest _apiRequest = new();
        private List<Pokemon> pokemonlist;
        private Image Pokeball = new Image("Images/pokeball.png");

        public void Initialize(int currentPage, string type, int choice)
        {
            LoadPokemonList(currentPage, type, choice);
        }

        public void LoadPokemonList(int currentPage, string type, int choice)
        {
            if (choice == 0)
            {
                pokemonlist = _apiRequest.GetPokemonListByTypeAll(currentPage, type);
            }
            if (choice == 1)
            {
                pokemonlist = _apiRequest.GetPokemonListByTypePure(currentPage, type);
            }
            else if (choice == 2)
            {
                pokemonlist = _apiRequest.GetPokemonListByTypeHalfType(currentPage, type);
            }
            else if (choice == 3)
            {
                pokemonlist = _apiRequest.GetPokemonlistByHalfTypeSecondary(currentPage, type);
            }
        }

        public void UpdateButtons(Fixed fix, int currentPage, string type, int choice)
        {
            Initialize(currentPage, type, choice);
            Pokeball = new Image("Images/pokeball.png");
            int buttonIndex = 0;

            foreach (var button in fix.AllChildren)
            {
                if (button is Button)
                {
                    Button btn = (Button)button;
                    if (VerifyButtonName(btn.Name))
                    {
                        if (buttonIndex < pokemonlist.Count)
                        {
                            btn.Data["id"] = pokemonlist[buttonIndex].Id;
                            btn.Data["name"] = pokemonlist[buttonIndex].Name;
                            btn.Sensitive = true;

                            if (VerifyButtonName(btn.Name))
                            {
                                var pokemon = pokemonlist[buttonIndex];
                                UpdateButtonImages(btn, pokemon.Id);
                            }

                            buttonIndex++;
                        }
                        else
                        {
                            btn.Sensitive = false;
                            btn.Data["id"] = 0;
                            btn.Data["name"] = "";
                            btn.Image = null;
                        }
                    }
                }
            }
        }

        public void SearchPokemonName(Fixed fix, int currentPage, string type, int choice, string PokeName)
        {
            Initialize(currentPage, type, choice);

            string pokemonName = PokeName.ToLower();
            if (pokemonName != string.Empty && pokemonName != "Buscar Pokémon")
            {
                pokemonlist = pokemonlist.Where(pokemon => pokemon.Name.StartsWith(pokemonName)).ToList();
            }

            int buttonIndex = 0;

            foreach (var widget in fix.AllChildren)
            {
                if (widget is VBox)
                {
                    var vboxwidget = (VBox)widget;
                    foreach (var button in vboxwidget.AllChildren)
                    {
                        Button btn = (Button)button;

                        if (buttonIndex < pokemonlist.Count)
                        {
                            btn.Data["id"] = pokemonlist[buttonIndex].Id;
                            btn.Data["name"] = pokemonlist[buttonIndex].Name;
                            btn.Sensitive = true;
                            btn.Image = Pokeball;

                            if (VerifyButtonName(btn.Name))
                            {
                                var pokemon = pokemonlist[buttonIndex];
                                UpdateButtonImages(btn, pokemon.Id);
                            }

                            buttonIndex++;
                        }
                        else
                        {
                            btn.Sensitive = false;
                            btn.Data["id"] = 0;
                            btn.Data["name"] = "";
                            btn.Image = null;
                        }
                    }
                }
            }
        }

        private void UpdateButtonImages(Button button, int id)
        {
            Image pokeimage = new Image();

            Pixbuf pokemonImage = _apiRequest.LoadPokemonSprite(id);
            if (pokemonImage != null)
            {
                pokemonImage = pokemonImage.ScaleSimple(40, 40, InterpType.Bilinear);
                pokeimage.Pixbuf = pokemonImage;
            }

            if (pokeimage != null)
            {
                button.Image = pokeimage;
            }
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
            if (name == "pokemon50")
            {
                return true;
            }
            if (name == "pokemon51")
            {
                return true;
            }

            return false;
        }
    }
}