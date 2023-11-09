using PokeTrainerBackEndTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokeApi.BackEnd.Service.PokemonApiRequest;

namespace PokeTrainerBackEnd.Helper
{
    public class PopulateLists
    {
        public List<Pokemon> GetPokemonListAll(int currentpage)
        {
            try
            {
                var pokemonList = Repository.Pokemon;
                pokemonList = pokemonList.Skip(currentpage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonListByTypeAll(int currentpage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = Repository.Pokemon;

                PokeList.pokemonListAllType = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = PokeList.pokemonListAllType.Skip(currentpage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonListByTypePure(int currentpage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = Repository.Pokemon;

                PokeList.pokemonListPureType = pokemonList.Where(pokemon => pokemon.Types.TrueForAll(type => type.Slot == 1) && pokemon.Types.Any(type => type.Type.Name == lowercasetype)).ToList();
                pokemonList = PokeList.pokemonListPureType.Skip(currentpage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonListByTypeHalfType(int currentpage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = Repository.Pokemon;

                PokeList.pokemonListHalfType = pokemonList.Where(pokemon => pokemon.Types.Any(type => type.Type.Name == lowercasetype) && pokemon.Types.Any(type => type.Slot == 2)).ToList();
                pokemonList = PokeList.pokemonListHalfType.Skip(currentpage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pokemon> GetPokemonlistByHalfTypeSecondary(int currentPage, string type)
        {
            string lowercasetype = type.ToLower();
            try
            {
                var pokemonList = Repository.Pokemon;

                PokeList.pokemonListHalfSecundaryType = pokemonList.Where(pokemon => pokemon.Types.Count == 2 && pokemon.Types[1].Type.Name == lowercasetype).ToList();
                pokemonList = PokeList.pokemonListHalfSecundaryType.Skip(currentPage).Take(25).ToList();
                return pokemonList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}