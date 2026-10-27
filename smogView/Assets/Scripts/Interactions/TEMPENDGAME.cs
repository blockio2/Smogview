using UnityEngine;

public class TEMPENDGAME : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject endPlace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.position = endPlace.transform.position;
    }

}
