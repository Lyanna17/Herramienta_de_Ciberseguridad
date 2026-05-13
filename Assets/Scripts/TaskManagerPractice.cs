using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManagerPractice : MonoBehaviour
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

    private class Tarea
    {
        public string Descripcion;
        public string ComandoEsperado;
    }

    private readonly string[] archivoPool = new string[]
    {
        "adjectives.txt", "alpha-first.txt", "alpha-second.txt", "alpha-third.txt",
        "alpha.txt", "animals.txt", "food.txt", "hidden.txt", "letters.txt",
        "linux.txt", "longfile.txt", "newhome.txt", "numbers.txt", "os.csv",
        "people.csv", "profile.txt", "red.txt"
    };

    private string[] archivosElegidos = new string[5];

    private string[] titulosBloques = new string[]
    {
        "Reto 1 — Reconocimiento inicial",
        "Reto 2 — El archivo misterioso",
        "Reto 3 — Fortalece el sistema",
        "Reto 4 — Control de acceso",
        "Reto 5 — Limpieza y cierre"
    };

    private Tarea[][] tareasPorBloque;

    void Start()
    {
        taskPanel.SetActive(false);
        ElegirArchivosAleatorios();
        InicializarTareas();
        CargarBloque(0);
    }

    void ElegirArchivosAleatorios()
    {
        List<string> pool = new List<string>(archivoPool);
        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, pool.Count);
            archivosElegidos[i] = pool[index];
            pool.RemoveAt(index);
        }
    }

    void InicializarTareas()
    {
        tareasPorBloque = new Tarea[][]
        {
            new Tarea[]
            {
                new Tarea { Descripcion = "Descubre quién eres en el sistema",          ComandoEsperado = "whoami" },
                new Tarea { Descripcion = "Averigua en qué directorio te encuentras",   ComandoEsperado = "pwd" },
                new Tarea { Descripcion = "Lista el contenido del directorio actual",   ComandoEsperado = "ls" },
                new Tarea { Descripcion = "Navega a Documents",                         ComandoEsperado = "cd Documents" },
                new Tarea { Descripcion = "Lista el contenido de Documents",            ComandoEsperado = "ls" },
            },

            new Tarea[]
            {
                new Tarea { Descripcion = $"Lee el archivo {archivosElegidos[0]}", ComandoEsperado = $"cat {archivosElegidos[0]}" },
                new Tarea { Descripcion = $"Lee el archivo {archivosElegidos[1]}", ComandoEsperado = $"cat {archivosElegidos[1]}" },
                new Tarea { Descripcion = $"Lee el archivo {archivosElegidos[2]}", ComandoEsperado = $"cat {archivosElegidos[2]}" },
                new Tarea { Descripcion = $"Lee el archivo {archivosElegidos[3]}", ComandoEsperado = $"cat {archivosElegidos[3]}" },
                new Tarea { Descripcion = $"Lee el archivo {archivosElegidos[4]}", ComandoEsperado = $"cat {archivosElegidos[4]}" },
            },

            new Tarea[]
            {
                new Tarea { Descripcion = "Dale permisos de ejecución a hello.sh",          ComandoEsperado = "chmod u+x hello.sh" },
                new Tarea { Descripcion = "Vuelve al directorio anterior",                   ComandoEsperado = "cd .." },
                new Tarea { Descripcion = "Crea una carpeta llamada seguro",                 ComandoEsperado = "mkdir seguro" },
                new Tarea { Descripcion = "Verifica los permisos y la carpeta creada",      ComandoEsperado = "ls -l" },
            },

            new Tarea[]
            {
                new Tarea { Descripcion = "Escala privilegios al usuario root",              ComandoEsperado = "su" },
                new Tarea { Descripcion = "Navega a la carpeta Documents de sysadmin",      ComandoEsperado = "cd /home/sysadmin/Documents" },
                new Tarea { Descripcion = "Cambia el propietario de hello.sh a root",       ComandoEsperado = "chown root hello.sh" },
                new Tarea { Descripcion = "Verifica que el cambio fue exitoso",             ComandoEsperado = "ls -l" },
            },

            new Tarea[]
            {
                new Tarea { Descripcion = "Navega al directorio home de sysadmin",          ComandoEsperado = "cd /home/sysadmin" },
                new Tarea { Descripcion = "Elimina el archivo hallazgo.txt",                ComandoEsperado = "rm hallazgo.txt" },
                new Tarea { Descripcion = "Elimina la carpeta seguro y su contenido",       ComandoEsperado = "rm -r seguro" },
                new Tarea { Descripcion = "Apaga el sistema de forma segura",               ComandoEsperado = "shutdown -h now" },
            }
        };
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

        foreach (Tarea tarea in tareasPorBloque[bloque])
        {
            GameObject item = Instantiate(taskItemPrefab, taskContainer);
            TextMeshProUGUI tmp = item.GetComponent<TextMeshProUGUI>();
            tmp.text  = tarea.Descripcion;
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
                tareasUI[i].text  = $"<s>{tareasPorBloque[bloqueActual][i].Descripcion}</s>";
                tareasUI[i].color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            else
            {
                tareasUI[i].text  = tareasPorBloque[bloqueActual][i].Descripcion;
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

    void MostrarCompletado()
    {
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);

        GameObject item = Instantiate(blockTitlePrefab, taskContainer);
        item.GetComponent<TextMeshProUGUI>().text = "¡Práctica completada!";
    }

    public void OnComandoEjecutado(string comando)
    {
        if (bloqueActual >= titulosBloques.Length) return;
        if (tareaActual >= tareasPorBloque[bloqueActual].Length) return;

        string cmdTrim = comando.Trim();

        if (bloqueActual == 1)
        {
            for (int i = 0; i < tareasPorBloque[bloqueActual].Length; i++)
            {
                if (cmdTrim == tareasPorBloque[bloqueActual][i].ComandoEsperado
                    && !tareasUI[i].text.StartsWith("<s>"))
                {
                    tareasUI[i].text  = $"<s>{tareasPorBloque[bloqueActual][i].Descripcion}</s>";
                    tareasUI[i].color = new Color(0.5f, 0.5f, 0.5f, 1f);

                    int completadas = 0;
                    foreach (var t in tareasUI)
                        if (t.text.StartsWith("<s>")) completadas++;

                    if (completadas >= tareasPorBloque[bloqueActual].Length)
                    {
                        int siguienteBloque = bloqueActual + 1;
                        if (siguienteBloque < titulosBloques.Length)
                            CargarBloque(siguienteBloque);
                        else
                            MostrarCompletado();
                    }
                    return;
                }
            }
            return;
        }

        string comandoEsperado = tareasPorBloque[bloqueActual][tareaActual].ComandoEsperado;
        if (cmdTrim == comandoEsperado)
            CompletarTareaActual();
    }
}