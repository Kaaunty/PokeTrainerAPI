using PokeApi.BackEnd.Entities;
using PokeTrainerBackEnd;
using PokeTrainerBackEndTest.Entities;
using System.Linq;
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

        public void GetPokemonAnimatedSprite(string pokemonName, bool shiny)
        {
            if (Repository.PokemonNameCorrection.ContainsKey(pokemonName))
                Repository.PokemonNameCorrection.TryGetValue(pokemonName, out pokemonName);

            string imageUrl;

            if (shiny)
                imageUrl = $"https://play.pokemonshowdown.com/sprites/ani-shiny/{pokemonName.ToLower()}.gif";
            else
                imageUrl = $"https://play.pokemonshowdown.com/sprites/xyani/{pokemonName.ToLower()}.gif";

            string pastaDestino = "Images";

            try
            {
                var response = _httpClient.GetAsync(imageUrl).Result;
                if (response != null)
                {
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    GetPokemonAnimatedSpritePixel(pokemonName, shiny);
                }
                else if (response.IsSuccessStatusCode)
                {
                    byte[] gifBytes = _httpClient.GetByteArrayAsync(imageUrl).Result;

                    if (!Directory.Exists(pastaDestino))
                    {
                        Directory.CreateDirectory(pastaDestino);
                    }
                    string nomeArquivo;
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

        public void GetPokemonAnimatedSpritePixel(string pokemonName, bool shiny)
        {
            string imageUrl;

            if (Repository.PokemonNameCorrection.ContainsKey(pokemonName))
            {
                Repository.PokemonNameCorrection.TryGetValue(pokemonName, out pokemonName);
            }

            if (shiny)
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5ani-shiny/{pokemonName.ToLower()}.gif";
            }
            else
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5ani/{pokemonName.ToLower()}.gif";
            }

            string pastaDestino = "Images";
            try
            {
                var response = _httpClient.GetAsync(imageUrl).Result;
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    GetPokemonStaticSprite(pokemonName, shiny);
                }
                else if (response.IsSuccessStatusCode)
                    if (response != null)
                    {
                        byte[] gifBytes = _httpClient.GetByteArrayAsync(imageUrl).Result;

                        if (!Directory.Exists(pastaDestino))
                        {
                            Directory.CreateDirectory(pastaDestino);
                        }
                        string nomeArquivo;
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
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar a imagem." + ex);
            }
        }

        public void GetPokemonStaticSprite(string pokemonName, bool shiny)
        {
            string imageUrl;

            if (Repository.PokemonNameCorrection.ContainsKey(pokemonName))
            {
                Repository.PokemonNameCorrection.TryGetValue(pokemonName, out pokemonName);
            }

            if (shiny)
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5-shiny/{pokemonName.ToLower()}.png";
            }
            else
            {
                imageUrl = $"https://play.pokemonshowdown.com/sprites/gen5/{pokemonName.ToLower()}.png";
            }

            string pastaDestino = "Images";
            try
            {
                byte[] gifBytes = _httpClient.GetByteArrayAsync(imageUrl).Result;

                if (!Directory.Exists(pastaDestino))
                {
                    Directory.CreateDirectory(pastaDestino);
                }
                string nomeArquivo;
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