using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationThreshold = 0.1f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveThreshold = 0.05f;

    [SerializeField] private Vector3 moveLocation = new Vector3();
    private Rigidbody rb = null;

    private bool isMoving = false;
    private bool isTurning = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveLocation = gameObject.transform.position;
    }

    public void SetMoveLocation(Vector3 location)
    {
        moveLocation = location;
        isMoving = true;
        isTurning = true;
    }

    private void FixedUpdate()
    {
        Vector3 direction = moveLocation - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        if (isTurning)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed*Time.deltaTime));

            if (rotationThreshold > Quaternion.Angle(transform.rotation, targetRotation)) { 
                isTurning = false;
                transform.rotation = targetRotation;
            }
        } else if(isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, moveLocation, moveSpeed * Time.deltaTime);
            //rb.velocity = direction.normalized * moveSpeed * Time.deltaTime;

            if (moveThreshold > Vector3.Distance(moveLocation, transform.position))
            {
                isMoving = false;
                transform.position = moveLocation;
            }
        }
    }
}
