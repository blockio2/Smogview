using Smogview.Wires;
using UnityEngine;

public class pausemenucontrols : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject lensScreen;

    public bool otherUiOpen = false;

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
                GameObject.Find("Wire Overlay").GetComponent<WireInteractionUI>().Close();
            }

    }

}
