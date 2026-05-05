using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenButtonController : MonoBehaviour
{
    public void OnNextClick()
    {
        SceneManager.LoadScene("MapScene");
    }
}
