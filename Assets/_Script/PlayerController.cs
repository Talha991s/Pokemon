using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //public static PlayerController player = null;
    public LevelLoader loader;
    public float moveSpeed;
    public LayerMask SolidObjectLayer;
    public LayerMask LongGrassLayer;

    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    public GameObject BattleScene;
    public UnityEvent onEnterEncounter;
    public UnityEvent onExitEncounter;

    //public AudioManager back;
    public event Action onEncounter;

    public GameObject calmaudio;
    SavePlayer savedata;
 

    private void Awake()
    {
        savedata = FindObjectOfType<SavePlayer>();
        savedata.PlayerPauseLoad();
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //prevent player to move diagonally
            if (input.x != 0) input.y = 0;

            if(input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
               
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos-transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
        CheckForEncounters();
    }

    private bool isWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos,0.15f,SolidObjectLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position,0.15f,LongGrassLayer) != null)
        {
            // randomised the tiles for long grass encounter.
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                calmaudio.gameObject.SetActive(false);
                animator.SetBool("isMoving", false);
                Debug.Log("Pokemon Encounter");
                
                StartCoroutine(EnterEncounterCoroutine());
                
                // BattleScene.SetActive(true);
              
            }
            
        }
    }

    IEnumerator EnterEncounterCoroutine()
    {
        
        // onEnterEncounter.Invoke();
        onEncounter();
       // transform.root.gameObject.SetActive(false);
        //loader.LoadNextLevel();
        yield return new WaitForSeconds(2.0f);
        //Destroy(loader);
        //SceneManager.LoadScene(2);
        BattleScene.SetActive(true);
    }

    //public void ExitEncounter()
    //{
    //   // onExitEncounter.Invoke();
    //   // loader.LoadNextLevel();
    //    //transform.root.gameObject.SetActive(true);
        
    //    BattleScene.SetActive(false);
    //    //back.GetComponent<PlayerController>().onExitEncounter.AddListener(t)
    //}
}
