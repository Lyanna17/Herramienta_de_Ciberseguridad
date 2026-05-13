using UnityEngine;
using TMPro;
using System;
using System.Data;

public class ModulosManager : MonoBehaviour
{
    [Header("TopBar")]
    public TextMeshProUGUI txtNombre;
    public TextMeshProUGUI txtIniciales;

    [Header("Footer")]
    public TextMeshProUGUI txtUltimoAcceso;

    void Start()
    {
        string nombre = PlayerPrefs.GetString("NombreEstudiante", "Estudiante");
        txtNombre.text = nombre;
        txtIniciales.text = nombre.Length >= 2
            ? nombre.Substring(0, 2).ToUpper()
            : nombre.ToUpper();

        string ultimoAccesoRaw = PlayerPrefs.GetString("UltimoAcceso", "");

        if (!string.IsNullOrEmpty(ultimoAccesoRaw))
        {
            DateTime fecha = DateTime.Parse(ultimoAccesoRaw);
            DateTime fechaLocal = fecha.ToLocalTime();
            txtUltimoAcceso.text = "Último acceso: " + fechaLocal.ToString("dd/MM/yyyy HH:mm");
        }
        else
        {
            txtUltimoAcceso.text = "Último acceso: sin registro";
        }
    }
}