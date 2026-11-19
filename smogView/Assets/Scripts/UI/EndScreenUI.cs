using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
