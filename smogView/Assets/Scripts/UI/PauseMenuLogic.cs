using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    private GameObject PauseCanvas;
    [SerializeField] private GameObject lensCanvas;
    [SerializeField] private GameObject player;



    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            lensCanvas.SetActive(false);
        }       

    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("StartScreen");
    }




    public void Resume()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        lensCanvas.SetActive(true);
        gameObject.SetActive(false);
    }


}
