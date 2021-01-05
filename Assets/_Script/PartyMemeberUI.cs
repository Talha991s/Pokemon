using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemeberUI : MonoBehaviour
{

    [SerializeField] Text nameText;
    [SerializeField] Text LevelText;
    [SerializeField] HpBar hpbar;
    [SerializeField] Color hightlightedColor;
    PokemonLevel _pokemon;
    public void SetData(PokemonLevel pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        LevelText.text = "Lvl" + pokemon.Level;
        hpbar.SetHP((float)pokemon.HP / pokemon.Maxhp);
    }

    public void SetSelected(bool selected)
    {
        if(selected)
        {
            nameText.color = hightlightedColor;
        }
        else
        {
            nameText.color = Color.black;
        }
    }
}
