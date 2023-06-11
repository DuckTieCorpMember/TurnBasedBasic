using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 moveLocation = Vector3.zero;
    private Rigidbody rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveLocation = gameObject.transform.position;
    }

    public void SetMoveLocation(Vector3 location)
    {
        moveLocation = location;
    }

    private void Update()
    {
        if(moveLocation == gameObject.transform.position) { return; }

        Vector3 direction = moveLocation - gameObject.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed));
        if(rb.rotation != targetRotation) { return; }

        rb.velocity = direction.normalized * moveSpeed;
    }
}
