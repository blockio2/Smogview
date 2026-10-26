using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LensSwitching : MonoBehaviour
{
    private GameObject[] defaultObjects;
    private GameObject[] physicalObjects;
    private GameObject[] socialObjects;

    [SerializeField] private List<GameObject> allLensObjects;


    lens currentlens;

    public enum lens {
    Ldefault,
    Lphysical,
    Lsocial
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentlens = lens.Ldefault;

        //lists of all every object with each lens tag
        defaultObjects = GameObject.FindGameObjectsWithTag("Ldefault");
        physicalObjects = GameObject.FindGameObjectsWithTag("Lphysical");
        socialObjects = GameObject.FindGameObjectsWithTag("Lsocial");

         //adds them to a collective list
        allLensObjects.AddRange(defaultObjects);
        allLensObjects.AddRange(physicalObjects);
        allLensObjects.AddRange(socialObjects);

        ActivateLen();
    }


    /// <summary>
    /// currently Deactive or Active the gameobjects depending on if they match the current lens 
    /// </summary>
    private void ActivateLen()
    {
        foreach(GameObject g in allLensObjects)
        {
            if(g.CompareTag(currentlens.ToString()))
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
            
        }
    }





    /// <summary>
    /// changes the players current lens
    /// </summary>
    /// <param name="Newlens">lens enum in lensSwitching</param>
    public void SetLens(lens Newlens)
    {
        //Name of key pressed
        currentlens = Newlens;
        ActivateLen();
    }


    /// <summary>
    /// called when the player presses 1,2,3(currently) 
    /// check which key is pressed and switches the Enum to the proper state
    /// </summary>
    /// <param name="context">the info unity provides when a key is pressed</param>
    public void SetLens(InputAction.CallbackContext context)
    {
        Debug.Log("called");
        //Name of key pressed
        switch (context.control.name)
        {
            case "1":
                currentlens = lens.Ldefault;
                ActivateLen();
                break;

            case "2":
                currentlens = lens.Lphysical;
                ActivateLen();
                break;

            case "3":
                currentlens = lens.Lsocial;
                ActivateLen();
                break;

            default:
                currentlens = lens.Ldefault;
                Debug.Log("broken");
                break;
        }
    }





}
