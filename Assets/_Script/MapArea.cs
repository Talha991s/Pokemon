using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<PokemonLevel> wildpokemons;

    public PokemonLevel GetRandomWildPokemon()
    {
        var wildPokemon = wildpokemons[Random.Range(0, wildpokemons.Count)];
        wildPokemon.init();
        return wildPokemon;
    }
}
