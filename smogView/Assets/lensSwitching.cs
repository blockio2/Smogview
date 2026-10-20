using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class lensSwitching : MonoBehaviour
{

    [SerializeField] private TagHandle defaultLayer;
    [SerializeField] private TagHandle physicalLayer;
    [SerializeField] private TagHandle socialLensLayer;

    public int state =0;

    enum lens {
    defaul,
    physical,
    social
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        { 
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        }



    }

    public void swtiching(InputAction.CallbackContext context)
    {
        //state = context.

    }














}
