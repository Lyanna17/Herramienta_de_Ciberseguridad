using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TaskManager2 : MonoBehaviour
{
    [Header("Panel de Tareas")]
    public GameObject taskPanel;

    [Header("UI Referencias")]
    public TextMeshProUGUI txtFase;      // Título de la fase actual
    public TextMeshProUGUI txtTarea1;    // Hasta 3 tareas visibles por fase
    public TextMeshProUGUI txtTarea2;
    public TextMeshProUGUI txtTarea3;
    public TextMeshProUGUI txtProgreso;  // "Fase 1 de 7"
    public TextMeshProUGUI txtPista;     // Hint del comando a usar

    private bool panelVisible = false;
    private int  faseActual   = 0;

    // ── Estructura de datos ───────────────────────────────
    private class Tarea
    {
        public string Descripcion;
        public string ComandoEsperado;
        public bool   Completada;
    }

    private class Fase
    {
        public string       Titulo;
        public string       Pista;      // hint general de la fase
        public List<Tarea>  Tareas;
        public bool         Completada => Tareas.TrueForAll(t => t.Completada);
    }

    private List<Fase> fases;

    // ─────────────────────────────────────────────────────
    void Start()
    {
        taskPanel.SetActive(false);
        InicializarFases();
        RenderizarFaseActual();
    }

    void InicializarFases()
    {
        fases = new List<Fase>
        {
            new Fase {
                Titulo = "FASE 1 — Reconocimiento",
                Pista  = "Comandos: ifconfig  |  ping -c 4 <ip>",
                Tareas = new List<Tarea> {
                    new Tarea { Descripcion = "1. Identifica tu IP con ifconfig",
                                ComandoEsperado = "ifconfig" },
                    new Tarea { Descripcion = "2. Verifica que 192.168.1.200 está activo",
                                ComandoEsperado = "ping" },
                }
            },

            new Fase {
                Titulo = "FASE 2 — Escaneo de Puertos",
                Pista  = "Comandos: nmap <ip>  |  nmap -sV <ip>",
                Tareas = new List<Tarea> {
                    new Tarea { Descripcion = "3. Escanea los puertos del objetivo",
                                ComandoEsperado = "nmap 192.168.1.200" },
                    new Tarea { Descripcion = "4. Identifica versiones de servicios",
                                ComandoEsperado = "nmap -sv 192.168.1.200" },
                }
            },

            new Fase {
                Titulo = "FASE 3 — Preparar el Ataque",
                Pista  = "Comandos: ls /usr/share/wordlists  |  cat <archivo>",
                Tareas = new List<Tarea> {
                    new Tarea { Descripcion = "5. Revisa las wordlists disponibles",
                                ComandoEsperado = "ls /usr/share/wordlists" },
                    new Tarea { Descripcion = "6. Lee el contenido de rockyou.txt",
                                ComandoEsperado = "cat /usr/share/wordlists/rockyou.txt" },
                }
            },

            new Fase {
                Titulo = "FASE 4 — Ataque de Fuerza Bruta",
                Pista  = "Comando: hydra -l admin -P <wordlist> ssh://<ip>",
                Tareas = new List<Tarea> {
                    new Tarea { Descripcion = "7. Lanza Hydra contra el SSH del objetivo",
                                ComandoEsperado = "hydra" },
                }
            },

            new Fase {
                Titulo = "FASE 5 — Acceso al Servidor",
                Pista  = "Comandos: ssh admin@192.168.1.200  |  contraseña: batman123",
                Tareas = new List<Tarea> {
                    new Tarea { Descripcion = "8. Conéctate por SSH al objetivo",
                                ComandoEsperado = "ssh admin@192.168.1.200" },
                    new Tarea { Descripcion = "9. Ingresa la contraseña correcta",
                                ComandoEsperado = "batman123" },
                }
            },

            new Fase {
                Titulo = "FASE 6 — Post-Explotación",
                Pista  = "Comandos: whoami  |  ls  |  cat  |  cd",
                Tareas = new List<Tarea> {
                    new Tarea { Descripcion = "10. Confirma tu usuario con whoami",
                                ComandoEsperado = "whoami" },
                    new Tarea { Descripcion = "11. Lista y lee notas.txt",
                                ComandoEsperado = "cat notas.txt" },
                    new Tarea { Descripcion = "12. Entra a backup y lee config.bak",
                                ComandoEsperado = "cat config.bak" },
                }
            },

            new Fase {
                Titulo = "FASE 7 — Escalada de Directorios",
                Pista  = "Comandos: cd /root  |  ls  |  cat flag.txt",
                Tareas = new List<Tarea> {
                    new Tarea { Descripcion = "13. Navega a /root",
                                ComandoEsperado = "cd /root" },
                    new Tarea { Descripcion = "14. Lee secreto.txt",
                                ComandoEsperado = "cat secreto.txt" },
                    new Tarea { Descripcion = "15. Lee flag.txt y completa el módulo",
                                ComandoEsperado = "cat flag.txt" },
                }
            },
        };
    }

    // ── Renderizar solo la fase actual ────────────────────
    void RenderizarFaseActual()
    {
        if (faseActual >= fases.Count)
        {
            MostrarCompletado();
            return;
        }

        Fase fase = fases[faseActual];

        // Título y progreso
        txtFase.text     = fase.Titulo;
        txtProgreso.text = $"Fase {faseActual + 1} de {fases.Count}";
        txtPista.text    = fase.Pista;

        // Limpiar labels
        txtTarea1.text = "";
        txtTarea2.text = "";
        txtTarea3.text = "";

        // Asignar tareas de esta fase (máximo 3)
        TextMeshProUGUI[] labels = { txtTarea1, txtTarea2, txtTarea3 };

        for (int i = 0; i < fase.Tareas.Count && i < labels.Length; i++)
            ActualizarLabel(labels[i], fase.Tareas[i]);
    }

    void ActualizarLabel(TextMeshProUGUI label, Tarea tarea)
    {
        if (tarea.Completada)
        {
            label.text  = $"<s>{tarea.Descripcion}</s>";
            label.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            label.text  = $"○ {tarea.Descripcion}";
            label.color = Color.white;
        }
    }

    void MostrarCompletado()
    {
        txtFase.text     = "✓ ¡MÓDULO 2 COMPLETADO!";
        txtProgreso.text = $"Fase {fases.Count} de {fases.Count}";
        txtPista.text    = "Encuentra la flag en /root/flag.txt";
        txtTarea1.text   = "Has completado todas las fases.";
        txtTarea1.color  = new Color(0.4f, 1f, 0.4f);
        txtTarea2.text   = "";
        txtTarea3.text   = "";
    }

    // ── Llamado desde Interpreter con cada comando ────────
    public void OnComandoEjecutado(string comando)
    {
        if (faseActual >= fases.Count) return;

        string     cmdLower  = comando.Trim().ToLower();
        Fase       fase      = fases[faseActual];
        TextMeshProUGUI[] labels = { txtTarea1, txtTarea2, txtTarea3 };

        foreach (var tarea in fase.Tareas)
        {
            if (tarea.Completada) continue;

            string esperado = tarea.ComandoEsperado.ToLower();
            bool   coincide = cmdLower == esperado
                           || cmdLower.StartsWith(esperado)
                           || cmdLower.Contains(esperado);

            if (coincide)
            {
                tarea.Completada = true;

                // Actualizar el label correspondiente
                for (int i = 0; i < fase.Tareas.Count && i < labels.Length; i++)
                    ActualizarLabel(labels[i], fase.Tareas[i]);

                // Si la fase quedó completa, avanzar a la siguiente
                if (fase.Completada)
                {
                    faseActual++;
                    RenderizarFaseActual();
                }

                break;
            }
        }
    }

    public void ToggleTaskPanel()
    {
        panelVisible = !panelVisible;
        taskPanel.SetActive(panelVisible);
    }
}