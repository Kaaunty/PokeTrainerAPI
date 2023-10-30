using Gdk;
using Gtk;
using PokeApiNet;
using static PokeApi.BackEnd.Service.ApiRequest;

namespace PokeApi.BackEnd.Service
{
    public class Methods
    {
#nullable disable

        private readonly ApiRequest _apiRequest = new();

        private List<Pokemon> _pokemonlist = new();
        private List<Pokemon> _pokemonListSearch = new();

        public void Initialize(int currentPage, string type, int choice)
        {
            LoadPokemonList(currentPage, type, choice);
        }

        public void LoadPokemonList(int currentPage, string type, int choice)
        {
            if (type == "all")
            {
                _pokemonlist = PokeList.pokemonList;
                _pokemonListSearch = PokeList.pokemonList;
            }
            else
            {
                if (choice == 0)
                {
                    _pokemonlist = _apiRequest.GetPokemonListByTypeAll(currentPage, type);
                    _pokemonListSearch = PokeList.pokemonListAll;
                }
                if (choice == 1)
                {
                    _pokemonlist = _apiRequest.GetPokemonListByTypePure(currentPage, type);
                    _pokemonListSearch = PokeList.pokemonListPureType;
                }
                else if (choice == 2)
                {
                    _pokemonlist = _apiRequest.GetPokemonListByTypeHalfType(currentPage, type);
                    _pokemonListSearch = PokeList.pokemonListHalfType;
                }
                else if (choice == 3)
                {
                    _pokemonlist = _apiRequest.GetPokemonlistByHalfTypeSecondary(currentPage, type);
                    _pokemonListSearch = PokeList.pokemonListHalfSecundaryType;
                }
            }
        }

        public void UpdateButtons(Fixed fix, int currentPage, string type, int choice)
        {
            Initialize(currentPage, type, choice);
            int buttonIndex = 0;

            foreach (var button in fix.AllChildren)
            {
                if (button is Button)
                {
                    Button btn = (Button)button;
                    if (VerifyButtonName(btn.Name))
                    {
                        if (buttonIndex < _pokemonlist.Count)
                        {
                            btn.Data["id"] = _pokemonlist[buttonIndex].Id;
                            btn.Data["name"] = _pokemonlist[buttonIndex].Name;
                            btn.Sensitive = true;

                            if (VerifyButtonName(btn.Name))
                            {
                                var pokemon = _pokemonlist[buttonIndex];
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
                _pokemonListSearch = _pokemonListSearch.Where(pokemon => pokemon.Name.StartsWith(pokemonName)).ToList();
            }

            int buttonIndex = 0;

            foreach (var button in fix.AllChildren)
            {
                if (button is Button)
                {
                    Button btn = (Button)button;
                    if (VerifyButtonName(btn.Name))
                    {
                        if (buttonIndex < _pokemonListSearch.Count)
                        {
                            btn.Data["id"] = _pokemonListSearch[buttonIndex].Id;
                            btn.Data["name"] = _pokemonListSearch[buttonIndex].Name;
                            btn.Sensitive = true;

                            if (VerifyButtonName(btn.Name))
                            {
                                var pokemon = _pokemonListSearch[buttonIndex];
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

        private async void UpdateButtonImages(Button button, int id)
        {
            Image pokeimage = new Image();

            Pixbuf pokemonImage = await _apiRequest.LoadPokemonSprite(id);
            if (pokemonImage != null)
            {
                pokemonImage = pokemonImage.ScaleSimple(50, 50, InterpType.Bilinear);
                pokeimage.Pixbuf = pokemonImage;
            }
            if (pokemonImage == null)
            {
                pokemonImage = new Pixbuf("Images/pokemonerror.png");
                pokemonImage = pokemonImage.ScaleSimple(50, 50, InterpType.Bilinear);
                pokeimage.Pixbuf = pokemonImage;
                button.Image = pokeimage;
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

            return false;
        }

        public void DisableButtons(Button btn)
        {
            if (_pokemonlist != null)
            {
                if (_pokemonlist.Count < 25)
                {
                    btn.Sensitive = false;
                }
            }
        }
    }
}