using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject OriginalPanel;
    public GameObject GlowPanel;

    private void Start()
    {
        OriginalPanel.SetActive(true);
        GlowPanel.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SwitchPanel(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SwitchPanel(false);
    }
    private void SwitchPanel(bool toPanel2)
    {
        OriginalPanel.SetActive(!toPanel2);
        GlowPanel.SetActive(toPanel2);
    }
}