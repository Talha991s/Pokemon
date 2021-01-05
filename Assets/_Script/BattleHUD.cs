using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text LevelText;
    [SerializeField] HpBar hpbar;

    PokemonLevel _pokemon;
    public void SetData(PokemonLevel pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        LevelText.text = "Lvl" + pokemon.Level;
        hpbar.SetHP((float)pokemon.HP / pokemon.Maxhp);
    }
    public IEnumerator UpdateHP()
    {
        if(_pokemon.HpChanged)
        {
            yield return hpbar.SetHPSmooth((float)_pokemon.HP / _pokemon.Maxhp);
            _pokemon.HpChanged = false;
        }
      
    }
}

