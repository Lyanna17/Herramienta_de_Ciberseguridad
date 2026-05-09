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
    private Color colorOriginal;

    void Start()
    {
        contenidoMenu.SetActive(true);
        contenidoLista.SetActive(false);
        imagenDetalle.gameObject.SetActive(false);
    }

    public void AbrirLista()
    {
        contenidoMenu.SetActive(false);
        contenidoLista.SetActive(true);
    }

    public void VolverMenu()
    {
        contenidoLista.SetActive(false);
        contenidoMenu.SetActive(true);
        imagenDetalle.gameObject.SetActive(false);
        DesseleccionarBoton();
    }

    void MostrarSprite(Sprite sprite)
    {
        Button boton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        imagenDetalle.gameObject.SetActive(true);
        imagenDetalle.sprite = sprite;
        SeleccionarBoton(boton);
    }

    void SeleccionarBoton(Button boton)
    {
        DesseleccionarBoton();
        botonActual = boton;
        colorOriginal = boton.colors.normalColor;
        SetColorBoton(botonActual, colorSeleccionado);
    }

    void DesseleccionarBoton()
    {
        if (botonActual != null)
            SetColorBoton(botonActual, colorOriginal);
        botonActual = null;
    }

    void SetColorBoton(Button boton, Color color)
    {
        ColorBlock cb = boton.colors;
        cb.normalColor = color;
        cb.selectedColor = color;
        boton.colors = cb;
    }

    public void MostrarComando_ls()       { MostrarSprite(sprite_ls); }
    public void MostrarComando_cd()       { MostrarSprite(sprite_cd); }
    public void MostrarComando_pwd()      { MostrarSprite(sprite_pwd); }
    public void MostrarComando_cat()      { MostrarSprite(sprite_cat); }
    public void MostrarComando_rm()       { MostrarSprite(sprite_rm); }
    public void MostrarComando_chmod()    { MostrarSprite(sprite_chmod); }
    public void MostrarComando_chown()    { MostrarSprite(sprite_chown); }
    public void MostrarComando_shutdown() { MostrarSprite(sprite_shutdown); }
    public void MostrarComando_su()       { MostrarSprite(sprite_su); }
}