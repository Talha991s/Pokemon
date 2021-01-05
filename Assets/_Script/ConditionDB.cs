using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDB 
{


    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned",
                OnAfterTurn = (PokemonLevel pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.Maxhp/8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to poison");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned",
                OnAfterTurn = (PokemonLevel pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.Maxhp/16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to burn");
                }
            }
        }

    };
}
public enum ConditionID
{
    none,psn, brn, slp, par, frz
}
