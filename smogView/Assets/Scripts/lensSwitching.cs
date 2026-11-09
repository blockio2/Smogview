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
    // Wire-system integration: PuzzleOnePowerController temporarily gates Mental until the tangle unlock.
    // A null gate preserves the original lens availability in scenes without that controller.
    public System.Func<lens, bool> CanSelectLens { get; set; }
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
        // Reject a locked lens before changing currentlens or firing LensChanged, so the HUD stays correct.
        if (CanSelectLens != null && !CanSelectLens(Newlens)) return;
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
        // Route number keys through the same guarded setter used by other scripts.
        switch (context.control.name)
        {
            case "1":
                SetLens(lens.Ldefault);
                break;

            case "2":
                SetLens(lens.Lphysical);
                break;

            case "3":
                SetLens(lens.Lsocial);
                break;

            case "4":
                SetLens(lens.Lmental);
                break;

            default:
                currentlens = lens.Ldefault;
                Debug.Log("broken");
                break;
        }
    }





}



