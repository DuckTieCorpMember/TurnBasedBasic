using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelection : MonoBehaviour
{
    [SerializeField] private GameObject unitSelectionIndicator = null;

    public void SelectUnitEvent()
    {
        unitSelectionIndicator.SetActive(true);
    }

    public void UnselectUnitEvent()
    {
        unitSelectionIndicator.SetActive(false);
    }
}
