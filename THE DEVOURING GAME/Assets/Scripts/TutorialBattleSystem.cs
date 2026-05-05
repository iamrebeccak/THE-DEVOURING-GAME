using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum TutorialBattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST } //defining the different states that the game can be in

public class TutorialBattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject goblinPrefab;

    public Transform TutorialPlayerBattleStation;
    public Transform TutorialEnemyBattleStation;

    Unit playerUnit;
    Unit goblinUnit;

    public DialogueManager dialogueManager;
    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD goblinHUD;

    public Slider hpSlider;

    public BattleHUD playerHealth;
    public BattleHUD goblinHealth;

    public bool SetActive;
    public bool isPlayerTurn = true;

    public Button AttackButton;
    public Button CookButton;
    public Button HealButton;
    public float cooldownTime = 2.0f;

    public Transform respawnPoint;

    public BattleState state;

    //Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, TutorialPlayerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject goblinGO = Instantiate(goblinPrefab, TutorialEnemyBattleStation);
        goblinUnit = goblinGO.GetComponent<Unit>();
        Debug.Log("Spawned");

        dialogueManager.dialogueText.text = "Hi there " + playerUnit.unitName + " Welcome to the tutorial!";

        playerHUD.SetHUD(playerUnit);
        goblinHUD.SetHUD(goblinUnit);

        playerHealth.SetSlider(playerUnit);
        goblinHealth.SetSlider(goblinUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
  
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = goblinUnit.TakeDamage(playerUnit.damage);
        Debug.Log("Takedamage");


        goblinHealth.SetHP(goblinUnit.currentHP);
        dialogueManager.dialogueText.text = "The attack is successful! " + goblinUnit.unitName + " takes " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;

            goblinHealth.SetHP(goblinUnit.currentHP = 0);
            goblinUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);

            SceneManager.LoadScene("WinScreen");
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            goblinHealth.SetHP(goblinUnit.currentHP);

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());

        }

    }

    IEnumerator PlayerCook()
    {
        bool isDead = goblinUnit.CookDamage(playerUnit.cook);
        Debug.Log("CookDamage");


        goblinHealth.SetHP(goblinUnit.currentHP);
        dialogueManager.dialogueText.text = "The attack is successful! " + goblinUnit.unitName + " takes " + playerUnit.cook + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;

            goblinHealth.SetHP(goblinUnit.currentHP = 0);
            goblinUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);

            SceneManager.LoadScene("WinScreen");
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            goblinHealth.SetHP(goblinUnit.currentHP);

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());

        }
    }


    public void StartButtonCooldown()
    {
        StartCoroutine(ButtonCooldownRoutine());
    }

    private IEnumerator ButtonCooldownRoutine()
    {
        AttackButton.interactable = false; // Disable button
        CookButton.interactable = false;
        HealButton.interactable = false;

        yield return new WaitForSeconds(cooldownTime);

        AttackButton.interactable = true; // Re-enable button
        CookButton.interactable = true;
        HealButton.interactable = true;
    }


    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHealth.SetHP(playerUnit.currentHP);
        dialogueManager.dialogueText.text = "You feel renewed strength";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        dialogueManager.dialogueText.text = goblinUnit.unitName + " attacks!";

        yield return new WaitForSeconds(2f);

        bool isDead = playerUnit.TakeDamage(goblinUnit.damage);
        playerHealth.SetHP(playerUnit.currentHP);
        dialogueManager.dialogueText.text = playerUnit.unitName + " takes " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.LOST;
            playerUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);

            ; SceneManager.LoadScene("LoseScreen");
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
            dialogueManager.dialogueText.text = "You have defeated the " + goblinUnit.unitName + "!";
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
        dialogueManager.dialogueText.text = "This is turn based combat - you choose your action, wait for your opponent to respond, and then choose your next action. Keep going until you either win or lose the battle";

    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());

    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerHeal());

    }

    public void OnCookButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerCook());

    }
}
