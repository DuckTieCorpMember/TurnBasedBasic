using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelectionControl : MonoBehaviour
{
    [SerializeField] LayerMask unitMask = new LayerMask();

    private Unit selectedUnit = null;
    private Camera mainCamera = null;

    public Unit GetSelectedUnit() { return selectedUnit; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask))
            {
                if(hit.collider == null) { UnselectUnit(); }
                Unit unit = hit.collider.GetComponent<Unit>();
                SelectUnit(unit);
            }
        }
    }

    public void SelectUnit(Unit unit)
    {
        if(selectedUnit != null) selectedUnit.GetComponent<UnitSelection>().UnselectUnitEvent();
        unit.GetComponent<UnitSelection>().SelectUnitEvent();
        selectedUnit = unit;
    }

    public void UnselectUnit()
    {
        if (selectedUnit != null) selectedUnit.GetComponent<UnitSelection>().UnselectUnitEvent();
        selectedUnit = null;
    }
}
