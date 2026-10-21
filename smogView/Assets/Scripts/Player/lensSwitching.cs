using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class lensSwitching : MonoBehaviour
{

    [SerializeField] private TagHandle defaultLayer;
    [SerializeField] private TagHandle physicalLayer;
    [SerializeField] private TagHandle socialLensLayer;

    lens currentlens;
    public int state =0;

    enum lens {
    Ldefault,
    Lphysical,
    Lsocial
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentlens = lens.Ldefault;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentlens)
        { 
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        }



    }

    public void swtiching(InputAction.CallbackContext context)
    {
        //Name of key pressed
        switch (context.control.name)
        {
            case "1":
                currentlens = lens.Ldefault;
                break;

            case "2":
                currentlens = lens.Lphysical;
                break;

            case "3":
                currentlens = lens.Lsocial;
                break;

            default:
                currentlens = lens.Ldefault;
                Debug.Log("broken");
                break;


        }
        
        
    }














}
