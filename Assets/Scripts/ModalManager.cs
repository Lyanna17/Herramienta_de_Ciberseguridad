using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModalManager : MonoBehaviour
{
    public GameObject modalWindow;
    public static ModalManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowModal()
    {
        modalWindow.SetActive(true);
        TaskManager tm = FindObjectOfType<TaskManager>();
        if (tm != null) tm.OnTerminalAbierta();
    }

    public void HideModal()
    {
        modalWindow.SetActive(false);
        TaskManager tm = FindObjectOfType<TaskManager>();
        if (tm != null) tm.OnTerminalCerrada();
    }
}