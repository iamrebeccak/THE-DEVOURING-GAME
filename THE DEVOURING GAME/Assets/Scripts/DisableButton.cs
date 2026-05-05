using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButton : MonoBehaviour
{
    public Button AttackButton;
    public Button CookButton;
    public Button HealButton;
    public float cooldownTime = 2.0f;

    public BattleState state;

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
}
