using UnityEngine;
using Unity.Netcode;

public class MoveProjectile : NetworkBehaviour
{
    public ShootFireBall parent;
    [SerializeField] private float shootForce;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.linearVelocity = rb.transform.forward * shootForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner)
            return;

        parent.DestroyServerRpc();
    }
}
