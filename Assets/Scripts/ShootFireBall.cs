using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootFireBall : NetworkBehaviour
{
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform shootTransform;
    [SerializeField] private List<GameObject> spawnedFireBalls = new List<GameObject>();

    private void Update()
    {
        if(!IsOwner)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            shootServerRpc();
        }
    }

    [ServerRpc]
    private void shootServerRpc()
    {
        GameObject go = Instantiate(fireBall, shootTransform.position, shootTransform.rotation);
        spawnedFireBalls.Add(go);
        go.GetComponent<MoveProjectile>().parent = this;
        go.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedFireBalls[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedFireBalls.Remove(toDestroy);
        Destroy(toDestroy);
    }
}
