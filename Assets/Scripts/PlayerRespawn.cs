using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    public float cubeSize = 0.2f;
    public int cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;
    
    
    private CharacterController characterController;
    private Renderer[] renderers;
    [SerializeField]
    public Behaviour[] behaviours;

    private PlayerHealth _health;
    [SerializeField] private GameObject _explosionPrefab;

    private List<Vector3> spawnPoints = new List<Vector3>()
    {
        new Vector3((float)-0.5, -1, -23),
        new Vector3((float)-0.5, -1, 23)
    };
    
    void Start()
    {
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
        
        characterController = GetComponent<CharacterController>();
        renderers = GetComponentsInChildren<Renderer>();
        _health = GetComponent<PlayerHealth>();
    }

    public void RespawnOnServer()
    {
        Debug.Log("Respawn Server");
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        System.Random r = new System.Random();
        HidePlayerClientRpc();
        yield return new WaitForSeconds(3);
        _health.ResetHP();
        RelocatePlayerClientRpc(spawnPoints[r.Next(spawnPoints.Count)]);
        ShowPlayerClientRpc();
    }

    [ClientRpc]
    void HidePlayerClientRpc()
    {
        explode();
        characterController.enabled = false;
        SetPlayerState(false);
    }
    
    [ClientRpc]
    void RelocatePlayerClientRpc(Vector3 pos)
    {
        System.Random r = new System.Random();
        transform.position = pos;
    }
    
    [ClientRpc]
    void ShowPlayerClientRpc()
    {
        SetPlayerState(true);
        characterController.enabled = true;
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
    
    public void explode() {

        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++) {
            for (int y = 0; y < cubesInRow; y++) {
                for (int z = 0; z < cubesInRow; z++) {
                    createPiece(x, y, z);
                }
            }
        }

        //get explosion position
        Vector3 explosionPos = transform.position;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders) {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }

    }

    void createPiece(int x, int y, int z) {

        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
    }
}
