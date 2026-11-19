using Smogview.Wires;
using UnityEngine;

public class pausemenucontrols : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject lensScreen;

    public bool otherUiOpen = false;

    void Awake()
    {
  
    }


    //Called to enable and disable the pause menu(currently bound to escape)
    public void Pause()
    {
        if (otherUiOpen) return;
            pauseScreen.SetActive(!pauseScreen.activeSelf);

            if (!pauseScreen.activeSelf)
            {
                gameObject.GetComponent<PlayerMovement>().enabled = true;
                lensScreen.SetActive(true);
            
            }
            else
            {
                gameObject.GetComponent<PlayerMovement>().enabled = false;
                lensScreen.SetActive(false);
                if (GameObject.Find("Wire Overlay") == null) return;
                GameObject.Find("Wire Overlay").GetComponent<WireInteractionUI>().Close();
            }

    }

}
