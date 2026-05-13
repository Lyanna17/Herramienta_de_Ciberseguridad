using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    [Header("Prefabs visuales (solo decorativos)")]

    [Header("Task Manager Módulo 2")]
    public TaskManager2 taskManager2;
    public GameObject folderPrefab;
    public GameObject textFilePrefab;
    public Transform  desktopArea;

    Dictionary<string, GameObject> spawnedIcons = new Dictionary<string, GameObject>();

    float currentX = 20f;
    float currentY = -20f;
    float offsetX  = 80f;
    float offsetY  = 80f;
    float startX   = 20f;
    float startY   = -20f;
    float panelH;

    TerminalSystem terminal;

    void Start()
    {
        terminal = new TerminalSystem();

        RectTransform rect = desktopArea.GetComponent<RectTransform>();
        panelH = rect.rect.height;
    }

    public List<string> Interpret(string userInput)
    {
        var response = new List<string>();

        if (string.IsNullOrWhiteSpace(userInput))
            return response;

        if (userInput.Trim() == "-help")
        {
            response.Add("Comandos disponibles: ls, cd, pwd, cat");
            response.Add("chmod, chown, ifconfig,");
            response.Add("touch, mkdir, whoami, echo");
            return response;
        }

        // Snapshot ANTES
        string dirAntes     = terminal.CurrentDirectory;
        var    entriesAntes = ObtenerEntradas(dirAntes);

        // Ejecutar en TerminalSystem
        string raw = terminal.Execute(userInput);

        Debug.Log($"[DEBUG] raw='{raw}' | trim='{raw.Trim()}' | esClear={raw.Trim() == "%%CLEAR%%"}");

        // Detectar clear 
        if (raw.Trim() == "%%CLEAR%%")
        {
            response.Add("%%CLEAR%%");
            return response;
        }

        // Snapshot DESPUÉS
        var entriesDespues = ObtenerEntradas(dirAntes);

        // Detectar creaciones
        foreach (var nombre in entriesDespues)
            if (!entriesAntes.Contains(nombre))
                SpawnIcono(dirAntes, nombre);

        // Detectar eliminaciones
        foreach (var nombre in entriesAntes)
            if (!entriesDespues.Contains(nombre))
                DestroyIcono(dirAntes, nombre);

        // Convertir resultado en líneas
        if (!string.IsNullOrEmpty(raw))
        {
            foreach (string line in raw.Split('\n'))
            {
                string display = line.StartsWith("ERROR:") ? line.Substring(6) : line;
                if (!string.IsNullOrWhiteSpace(display))
                    response.Add(display);
            }
        }

        if (taskManager2 != null)
            taskManager2.OnComandoEjecutado(userInput);

        return response;
    }

    HashSet<string> ObtenerEntradas(string dirPath)
    {
        return new HashSet<string>(terminal.GetEntradas(dirPath));
    }

    void SpawnIcono(string dir, string nombre)
    {
        string key = dir + "/" + nombre;
        if (spawnedIcons.ContainsKey(key)) return;

        bool       esCarpeta = !nombre.Contains(".");
        GameObject prefab    = esCarpeta ? folderPrefab : textFilePrefab;
        if (prefab == null) return;

        GameObject icono = Instantiate(prefab, desktopArea);

        var folder = icono.GetComponent<Folder>();
        if (folder != null) folder.SetName(nombre);
        else
        {
            var label = icono.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (label != null) label.text = nombre;
        }

        RectTransform rt = icono.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(currentX, currentY);
        UpdateNextPosition();

        var btn = icono.GetComponent<UnityEngine.UI.Button>();
        if (btn != null) btn.interactable = false;

        spawnedIcons[key] = icono;
    }

    void DestroyIcono(string dir, string nombre)
    {
        string key = dir + "/" + nombre;
        if (!spawnedIcons.ContainsKey(key)) return;

        Destroy(spawnedIcons[key]);
        spawnedIcons.Remove(key);
    }

    void UpdateNextPosition()
    {
        currentY -= offsetY;
        if (currentY < -panelH + 100f)
        {
            currentY  = startY;
            currentX += offsetX;
        }
    }

    public bool EsperandoPassword()
    {
        return terminal.EsperandoPasswordSSH();
    }
}