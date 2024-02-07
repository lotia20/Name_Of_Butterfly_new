using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel1;
    public GameObject panel2;

    private Image panel1Image;
    private Image panel2Image;

    private void Start()
    {
        panel1Image = panel1.GetComponent<Image>();
        panel2Image = panel2.GetComponent<Image>();

        // 초기 상태
        panel1.SetActive(true);
        panel2.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        SwitchPanel(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 원상태
        SwitchPanel(false);
    }
    private void SwitchPanel(bool toPanel2)
    {
        panel1.SetActive(!toPanel2);
        panel2.SetActive(toPanel2);
    }
}