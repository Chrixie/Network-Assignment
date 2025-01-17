using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using System.Collections;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject spawnMessage;
    [SerializeField] private List<GameObject> spawnedMessages = new List<GameObject>();
    [SerializeField] private Transform playerPosition;
    //[SerializeField] private NetworkVariable<FixedString32Bytes> networkPlayerText = new NetworkVariable<FixedString32Bytes>("Player 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public List<Color> colors = new List<Color>();

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && IsOwner)
        {
            SpawnMessageServerRpc();
        }
    }

    public override void OnNetworkSpawn()
    {
        meshRenderer.material.color = colors[(int)OwnerClientId];
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnMessageServerRpc()
    {
        GameObject message = Instantiate(spawnMessage, playerPosition);
        spawnedMessages.Add(message);
        message.GetComponent<NetworkObject>().Spawn();

        StartCoroutine(MessageSpawned());

    }    
    [ServerRpc(RequireOwnership = false)]
    private void DespawnMessageServerRpc()
    {
        GameObject toDestroy = spawnedMessages[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedMessages.Remove(toDestroy);
        Destroy(toDestroy);

    }

    private IEnumerator MessageSpawned()
    {
        yield return new WaitForSeconds(3);
        DespawnMessageServerRpc();
    }

}
