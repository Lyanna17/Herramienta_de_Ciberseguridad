using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [Header("Panel de Tareas")]
    public GameObject taskPanel;

    [Header("Tareas - Toggles visuales")]
    public TextMeshProUGUI taskTerminalAbrir;
    public TextMeshProUGUI taskTerminalCerrar;
    public TextMeshProUGUI taskLibroAbrir;
    public TextMeshProUGUI taskNotasAbrir;

    private bool terminalAbierta  = false;
    private bool terminalCerrada  = false;
    private bool libroAbierto     = false;
    private bool notasAbiertas    = false;

    private bool panelVisible = false;

    void Start()
    {
        taskPanel.SetActive(false);
        ActualizarUI();
    }

    public void ToggleTaskPanel()
    {
        panelVisible = !panelVisible;
        taskPanel.SetActive(panelVisible);
    }

    public void OnTerminalAbierta()
    {
        if (terminalAbierta) return;
        terminalAbierta = true;
        ActualizarUI();
    }

    public void OnTerminalCerrada()
    {
        if (!terminalAbierta) return;
        if (terminalCerrada) return;
        terminalCerrada = true;
        ActualizarUI();
    }

    public void OnLibroAbierto()
    {
        if (libroAbierto) return;
        libroAbierto = true;
        ActualizarUI();
    }

    public void OnNotasAbiertas()
    {
        if (notasAbiertas) return;
        notasAbiertas = true;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        SetTask(taskTerminalAbrir,  terminalAbierta,  "Abre la terminal");
        SetTask(taskTerminalCerrar, terminalCerrada,  "Cierra la terminal");
        SetTask(taskLibroAbrir,     libroAbierto,     "Abre el libro");
        SetTask(taskNotasAbrir,     notasAbiertas,    "Abre el bloc de notas");
    }

    void SetTask(TextMeshProUGUI label, bool completada, string texto)
    {
        if (completada)
        {
            label.text  = $"<s>{texto}</s>";
            label.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else
        {
            label.text  = texto;
            label.color = Color.white;
        }
    }
}