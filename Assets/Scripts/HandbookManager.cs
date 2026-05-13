using UnityEngine;
using UnityEngine.UI;

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
    private Color colorOriginal;
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
    }

    public void MostrarDetalle(Sprite sprite)
    {
        imagenDetalle.gameObject.SetActive(true);
        imagenDetalle.sprite = sprite;
    }

    public void SeleccionarBoton(Button boton)
    {
        if (botonActual != null)
            botonActual.GetComponent<Image>().color = colorNormal;

        botonActual = boton;
        botonActual.GetComponent<Image>().color = colorSeleccionado;
    }

    public void DesseleccionarBoton()
    {
        if (botonActual != null)
        {
            botonActual.GetComponent<Image>().color = colorNormal;
            botonActual = null;
        }
    }

    public void MostrarDetalle_ls()      { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_ls); }
    public void MostrarDetalle_cd()      { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_cd); }
    public void MostrarDetalle_pwd()     { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_pwd); }
    public void MostrarDetalle_cat()     { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_cat); }
    public void MostrarDetalle_rm()      { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_rm); }
    public void MostrarDetalle_chmod()   { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_chmod); }
    public void MostrarDetalle_chown()   { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_chown); }
    public void MostrarDetalle_shutdown(){ SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_shutdown); }
    public void MostrarDetalle_su()      { SeleccionarBoton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>()); MostrarDetalle(sprite_su); }
}