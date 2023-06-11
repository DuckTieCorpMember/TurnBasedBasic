using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UnitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Unit unit = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private TMP_Text unitName = null;
    [SerializeField] private LayerMask floorMask = new LayerMask();

    private Camera mainCamera;
    private TBPlayer player;
    private BoxCollider unitCollider;
    private GameObject unitPreviewInstance;
    private Renderer unitRendererInstance;

    private void Start()
    {
        mainCamera = Camera.main;

        iconImage.sprite = unit.GetIcon();
        unitName.text = unit.GetUnitName();

        unitCollider = unit.GetComponent<BoxCollider>();
        //player = NetworkClient.connection.identity.GetComponent<TBPlayer>();
    }

    private void Update()
    {
        if(player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<TBPlayer>();
        }

        if(unitPreviewInstance == null) { return; }

        UpdateUnitPreview();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        unitPreviewInstance = Instantiate(unit.GetUnitPreview());
        unitRendererInstance = unitPreviewInstance.GetComponentInChildren<Renderer>();

        unitPreviewInstance.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (unitPreviewInstance == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
        {
            player.CmdTryPlaceUnit(unit.GetID(), hit.transform.position);
        }

        Destroy(unitPreviewInstance);
    }

    private void UpdateUnitPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) { return; }

        unitPreviewInstance.transform.position = hit.point;

        if (!unitPreviewInstance.activeSelf)
        {
            unitPreviewInstance.SetActive(true);
        }

        Color color = player.CanPlaceUnit(unitCollider, hit.point) ? Color.green : Color.red;
        unitRendererInstance.material.SetColor("_BaseColor", color);
    }
}
