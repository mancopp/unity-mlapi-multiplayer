using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public TrailRenderer bulletTrail;
    public Transform gunBarrel;
    
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootServerRpc();
            }
        }
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        Debug.Log("hi");
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out RaycastHit hit))
        {
            Debug.Log("hit");
            var enemyHealth = hit.transform.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }
        }
        ShootClientRpc();
    }
    
    [ClientRpc]
    void ShootClientRpc()
    {
        var bullet = Instantiate(bulletTrail, gunBarrel.position, Quaternion.identity);
        bullet.AddPosition(gunBarrel.position);
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out RaycastHit hit))
        {
            bullet.transform.position = hit.point;
        }
        else
        {
            bullet.transform.position = gunBarrel.position + gunBarrel.forward;
        }
    }
}
