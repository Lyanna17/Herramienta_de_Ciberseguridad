using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
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
        "Bloque 1 — ¿Quién soy y dónde estoy?",
        "Bloque 2 — Navegación por el sistema",
        "Bloque 3 — Crea tus propios archivos y carpetas",
        "Bloque 4 — Lectura de archivos",
        "Bloque 5 — Permisos de archivos",
        "Bloque 6 — Eliminación de archivos",
        "Bloque 7 — Cambio de propietario y cierre del sistema",
        "Bloque 8 — Conoce tu herramienta de ayuda"
    };

    private string[][] tareasPorBloque = new string[][]
    {
        new string[] { "Abre la terminal", "Ejecuta: whoami", "Ejecuta: pwd", "Ejecuta: ls" },
        new string[] { "Ejecuta: cd Documents", "Ejecuta: pwd", "Ejecuta: ls", "Ejecuta: cd .." },
        new string[] { "Ejecuta: mkdir mis_archivos", "Ejecuta: cd mis_archivos", "Ejecuta: echo \"Hola mundo\" > saludo.txt", "Ejecuta: echo \"Linux es genial\" > nota.txt", "Ejecuta: ls" },
        new string[] { "Ejecuta: cat saludo.txt", "Ejecuta: cat nota.txt" },
        new string[] { "Ejecuta: cd /home/sysadmin/Documents", "Ejecuta: chmod u+x hello.sh", "Ejecuta: ls -l" },
        new string[] { "Ejecuta: cd /home/sysadmin/mis_archivos", "Ejecuta: rm nota.txt", "Ejecuta: ls" },
        new string[] { "Ejecuta: su", "Ejecuta: cd /home/sysadmin/Documents", "Ejecuta: chown root animals.txt", "Ejecuta: ls -l", "Ejecuta: cd /", "Ejecuta: shutdown -h now" },
        new string[] { "Cierra la terminal", "Abre el Handbook y Explora Terminal Commands", "Cierra el Handbook" }
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
        item.GetComponent<TextMeshProUGUI>().text = "¡Módulo completado!";
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

    public void OnTerminalAbierta()  { CompletarSiCoincide("Abre la terminal"); }
    public void OnTerminalCerrada()  { CompletarSiCoincide("Cierra la terminal"); }
    public void OnLibroAbierto()     { CompletarSiCoincide("Abre el Handbook y Explora Terminal Commands"); }
    public void OnLibroCerrado()     { CompletarSiCoincide("Cierra el Handbook"); }
    public void OnNotasAbiertas()    { CompletarSiCoincide("Abre las notas"); }
}