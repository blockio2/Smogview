using Smogview.UI;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LensManager : MonoBehaviour
{
    private GameObject[] defaultObjects;
    private GameObject[] physicalObjects;
    private GameObject[] socialObjects;
    private GameObject[] mentalObjects;


    private Camera _mainCamera;
    private float _cameraZoom;
    private float _targetZoom;
    [SerializeField] private float _defaultCameraZoom = 5.5f;
    [SerializeField] private float _lensCameraZoom = 3.5f;
    [SerializeField] private float _zoomTransitionTime = 0.2f;
    private float _zoomTransitionTimer;
    private bool _zooming;


    [SerializeField] private List<GameObject> allLensObjects = new List<GameObject>();

    private LensIndicatorUI _indicatorUI;


    lens currentlens;
    public lens CurrentLens => currentlens;
    
    
    // old code that is being referenced by other scripts and can be deleted with the legacy wire scripts.
    // Puzzle controllers can gate a lens without replacing the team's smog or camera behavior.
    // A null predicate preserves normal lens availability in all other scenes.
    public System.Func<lens, bool> CanSelectLens { get; set; }
    public event System.Action<lens> LensChanged;

    // New variables for managing current lens
    private bool _physicalUnlocked = false;
    private bool _socialUnlocked = false;
    private bool _mentalUnlocked = false;
    public bool PhysicalUnlocked { 
        get { return _physicalUnlocked; }
        set { _physicalUnlocked = value;
            _indicatorUI.ActivateIcon(lens.Lphysical, value);
        }
    } 
    public bool SocialUnlocked
    {
        get { return _socialUnlocked; }
        set { _socialUnlocked = value;
            _indicatorUI.ActivateIcon(lens.Lsocial, value);
        }
    }
    public bool MentalUnlocked
    {
        get { return _mentalUnlocked; }
        set { _mentalUnlocked = value;
            _indicatorUI.ActivateIcon(lens.Lmental, value);
        }
    }
    

    public enum lens {
        Ldefault,
        Lphysical,
        Lsocial,
        Lmental
    }

    void Awake()
    {
        _mainCamera = FindAnyObjectByType<Camera>();
        _cameraZoom = _defaultCameraZoom;
        _mainCamera.orthographicSize = _defaultCameraZoom;
        _indicatorUI = FindAnyObjectByType<LensIndicatorUI>();
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

        ActivateLens();
    }


    /// <summary>
    /// currently Deactive or Active the gameobjects depending on if they match the current lens 
    /// </summary>
    private void ActivateLens()
    {
        foreach(GameObject g in allLensObjects)
        {
            if (g == null) continue;

            HideWithSmog smogEffect = g.GetComponent<HideWithSmog>();

            if(g.CompareTag(currentlens.ToString()))
            {
                g.SetActive(true);

                if (smogEffect)
                    smogEffect.RemoveSmog();
            }
            else
            {
                if (smogEffect)
                    smogEffect.CreateSmog();

                g.SetActive(false);
            }
            
        }
        LensChanged?.Invoke(currentlens);
    }

    void Update()
    {
        //If zooming, using linear interpolation to change the camera zoom towards it's target zoom
        if (_zooming)
        {
            _zoomTransitionTimer += Time.deltaTime;

            _mainCamera.orthographicSize = Mathf.Lerp(_cameraZoom, _targetZoom, _zoomTransitionTimer / _zoomTransitionTime);

            //Zoom is complete, reset timer and stop zooming.
            if (_zoomTransitionTimer > _zoomTransitionTime)
            {
                _zooming = false;
                _zoomTransitionTimer = 0;
            }
                
        }

    }



    /// <summary>
    /// changes the players current lens
    /// </summary>
    /// <param name="Newlens">lens enum in lensSwitching</param>
    public void SetLens(lens Newlens)
    {
        currentlens = Newlens;
        ActivateLens();

        //Lens changed - camera should zoom to match the lens zoom level
        _zoomTransitionTimer = 0;
        _zooming = true;
        _cameraZoom = _mainCamera.orthographicSize;

        if (currentlens == lens.Ldefault)
        {
            _targetZoom = _defaultCameraZoom;
        }
        else
        {
            _targetZoom = _lensCameraZoom;
        }
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
        // Keyboard and scripted changes use the same unlock check and camera transition.
        switch (context.control.name)
        {
            case "1":
                SetLens(lens.Ldefault);
                break;

            case "2":
                if (PhysicalUnlocked)
                    SetLens(lens.Lphysical);
                break;

            case "3":
                if(SocialUnlocked)
                    SetLens(lens.Lsocial);
                break;

            case "4":
                if(MentalUnlocked)
                    SetLens(lens.Lmental);
                break;

            default:
                currentlens = lens.Ldefault;
                Debug.Log("broken");
                break;
        }




    }

}



