using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenButtonController : MonoBehaviour
{
    public void OnNextClick()
    {
        SceneManager.LoadScene("MapScene");
    }
}
