using UnityEngine;

public class CombatUIController : MonoBehaviour
{
    // Drag your buttons here in the Unity Inspector
    public GameObject OnAttackButton;
    public GameObject OnCookButton;
    public GameObject OnHealButton;

    // Track whose turn it is
    private bool isPlayerTurn = true;
    public bool SetActive = true;
    public BattleState state;
 void Update()
    {

        // If/Else statement to control button visibility
        if (isPlayerTurn)
        {
            state = BattleState.PLAYERTURN;
            // Show the buttons during the player's turn
            OnAttackButton.SetActive(true);
            OnCookButton.SetActive(true);
            OnHealButton.SetActive(true);
        }
        else
        {
            state = BattleState.ENEMYTURN;
            // Hide the buttons during the enemy's turn or other actions
            OnAttackButton.SetActive(false);
            OnCookButton.SetActive(false);
            OnHealButton.SetActive(false);
        }
    }
   
}
