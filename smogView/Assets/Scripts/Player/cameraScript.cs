using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
    }
}
