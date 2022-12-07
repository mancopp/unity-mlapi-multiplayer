using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> HP = new(100);

    private PlayerRespawn _respawn;

    private void Start()
    {
        _respawn = GetComponent<PlayerRespawn>();
    }

    public void ResetHP()
    {
        HP.Value = 100;
    }

    public void TakeDamage(int damage)
    {
        HP.Value -= damage;

        if (HP.Value <= 0)
        {
            _respawn.RespawnOnServer();
        }
    }
}
