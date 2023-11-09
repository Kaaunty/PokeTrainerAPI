using PokeApi.BackEnd.Entities;
using PokeTrainerBackEnd;
using PokeTrainerBackEndTest.Entities;
using System.Net;

namespace PokeApi.BackEnd.Service
{
    public class PokemonImageApiRequest : IPokemonSpriteLoaderAPI
    {
#nullable disable

        private HttpClient _httpClient = new();

        public async Task<Byte[]> LoadPokemonSprite(int id)
        {
            try
            {
                Byte[] ImageBytes = new Byte[0];
                if (Repository.pokemonImageCache.ContainsKey(id))
                {
                    return Repository.pokemonImageCache[id];
                }

                var poke = Repository.Pokemon.FirstOrDefault(pokemon => pokemon.Id == id);

                string url = poke.SpriteUrl;
                url ??= $"https://play.pokemonshowdown.com/sprites/gen5/{poke.Name}.png";
                if (url == "")
                {
                    if (poke.Name.Contains("koraidon"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/koraidon.png";
                    }
                    else if (poke.Name.Contains("-totem") && poke.Name != "kommo-o-totem")
                    {
                        url = $"https://play.pokemonshowdown.com/sprites/gen5/{poke.Name.Replace("-totem", "")}.png";
                    }
                    else if (poke.Name.Contains("kommo-o-totem"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/kommoo.png";
                    }
                    else if (poke.Name == "pikachu-world-cap")
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/pikachu-world.png";
                    }
                    else if (poke.Name.Contains("miraidon"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/miraidon.png";
                    }
                    else if (poke.Name.Contains("ogerpon-wellspring-mask"))
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/ogerpon-wellspring.png";
                    }
                    else if (poke.Name == "mimikyu-totem-disguised")
                    {
                        url = "https://play.pokemonshowdown.com/sprites/gen5/mimikyu.png";
                    }
                }

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.IsSuccessStatusCode)
                {
                    ImageBytes = await _httpClient.GetByteArrayAsync(url);

                    if (ImageBytes != null)
                    {
                        Repository.pokemonImageCache.Add(id, ImageBytes);
                        return ImageBytes;
                    }
                }
                return ImageBytes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task GetPokemonAnimatedSprite(string pokemonName, bool shiny)
        {
            #region if's

            if (pokemonName == "giratina-altered")
            {
                pokemonName = "giratina";
            }
            else if (pokemonName.Contains("mega-y"))
            {
                pokemonName = pokemonName.Replace("mega-y", "megay");
            }
            else if (pokemonName.Contains("mega-x"))
            {
                pokemonName = pokemonName.Replace("mega-x", "megax");
            }
            else if (pokemonName.Contains("-natural"))
            {
                pokemonName = pokemonName.Replace("-natural", "");
            }
            else if (pokemonName.Contains("-normal"))
            {
                pokemonName = pokemonName.Replace("-normal", "");
            }
            else if (pokemonName.Contains("amped-gmax"))
            {
                pokemonName = pokemonName.Replace("amped-gmax", "gmax");
            }
            else if (pokemonName.Contains("-low-key"))
            {
                pokemonName = pokemonName.Replace("-low-key", "-lowkey");
            }
            else if (pokemonName.Contains("toxtricity-amped"))
            {
                pokemonName = pokemonName.Replace("toxtricity-amped", "toxtricity");
            }
            else if (pokemonName.Contains("furfrou-la-reine"))
            {
                pokemonName = pokemonName.Replace("la-reine", "lareine");
            }
            else if (pokemonName.Contains("necrozma-dawn"))
            {
                pokemonName = pokemonName.Replace("dawn", "dawnwings");
            }
            else if (pokemonName.Contains("necrozma-dusk"))
            {
                pokemonName = pokemonName.Replace("dusk", "duskmane");
            }
            else if (pokemonName.Contains("oinkologne-female"))
            {
                pokemonName = pokemonName.Replace("-female", "-f");
            }
            else if (pokemonName.Contains("plumage"))
            {
                pokemonName = pokemonName.Replace("-plumage", "");
            }
            else if (pokemonName.Contains("family-of-three"))
            {
                pokemonName = pokemonName.Replace("family-of-three", "four");
            }
            else if (pokemonName.Contains("disguised"))
            {
                pokemonName = pokemonName.Replace("-disguised", "");
            }
            else if (pokemonName.Contains("totem-busted"))
            {
                pokemonName = pokemonName.Replace("totem-busted", "busted-totem");
            }
            else if (pokemonName.Contains("nidoran-m"))
            {
                pokemonName = pokemonName.Replace("-m", "");
            }

            #endregion if's

            string imageUrl = "";

            if (shiny)
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/ani-shiny/{pokemonName.ToLower()}.gif";
            }
            else
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/xyani/{pokemonName.ToLower()}.gif";
            }

            string pastaDestino = "Images";
            string nomeArquivo = "";
            try
            {
                var response = _httpClient.GetAsync(imageUrl).Result;
                if (response != null)
                {
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    await GetPokemonStaticSprite(pokemonName, shiny);
                }
                else if (response.IsSuccessStatusCode)
                {
                    byte[] gifBytes = _httpClient.GetByteArrayAsync(imageUrl).Result;

                    if (!Directory.Exists(pastaDestino))
                    {
                        Directory.CreateDirectory(pastaDestino);
                    }
                    if (shiny)
                    {
                        nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimatedShiny.gif");
                    }
                    else
                    {
                        nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimated.gif");
                    }

                    File.WriteAllBytes(nomeArquivo, gifBytes);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Erro ao carregar a imagem.");
            }
        }

        public async Task GetPokemonStaticSprite(string pokemonName, bool shiny)
        {
            #region if's name validation

            string imageUrl = "";

            if (pokemonName == "giratina-altered")
            {
                pokemonName = "giratina";
            }
            if (pokemonName.Contains("mega-y"))
            {
                pokemonName = pokemonName.Replace("mega-y", "megay");
            }
            if (pokemonName.Contains("mega-x"))
            {
                pokemonName = pokemonName.Replace("mega-x", "megax");
            }
            if (pokemonName.Contains("-natural"))
            {
                pokemonName = pokemonName.Replace("-natural", "");
            }
            if (pokemonName.Contains("-normal"))
            {
                pokemonName = pokemonName.Replace("-normal", "");
            }
            if (pokemonName.Contains("koraidon"))
            {
                pokemonName = pokemonName.Replace(pokemonName, "koraidon");
            }
            if (pokemonName.Contains("miraidon"))
            {
                pokemonName = pokemonName.Replace(pokemonName, "miraidon");
            }
            if (pokemonName == "tauros-paldea-combat-breed")
            {
                pokemonName = "tauros-paldeacombat";
            }
            if (pokemonName == "tauros-paldea-blaze-breed")
            {
                pokemonName = "tauros-paldeablaze";
            }
            if (pokemonName == "tauros-paldea-aqua-breed")
            {
                pokemonName = "tauros-paldeaaqua";
            }
            if (pokemonName == "roaring-moon")
            {
                pokemonName = "roaringmoon";
            }

            #endregion if's name validation

            if (shiny)
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5-shiny/{pokemonName.ToLower()}.png";
            }
            else
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5/{pokemonName.ToLower()}.png";
            }

            string pastaDestino = "Images";
            string nomeArquivo = "";
            try
            {
                byte[] gifBytes = await _httpClient.GetByteArrayAsync(imageUrl);

                if (!Directory.Exists(pastaDestino))
                {
                    Directory.CreateDirectory(pastaDestino);
                }
                if (shiny)
                {
                    nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimatedShiny.gif");
                }
                else
                {
                    nomeArquivo = Path.Combine(pastaDestino, "PokemonAnimated.gif");
                }
                File.WriteAllBytes(nomeArquivo, gifBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar a imagem." + ex);
            }
        }
    }
}