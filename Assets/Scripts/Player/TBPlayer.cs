using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TBPlayer : NetworkBehaviour
{
    [SerializeField] private Unit[] units = new Unit[0];
    [SerializeField] private LayerMask unitBlockLayer = new LayerMask();

    #region Server
    [Command]
    public void CmdTryPlaceUnit(int unitID, Vector3 point)
    {
        Unit unitToPlace = null;
        foreach(Unit unit in units)
        {
            if(unit.GetID() == unitID)
            {
                unitToPlace = unit;
                break;
            }
        }
        if(unitToPlace == null) { return; }

        BoxCollider collider = unitToPlace.GetComponent<BoxCollider>();
        if(!CanPlaceUnit(collider, point)) { return; }

        GameObject unitInstance = Instantiate(unitToPlace.gameObject, point, unitToPlace.transform.rotation);
        NetworkServer.Spawn(unitInstance, connectionToClient);
    }
    #endregion

    #region Client

    #endregion

    public bool CanPlaceUnit(BoxCollider collider, Vector3 point)
    {
        if(Physics.CheckBox(point+collider.center, collider.size/2, Quaternion.identity, unitBlockLayer))
        {
            return false;
        }
        return true;
    }
}
