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
    private GameObject[] mentalObjects;

    [SerializeField] private List<GameObject> allLensObjects = new List<GameObject>();


    lens currentlens;
    public lens CurrentLens => currentlens;
    public event System.Action<lens> LensChanged;

    public enum lens {
    Ldefault,
    Lphysical,
    Lsocial,
    Lmental
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentlens = lens.Ldefault;

        //lists of all every object with each lens tag
        defaultObjects = GameObject.FindGameObjectsWithTag("Ldefault");
        physicalObjects = GameObject.FindGameObjectsWithTag("Lphysical");
        socialObjects = GameObject.FindGameObjectsWithTag("Lsocial");
        mentalObjects = GameObject.FindGameObjectsWithTag("Lmental");
        //adds them to a collective list
        allLensObjects.AddRange(defaultObjects);
        allLensObjects.AddRange(physicalObjects);
        allLensObjects.AddRange(socialObjects);
        allLensObjects.AddRange(mentalObjects);

        ActivateLen();
    }


    /// <summary>
    /// currently Deactive or Active the gameobjects depending on if they match the current lens 
    /// </summary>
    private void ActivateLen()
    {
        foreach(GameObject g in allLensObjects)
        {
            if (g == null) continue;
            if(g.CompareTag(currentlens.ToString()))
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
            
        }
        LensChanged?.Invoke(currentlens);
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
    /// called when the player presses 1,2,3,4(currently) 
    /// check which key is pressed and switches the Enum to the proper state
    /// </summary>
    /// <param name="context">the info unity provides when a key is pressed</param>
    public void SetLens(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
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

            case "4":
                currentlens = lens.Lmental;
                ActivateLen();
                break;

            default:
                currentlens = lens.Ldefault;
                Debug.Log("broken");
                break;
        }
    }





}



