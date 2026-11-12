using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    private GameObject PauseCanvas;
    [SerializeField] private GameObject lensCanvas;
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
            player.GetComponent<PlayerMovement>().enabled = false;

        if (lensCanvas != null)
        {
            lensCanvas.SetActive(false);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        SceneManager.LoadScene("StartScreen");
    }




    public void Resume()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        lensCanvas.SetActive(true);
        gameObject.SetActive(false);
    }


}
