using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUnitControl : MonoBehaviour
{
    [SerializeField] PlayerSelectionControl playerSelectionControl = null;
    [SerializeField] LayerMask floorMask = new LayerMask();

    private Camera mainCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mouse.current.rightButton.wasReleasedThisFrame) { return; }

        Unit unit = playerSelectionControl.GetSelectedUnit();
        if(unit == null) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
        {
            //if (hit.collider == null) { UnselectUnit(); }
            //Unit unit = hit.collider.GetComponent<Unit>();
            //SelectUnit(unit);
        }
    }
}
