using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;
    [SerializeField] int lettersperSecond;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionText;
    [SerializeField] List<Text> moveText;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    [SerializeField] Color highlightedcolor;
    public void SetDialogue(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog) 
    {
        //delay letter appearance
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1.0f/lettersperSecond);
        }

        yield return new WaitForSeconds(1.0f);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enable)
    {
        moveSelector.SetActive(enable);
        moveDetails.SetActive(enable);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionText.Count; ++i)
        {
            if(i ==selectedAction)
            {
                actionText[i].color = highlightedcolor;
            }
            else
            {
                actionText[i].color = Color.black;
            }
        }
    }
    public void UpdateMovesSelection(int selectedMoves, Move move)
    {
        for(int i = 0; i<moveText.Count;++i)
        {
            if(i==selectedMoves)
            {
                moveText[i].color = highlightedcolor;
            }
            else
            {
                moveText[i].color = Color.black;
            }
        }
        ppText.text = $"PP {move.PP}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();
    }

    public void SetMoveName(List<Move> moves)
    {
        for(int i =0; i < moveText.Count; ++i)
        {
            if(i<moves.Count)
            {
                moveText[i].text = moves[i].Base.Name;
            }
            else
            {
                moveText[i].text = "-";
            }
        }
    }    
}
