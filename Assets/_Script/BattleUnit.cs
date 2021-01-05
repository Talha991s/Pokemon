﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHUD hud;
    
    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }

    public BattleHUD Hud
    {
        get { return hud; }
    }

    public PokemonLevel Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }


    public void Setup(PokemonLevel pokemon)
    {
        Pokemon = pokemon;
        if(isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }

        hud.SetData(pokemon);

        image.color = originalColor;
        PlayEnteranimation();
    }
    
    public void PlayEnteranimation()
    {
        if(isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }
        image.transform.DOLocalMoveX(originalPos.x, 2.0f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if(isPlayerUnit)
        {
           sequence.Append( image.transform.DOLocalMoveX(originalPos.x + 50.0f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50.0f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150.0f, 0.5f));
        sequence.Join(image.DOFade(0.0f,0.5f));
    }
}
