using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    
    private CharacterController characterController;
    private Renderer[] renderers;
    [SerializeField]
    public Behaviour[] behaviours;

    private PlayerHealth _health;
    [SerializeField] private GameObject _explosionPrefab;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        renderers = GetComponentsInChildren<Renderer>();
        _health = GetComponent<PlayerHealth>();
    }

    public void RespawnOnServer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        HidePlayerClientRpc();
        yield return new WaitForSeconds(3);
        _health.ResetHP();
        ShowPlayerClientRpc();
    }

    [ClientRpc]
    void HidePlayerClientRpc()
    {
        characterController.enabled = false;
        SetPlayerState(false);
        //Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
    }
    
    [ClientRpc]
    void ShowPlayerClientRpc()
    {
        characterController.enabled = false;
        SetPlayerState(false);
    }
    
    void SetPlayerState(bool state)
    {
        foreach (var script in behaviours)
        {
            script.enabled = state;
        }

        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
    }
}
