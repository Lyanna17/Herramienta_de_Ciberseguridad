using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    [Header("Prefabs visuales (solo decorativos)")]
    public GameObject folderPrefab;
    public GameObject textFilePrefab;
    public Transform  desktopArea;

    // Seguimiento de íconos en el escritorio
    Dictionary<string, GameObject> spawnedIcons = new Dictionary<string, GameObject>();

    // Posición en grilla para los íconos
    float currentX  = 20f;
    float currentY  = -20f;
    float offsetX   = 80f;
    float offsetY   = 80f;
    float startX    = 20f;
    float startY    = -20f;
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
            response.Add("chmod, chown, shutdown,");
            response.Add("sudo su, mkdir, touch, echo");
            return response;
        }

        // Guardar entradas ANTES (snapshot real del fileSystem)
        string dirAntes     = terminal.CurrentDirectory;
        var    entriesAntes = ObtenerEntradas(dirAntes);

        // Ejecutar — esto modifica el fileSystem internamente
        string raw = terminal.Execute(userInput);

        // Leer entradas DESPUÉS del mismo directorio donde se ejecutó
        var entriesDespues = ObtenerEntradas(dirAntes);

        // Detectar creaciones
        foreach (var nombre in entriesDespues)
        {
            if (!entriesAntes.Contains(nombre))
                SpawnIcono(dirAntes, nombre);
        }

        // Detectar eliminaciones
        foreach (var nombre in entriesAntes)
        {
            if (!entriesDespues.Contains(nombre))
                DestroyIcono(dirAntes, nombre);
        }

        // Mostrar resultado en terminal
        if (!string.IsNullOrEmpty(raw))
        {
            foreach (string line in raw.Split('\n'))
            {
                string display = line.StartsWith("ERROR:") ? line.Substring(6) : line;
                if (!string.IsNullOrWhiteSpace(display))
                    response.Add(display);
            }
        }

        return response;
    }

    // Devuelve la lista actual de entradas de un directorio
    // (usa reflexión mínima para no exponer todo el fileSystem)
    HashSet<string> ObtenerEntradas(string dirPath)
    {
        return new HashSet<string>(terminal.GetEntradas(dirPath));
    }

    void SpawnIcono(string dir, string nombre)
    {
        // Solo spawnear en el directorio del Desktop por ahora (o en todos)
        // Puedes agregar un filtro: if (!dir.Contains("Desktop")) return;

        string key = dir + "/" + nombre;
        if (spawnedIcons.ContainsKey(key)) return;

        // Determinar si es carpeta o archivo según si tiene extensión
        bool esCarpeta = !nombre.Contains(".");
        GameObject prefab = esCarpeta ? folderPrefab : textFilePrefab;
        if (prefab == null) return;

        GameObject icono = Instantiate(prefab, desktopArea);

        // Asignar nombre en el label del prefab
        var folder = icono.GetComponent<Folder>();
        if (folder != null) folder.SetName(nombre);
        else
        {
            var label = icono.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (label != null) label.text = nombre;
        }

        // Posición en grilla
        RectTransform rt = icono.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(currentX, currentY);
        UpdateNextPosition();

        // Desactivar interacción de clic (solo visual)
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
}