using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer && Input.GetButton("Fire1"))
        {
            transform.position = transform.position + Vector3.up;
        }
    }
}
