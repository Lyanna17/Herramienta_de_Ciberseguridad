using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager2 : MonoBehaviour
{
    [Header("Panel de Tareas")]
    public GameObject taskPanel;
    public RectTransform taskPanelRect;

    [Header("Prefabs")]
    public GameObject blockTitlePrefab;
    public GameObject taskItemPrefab;

    [Header("Contenedor")]
    public Transform taskContainer;

    private bool panelVisible = false;
    private int bloqueActual = 0;
    private int tareaActual  = 0;
    private List<TextMeshProUGUI> tareasUI = new List<TextMeshProUGUI>();

    private string[] titulosBloques = new string[]
    {
        "FASE 1 — Reconocimiento",
        "FASE 2 — Escaneo de Puertos",
        "FASE 3 — Preparar el Ataque",
        "FASE 4 — Ataque de Fuerza Bruta",
        "FASE 5 — Acceso al Servidor",
        "FASE 6 — Post-Explotación",
        "FASE 7 — Escalada de Directorios"
    };

    private string[][] tareasPorBloque = new string[][]
    {
        new string[] {
            "Ejecuta: ifconfig",
            "Ejecuta: ping -c 4 192.168.1.200"
        },
        new string[] {
            "Ejecuta: nmap 192.168.1.200",
            "Ejecuta: nmap -sV 192.168.1.200"
        },
        new string[] {
            "Ejecuta: ls /usr/share/wordlists",
            "Ejecuta: cat /usr/share/wordlists/rockyou.txt"
        },
        new string[] {
            "Ejecuta: hydra -l admin -P /usr/share/wordlists/rockyou.txt ssh://192.168.1.200"
        },
        new string[] {
            "Ejecuta: ssh admin@192.168.1.200",
            "Ejecuta: batman123"
        },
        new string[] {
            "Ejecuta: whoami",
            "Ejecuta: cat notas.txt",
            "Ejecuta: cat config.bak"
        },
        new string[] {
            "Ejecuta: cd /root",
            "Ejecuta: cat secreto.txt",
            "Ejecuta: cat flag.txt"
        }
    };

    void Start()
    {
        taskPanel.SetActive(false);
        CargarBloque(0);
    }

    public void ToggleTaskPanel()
    {
        panelVisible = !panelVisible;
        taskPanel.SetActive(panelVisible);
    }

    void CargarBloque(int bloque)
    {
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);

        tareasUI.Clear();
        tareaActual  = 0;
        bloqueActual = bloque;

        GameObject titulo = Instantiate(blockTitlePrefab, taskContainer);
        titulo.GetComponent<TextMeshProUGUI>().text = titulosBloques[bloque];

        foreach (string tarea in tareasPorBloque[bloque])
        {
            GameObject item = Instantiate(taskItemPrefab, taskContainer);
            TextMeshProUGUI tmp = item.GetComponent<TextMeshProUGUI>();
            tmp.text  = tarea;
            tmp.color = Color.white;
            tareasUI.Add(tmp);
        }

        ActualizarUI();
    }

    void ActualizarUI()
    {
        for (int i = 0; i < tareasUI.Count; i++)
        {
            if (i < tareaActual)
            {
                tareasUI[i].text  = $"<s>{tareasPorBloque[bloqueActual][i]}</s>";
                tareasUI[i].color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            else
            {
                tareasUI[i].text  = tareasPorBloque[bloqueActual][i];
                tareasUI[i].color = Color.white;
            }
        }
    }

    public void CompletarTareaActual()
    {
        if (bloqueActual >= titulosBloques.Length) return;
        if (tareaActual >= tareasPorBloque[bloqueActual].Length) return;

        tareaActual++;
        ActualizarUI();

        if (tareaActual >= tareasPorBloque[bloqueActual].Length)
        {
            int siguienteBloque = bloqueActual + 1;
            if (siguienteBloque < titulosBloques.Length)
                CargarBloque(siguienteBloque);
            else
                MostrarCompletado();
        }
    }

    private void CompletarSiCoincide(string accion)
    {
        if (bloqueActual >= titulosBloques.Length) return;
        if (tareaActual >= tareasPorBloque[bloqueActual].Length) return;

        string tareaTexto = tareasPorBloque[bloqueActual][tareaActual];

        if (tareaTexto.Trim() == accion.Trim())
            CompletarTareaActual();
    }

    void MostrarCompletado()
    {
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);

        GameObject item = Instantiate(blockTitlePrefab, taskContainer);
        item.GetComponent<TextMeshProUGUI>().text = "¡Módulo 2 completado!";
    }

    public void OnComandoEjecutado(string comando)
    {
        if (bloqueActual >= titulosBloques.Length) return;
        if (tareaActual >= tareasPorBloque[bloqueActual].Length) return;

        string tareaTexto = tareasPorBloque[bloqueActual][tareaActual];

        if (tareaTexto.StartsWith("Ejecuta: "))
        {
            string comandoEsperado = tareaTexto.Substring("Ejecuta: ".Length).Trim();
            if (comando.Trim() == comandoEsperado)
                CompletarTareaActual();
        }
    }
}