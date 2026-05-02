using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST } //defining the different states that the game can be in

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public DialogueManager dialogueManager;
    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public Slider hpSlider;

    public BattleHUD playerHealth;
    public BattleHUD enemyHealth;

    public bool SetActive;

    public BattleState state;

    //Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueManager.dialogueText.text = "A viscous " + enemyUnit.unitName + " has appeared...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        playerHealth.SetSlider(playerUnit);
        enemyHealth.SetSlider(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        Debug.Log("Takedamage");


        enemyHealth.SetHP(enemyUnit.currentHP);
        dialogueManager.dialogueText.text = "The attack is successful! " + enemyUnit.unitName + " takes " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(2.5f);

        if(isDead)
        {
            state = BattleState.WON;

            enemyHealth.SetHP(enemyUnit.currentHP = 0);
            enemyUnit.GetComponent<SpriteRenderer>().enabled = false;
            SceneManager.LoadScene("WinScreen");
            EndBattle();
        } else
        {
            state = BattleState.ENEMYTURN;
            enemyHealth.SetHP(enemyUnit.currentHP);

            yield return new WaitForSeconds(2.5f);
            StartCoroutine(EnemyTurn());

        }

    }

IEnumerator EnemyTurn()
    {
        dialogueManager.dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(2f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHealth.SetHP(playerUnit.currentHP);
        dialogueManager.dialogueText.text = playerUnit.unitName + " takes " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(2.5f);

        if (isDead)
        {
            state = BattleState.LOST;
            playerUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);

;           SceneManager.LoadScene("LoseScreen");
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueManager.dialogueText.text = "You have defeated the " + enemyUnit.unitName + "!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueManager.dialogueText.text = "You were defeated...";

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void Update()
    {
        if (state == BattleState.WON)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else if (state == BattleState.LOST)
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void PlayerTurn()
    {
        dialogueManager.dialogueText.text = "Choose an action, " + playerUnit.unitName;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return; 
        StartCoroutine(PlayerAttack());

    }

}
