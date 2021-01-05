﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField]Text messageText;
    PartyMemeberUI[] memberSlots;
    List<PokemonLevel> pokemons;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemeberUI>();
    }

    public void SetPartyData(List<PokemonLevel> pokemons)
    {
        this.pokemons = pokemons;
        for(int i = 0; i < memberSlots.Length; i++)
        {
            if(i< pokemons.Count)
            {
                memberSlots[i].SetData(pokemons[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }
        messageText.text = "Choose a Pokemon";
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for(int i = 0; i < pokemons.Count; i++)
        {
            if(i == selectedMember)
            {
                memberSlots[i].SetSelected(true);
            }
            else
            {
                memberSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
