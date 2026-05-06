using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalManagerText : MonoBehaviour
{
    public GameObject modalWindow;

    public static ModalManagerText instance;

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
