using System.Collections;
using UnityEngine;

public class ModalManagerHandbook : MonoBehaviour
{
    public GameObject modalWindow;
    public static ModalManagerHandbook instance;

    [Header("Animacion")]
    public float animSpeed = 8f;

    private TaskManager taskManager;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    public void ShowModal()
    {
        modalWindow.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ScaleUp());
        if (taskManager != null) taskManager.OnLibroAbierto();
    }

    public void HideModal()
    {
        modalWindow.transform.localScale = Vector3.one;
        modalWindow.SetActive(false);
        if (taskManager != null) taskManager.OnLibroCerrado();
    }

    IEnumerator ScaleUp()
    {
        modalWindow.transform.localScale = Vector3.zero;
        Vector3 target = Vector3.one;

        while (Vector3.Distance(modalWindow.transform.localScale, target) > 0.01f)
        {
            modalWindow.transform.localScale = Vector3.Lerp(
                modalWindow.transform.localScale, target, Time.deltaTime * animSpeed);
            yield return null;
        }

        modalWindow.transform.localScale = target;
    }
}