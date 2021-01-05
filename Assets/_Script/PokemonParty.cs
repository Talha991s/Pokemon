using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<PokemonLevel> pokemons;
    public List<PokemonLevel> Pokemons
    {
        get
        {
            return pokemons;
        }
    }

    private void Start()
    {
        foreach(var pokemon in pokemons)
        {
            pokemon.init();
        }
    }

    public PokemonLevel GetHealthyPokemon()
    {
      return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}
