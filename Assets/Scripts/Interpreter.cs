using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    public GameObject folderPrefab;
    public Transform desktopArea;
    public GameObject textFilePrefab;

    Dictionary<string, GameObject> files = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> folders = new Dictionary<string, GameObject>();

    Vector2 nextPosition = new Vector2(20, -20);
    float startX = 20f;
    float startY;

    float offsetX = 50f;
    float offsetY = 50f;

    float currentX;
    float currentY;

    float rectHeight;


    List<string> response = new List<string>();

    TerminalSystem terminal; 

    void Start()
    {
        terminal = new TerminalSystem();

        RectTransform rect = desktopArea.GetComponent<RectTransform>();
        rectHeight = rect.rect.height;

        startY = -20f; 
        currentX = 20f;
        currentY = startY;
    }

    public List<string> Interpret(string userInput)
    {
        response.Clear();

        if(string.IsNullOrWhiteSpace(userInput))
            return response;

        if(userInput == "-help")
        {
            response.Add("Comandos disponibles:");
            response.Add("ls, cd, pwd, cat, head, tail, cp, mv, rm, grep");
            response.Add("chmod, chown, dd, shutdown, su, aptitude");
            return response;
        }

        // concetar todo con el systema de terminal
        string result = terminal.Execute(userInput);

        // Manejo de múltiples líneas
        string[] lines = result.Split('\n');

        foreach(string line in lines)
        {
            if(!string.IsNullOrWhiteSpace(line))
                response.Add(line);
        }

        string[] parts = userInput.Split(' ');

        if(parts[0] == "mkdir" && parts.Length > 1)
        {
            string folderName = parts[1];

            CreateFolder(folderName);

            return new List<string> { "Directorio creado: " + folderName };
        }

        if(parts[0] == "echo" && parts.Length > 1)
        {
            string fileName = parts[1];

            CreateTextFile(fileName);

            return new List<string> { "Archivo creado: " + fileName };
        }

        if(parts[0] == "rm" && parts.Length > 1)
        {
            // rm -r carpeta
            if(parts[1] == "-r" && parts.Length > 2)
            {
                string folderName = parts[2];

                if(folders.ContainsKey(folderName))
                {
                    Destroy(folders[folderName]);
                    folders.Remove(folderName);

                    return new List<string> { "Carpeta eliminada: " + folderName };
                }
                else
                {
                    return new List<string> { "Carpeta no encontrada" };
                }
            }
            else
            {
                // rm archivo
                string fileName = parts[1];

                if(files.ContainsKey(fileName))
                {
                    Destroy(files[fileName]);
                    files.Remove(fileName);

                    return new List<string> { "Archivo eliminado: " + fileName };
                }
                else
                {
                    return new List<string> { "Archivo no encontrado" };
                }
            }
        }

        return response;
    }

    void CreateFolder(string folderName)
    {
        GameObject folder = Instantiate(folderPrefab, desktopArea);

        folder.GetComponent<Folder>().SetName(folderName);

        //posicion
        RectTransform rt = folder.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(currentX, currentY);

        folders.Add(folderName, folder);

        UpdateNextPosition();
    }

    void CreateTextFile(string fileName)
    {
        GameObject file = Instantiate(textFilePrefab, desktopArea);

        file.GetComponent<Folder>().SetName(fileName);

        //posicion
        RectTransform rt = file.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(currentX, currentY);

        files.Add(fileName, file);

        UpdateNextPosition();
    }

    void UpdateNextPosition()
{
    currentY -= offsetY;

    // límite inferior (dentro del panel)
    if (currentY < -rectHeight + 100f)
    {
        currentY = startY;
        currentX += offsetX;
    }
}
}