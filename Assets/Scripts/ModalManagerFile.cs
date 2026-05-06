using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalManagerFile : MonoBehaviour
{
    public GameObject modalWindow;

    public static ModalManagerFile instance;

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
    }

    public void HideModal()
    {
        modalWindow.SetActive(false);
    }
}
