using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationThreshold = 0.1f;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private Vector3 moveLocation = new Vector3();
    private Rigidbody rb = null;

    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveLocation = gameObject.transform.position;
    }

    public void SetMoveLocation(Vector3 location)
    {
        moveLocation = location;
        isMoving = true;
    }

    private void Update()
    {
        if(!isMoving) { return; }

        Vector3 direction = moveLocation - rb.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed));

        if(rotationThreshold < Quaternion.Angle(rb.rotation, targetRotation)) { return; }

        rb.velocity = direction.normalized * moveSpeed;

        if (transform.position == moveLocation) {
            isMoving = false;
        }
    }
}
