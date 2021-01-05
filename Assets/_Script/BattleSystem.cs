using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen,BattleOver}

[System.Serializable]
public enum ImpulseSound
{
    HIT1,
    HIT2,
    HIT3,
    HIT4,
    DIE
}

public class BattleSystem : MonoBehaviour
{
    public AudioSource [] sounds;

    public UnityEvent onExitEncounter;
    public GameObject calmAudio;

    [SerializeField] BattleUnit playerUnit;
    //[SerializeField] BattleHUD playerHud;

    [SerializeField] BattleUnit enemyUnit;
    //[SerializeField] BattleHUD enemyHud;

    [SerializeField] BattleDialogBox dialogBox;

    [SerializeField] PartyScreen partyScreen;

    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;

    public event Action<bool> OnBattleOver;

    PokemonParty playerParty;
    PokemonLevel wildpokemon;

    public void StartBattle(PokemonParty playerParty, PokemonLevel wildpokemon)
    {
        sounds = GetComponents<AudioSource>();
        this.playerParty = playerParty;
        this.wildpokemon = wildpokemon;

       StartCoroutine( SetupBattle());
    }
    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
       // playerHud.SetData(playerUnit.Pokemon);

        enemyUnit.Setup(wildpokemon);
       // enemyHud.SetData(enemyUnit.Pokemon);

        partyScreen.Init();

        dialogBox.SetMoveName(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared. ");

        ChooseFirstTurn();
        //ActionSelection();
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialogue("Choose an action");
        dialogBox.EnableActionSelector(true);
    }
    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }
    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }
    void ChooseFirstTurn()
    {
        if(playerUnit.Pokemon.Speed >= enemyUnit.Pokemon.Speed)
        {
            ActionSelection();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }
    void BattleOver(bool won)
    {
        calmAudio.gameObject.SetActive(true);
        state = BattleState.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        OnBattleOver(won);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;
        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return RunMove(playerUnit, enemyUnit, move);
        if(state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
        
        //move.PP--;
        //yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");

        //playerUnit.PlayAttackAnimation();
        //yield return new WaitForSeconds(1.0f);

        //enemyUnit.PlayHitAnimation();
        //var damageDetails =  enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon); // so if the enemy pokemon fainted else player attack now
        //yield return enemyHud.UpdateHP();
        //yield return ShowDamageDetails(damageDetails);
        //if (damageDetails.Fainted)
        //{
        //    yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} Fainted");
        //    //StartCoroutine(ExitEncounterCoroutine());
        //    enemyUnit.PlayFaintAnimation();

        //    yield return new WaitForSeconds(2.0f);
        //    OnBattleOver(true);
        //}
        //else
        //{
        //    StartCoroutine(EnemyMove());

        //}
    }
    IEnumerator EnemyMove()
    {
        state =BattleState.PerformMove; //checking the state
        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return RunMove(enemyUnit, playerUnit, move);

        if (state == BattleState.PerformMove)
        {
            ActionSelection();
        }
        
        //random move from enemy
        //move.PP--;
        //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");

        //enemyUnit.PlayAttackAnimation();
        //yield return new WaitForSeconds(1.0f);

        //playerUnit.PlayHitAnimation();
        //var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon); // so if the player pokemon fainted else enemy attack now
        //yield return playerHud.UpdateHP();
        //yield return ShowDamageDetails(damageDetails);
        //if (damageDetails.Fainted)
        //{
        //    yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} Fainted");
        //    //StartCoroutine(ExitEncounterCoroutine());
        //    playerUnit.PlayFaintAnimation();

        //    yield return new WaitForSeconds(2.0f);


        //    var nextPokemon = playerParty.GetHealthyPokemon();
        //    if (nextPokemon != null)
        //    {
        //        OpenPartyScreen();
        //    }
        //    else
        //    {
        //        OnBattleOver(false);
        //    }
        //}
        //else
        //{
        //    ActionSelection();

        //}
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move.Base.Name}");
        PlayRandomHitSound();
        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1.0f);

        targetUnit.PlayHitAnimation();

        if(move.Base.Category == MoveCategory.Status)
        {
            yield return RunMoveEffect(move, sourceUnit.Pokemon, targetUnit.Pokemon);
        }
        else
        {
            var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon); // so if the enemy pokemon fainted else player attack now
            yield return targetUnit.Hud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);
        }
        
        if (targetUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} Fainted");
            //StartCoroutine(ExitEncounterCoroutine());
            sounds[(int)ImpulseSound.DIE].Play();
            targetUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2.0f);

            CheckForBattleOver(targetUnit);
            //OnBattleOver(true);
        }
        // status like burn or poison hurt the pokemon after hte turn.
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChange(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.UpdateHP();

        if (sourceUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} Fainted");
            //StartCoroutine(ExitEncounterCoroutine());
            sounds[(int)ImpulseSound.DIE].Play();
            sourceUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2.0f);

            CheckForBattleOver(sourceUnit);
            //OnBattleOver(true);
        }
    }

    IEnumerator RunMoveEffect(Move move, PokemonLevel source, PokemonLevel target)
    {
        var effects = move.Base.Effects;

        //start Boosting
        if (effects.Boosts != null)
        {
            if (move.Base.Target == MoveTarget.Self)
            {
                source.ApplyBoosts(effects.Boosts);
            }
            else
            {
                target.ApplyBoosts(effects.Boosts);
            }
        }
        //Status Condition
        if(effects.Status != ConditionID.none)
        {
            target.SetStatus(effects.Status);
        }


        yield return ShowStatusChange(source);
        yield return ShowStatusChange(target);
    }
    IEnumerator ShowStatusChange(PokemonLevel pokemon)
    {
        while (pokemon.StatusChanges.Count >0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }


    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if(faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                OpenPartyScreen();
            }
            else
            {
                OnBattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
        }
    }


    //show damage details
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical > 1.0f)
        {
            yield return dialogBox.TypeDialog("A Critical hit!");
        }

        if(damageDetails.TypeEffectiveness > 1.0f)
        {
            yield return dialogBox.TypeDialog("The Critical hit is super effective");
        }
        else if(damageDetails.TypeEffectiveness < 1.0f)
        {
            yield return dialogBox.TypeDialog("It's not very effective");
        }
    }
    public void HandleUpdate()
    {
        if(state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if(state == BattleState.PartyScreen)
        {
            // HandleMoveSelection();
            HandlePartySelection();
        }
    }
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        { 
            currentAction -= 2;
        }

        currentAction = Mathf.Clamp(currentAction, 0, 3);
        dialogBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(currentAction == 0)
            {
                MoveSelection();
            }
            else if (currentAction ==1)
            {
                //Bag
            }
            else if (currentAction == 2)
            {
                //Pokemon
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                OnBattleOver(true);
                calmAudio.gameObject.SetActive(true);
            }
        }
        
    }
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMove -= 2;
        }

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);

        dialogBox.UpdateMovesSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }
    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMember -= 2;
        }

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Pokemons[currentMember];
            if(selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pokemon");
                return;
            }
            if(selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText("You can't switch to the same pokemon");
                return;
            }
            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPokemon(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }
    }

    IEnumerator SwitchPokemon(PokemonLevel newPokemon)
    {
        bool currentPokemonFainted = true;

        if(playerUnit.Pokemon.HP > 0)
        {
            currentPokemonFainted = false;
            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}");
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2.0f);
        }

        playerUnit.Setup(newPokemon);
       // playerHud.SetData(newPokemon);

        dialogBox.SetMoveName(newPokemon.Moves);

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name} . ");

        if (currentPokemonFainted)
        {
            ChooseFirstTurn();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }

       
    }

    private void PlayRandomHitSound()
    {
        var randomHitSound = UnityEngine.Random.Range(1, 4);
        sounds[randomHitSound].Play();
    }
    //public void ExitEncounter()
    //{
    //    StartCoroutine(ExitEncounterCoroutine());
    //}

    //public IEnumerator ExitEncounterCoroutine()
    //{
    //    //onExitEncounter.Invoke();
    //    yield return new WaitForSeconds(2.0f);
    //    //transform.root.gameObject.SetActive(true);
    //    SceneManager.LoadScene(1);
    //}
}
