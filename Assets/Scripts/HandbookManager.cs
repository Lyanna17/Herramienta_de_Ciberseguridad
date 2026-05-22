using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandbookManager : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject contenidoMenu;
    public GameObject contenidoLista;

    [Header("Detalle")]
    public Image imagenDetalle;

    [Header("Sprites Comandos")]
    public Sprite sprite_ls;
    public Sprite sprite_cd;
    public Sprite sprite_pwd;
    public Sprite sprite_cat;
    public Sprite sprite_rm;
    public Sprite sprite_chmod;
    public Sprite sprite_chown;
    public Sprite sprite_shutdown;
    public Sprite sprite_su;

    [Header("Colores Lista")]
    public Color colorNormal = new Color(0.12f, 0.12f, 0.12f);
    public Color colorSeleccionado = new Color(0.23f, 0.23f, 0.23f);

    private Button botonActual;
    private TaskManager taskManager;
    public bool enLista = false;

    void Start()
    {
        contenidoMenu.SetActive(true);
        contenidoLista.SetActive(false);
        imagenDetalle.gameObject.SetActive(false);
        taskManager = FindObjectOfType<TaskManager>();
    }

    public void AbrirLista()
    {
        enLista = true;
        contenidoMenu.SetActive(false);
        contenidoLista.SetActive(true);
    }

    public void VolverMenu()
    {
        enLista = false;
        contenidoLista.SetActive(false);
        contenidoMenu.SetActive(true);
        imagenDetalle.gameObject.SetActive(false);
        DesseleccionarBoton();
        if (taskManager != null) taskManager.OnComandoEjecutado("Explora Terminal Commands");
    }

    public void MostrarDetalle(Sprite sprite)
    {
        imagenDetalle.gameObject.SetActive(true);
        imagenDetalle.sprite = sprite;
    }

    void SeleccionarBoton(Button boton)
    {
        DesseleccionarBoton();
        botonActual = boton;
        if (botonActual != null)
            botonActual.GetComponent<Image>().color = colorSeleccionado;
    }

    void DesseleccionarBoton()
    {
        if (botonActual != null)
        {
            botonActual.GetComponent<Image>().color = colorNormal;
            botonActual = null;
        }
    }

    void MostrarComando(Sprite sprite)
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
            SeleccionarBoton(go.GetComponent<Button>());
        MostrarDetalle(sprite);
    }

    public void MostrarDetalle_ls()       { MostrarComando(sprite_ls); }
    public void MostrarDetalle_cd()       { MostrarComando(sprite_cd); }
    public void MostrarDetalle_pwd()      { MostrarComando(sprite_pwd); }
    public void MostrarDetalle_cat()      { MostrarComando(sprite_cat); }
    public void MostrarDetalle_rm()       { MostrarComando(sprite_rm); }
    public void MostrarDetalle_chmod()    { MostrarComando(sprite_chmod); }
    public void MostrarDetalle_chown()    { MostrarComando(sprite_chown); }
    public void MostrarDetalle_shutdown() { MostrarComando(sprite_shutdown); }
    public void MostrarDetalle_su()       { MostrarComando(sprite_su); }
}