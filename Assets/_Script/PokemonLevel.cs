using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonLevel
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;


    public PokemonBase Base {
        get
        {
            return _base;
        }
            
    }
    public int Level {
        get
        {
            return level;
        }
            
    }
    public int HP { get; set; }

    public List<Move> Moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public Condition Status { get; private set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();

    public bool HpChanged { get; set;}

    public void init()
    {

        
        //HP = Maxhp;


        //geenrate moves
        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves)
        {
            if(move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count >= 4)
                break;
        }
        CalculateStats();

        HP = Maxhp;
        ResetStatBoost();
        
    }
    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100.0f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100.0f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SPattack * Level) / 100.0f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SPdefense * Level) / 100.0f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100.0f) + 5);


        Maxhp = Mathf.FloorToInt((Base.MaxHp * Level) / 100.0f) + 10;

    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0 }, {Stat.Defense, 0 },{Stat.SpAttack, 0 },{Stat.SpDefense, 0 },{Stat.Speed, 0 },
        };
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];
        //Todo
        int boost = StatBoosts[stat];
        var boostValues = new float[]
        {
            1.0f,1.5f,2.0f,2.5f,3.0f,3.5f,4.0f
        };
        if(boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        }

        return statVal;
    }

    public void ApplyBoosts(List<StatBoost>statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost,-6,6);

            if(boost > 0)
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");
            }



            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
        }
    }
    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }

    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }

    public int SpDefense
    {
        get { return GetStat(Stat.SpDefense); }
    }

    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }


    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }

    public int Maxhp
    {get;private set;
        //get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100.0f) + 10; }
    }
    public DamageDetails TakeDamage(Move move, PokemonLevel attacker)
    {
        float criticalhit = 1.0f;
        if(Random.value * 100.0f <= 6.25)   // random value for critical hit
        {
            criticalhit = 2.0f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = criticalhit,
            Fainted = false
        };

        float attack = (move.Base.Category == MoveCategory.Special)? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.Category == MoveCategory.Special) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1.0f) * type *criticalhit;   // damage depend on the Level of the attacker // random range so that the damage is a lil bit different everytime
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2; // depend the power of the move and the attack stats and the defense the pokemon.
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);

        //HP -= damage; // checking if the pokemon faint
        //if(HP <= 0)
        //{
        //    HP = 0;
        //    damageDetails.Fainted = true;
        //}

        return damageDetails;

    }
    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, Maxhp);
        HpChanged = true;
    }
    public void SetStatus(ConditionID conditionID)
    {
       Status = ConditionDB.Conditions[conditionID];
        StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}");
    }


    // a random move from the enemy
    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
    }
    public void OnBattleOver()
    {
        ResetStatBoost();
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }

    public float Critical { get; set; }

    public float TypeEffectiveness { get; set; }
}