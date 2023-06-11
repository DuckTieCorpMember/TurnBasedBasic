using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Unit : NetworkBehaviour
{
    [SerializeField] GameObject unitPreview;
    [SerializeField] private int id = -1;
    [SerializeField] private string unitName = string.Empty;
    [SerializeField] private Sprite icon = null;

    public int GetID() { return id; }
    public Sprite GetIcon() { return icon; }
    public string GetUnitName() { return unitName; }
    public GameObject GetUnitPreview() { return unitPreview; }


    //public static event Action<Unit> ServerOnUnitSpawned;
    //public static event Action<Unit> ServerOnUnitDespawned;

}
