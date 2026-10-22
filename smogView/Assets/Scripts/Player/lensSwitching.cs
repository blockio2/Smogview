using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class lensSwitching : MonoBehaviour
{

    [SerializeField] private TagHandle defaultTag;
    [SerializeField] private TagHandle physicalTag;
    [SerializeField] private TagHandle socialLensTag;

    [SerializeField] private GameObject listOfObjects;

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
            case lens.Ldefault:
                listOfObjects.SendMessage("StateChange",defaultTag.ToString() );
                break;

            case lens.Lphysical:
                listOfObjects.SendMessage("StateChange", physicalTag.ToString());
                break;

            case lens.Lsocial:
                listOfObjects.SendMessage("StateChange", socialLensTag.ToString());
                break;

            default:
                Debug.Log("broken");
                listOfObjects.SendMessage("StateChange", defaultTag.ToString());
                break;
        }

    }


    public void Deactivate(TagHandle tag )
    {

    }

    /// <summary>
    /// called when the player presses 1,2,3(currently) 
    /// check which key is pressed and switches the Enum to the proper state
    /// </summary>
    /// <param name="context"></param>
    public void Switching(InputAction.CallbackContext context)
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
