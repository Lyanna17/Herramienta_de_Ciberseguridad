using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using UnityEngine.Networking;

public class ModulosManager : MonoBehaviour
{
    [Header("TopBar")]
    public TextMeshProUGUI txtNombre;
    public TextMeshProUGUI txtIniciales;

    [Header("Footer")]
    public TextMeshProUGUI txtUltimoAcceso;

    [Header("Barras de progreso (orden: modulo1, modulo2, ...)")]
    public Image[] barrasProgreso;

    private const string URL_CONSULTAR = "http://127.0.0.1:8000/progreso/consultar";

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
            txtUltimoAcceso.text = "Último acceso: " + fecha.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        }
        else
        {
            txtUltimoAcceso.text = "Último acceso: sin registro";
        }

        string usuario = PlayerPrefs.GetString("UsuarioActual", "");
        if (!string.IsNullOrEmpty(usuario))
            StartCoroutine(CargarProgresos(usuario));
    }

    private IEnumerator CargarProgresos(string usuario)
    {
        string json = $"{{\"usuario\":\"{usuario}\"}}";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        using UnityWebRequest req = new UnityWebRequest(URL_CONSULTAR, "POST");
        req.uploadHandler   = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Accept",       "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("[ModulosManager] No se pudo cargar el progreso: " + req.error);
            yield break;
        }

        ProgresoResponse resp = JsonUtility.FromJson<ProgresoResponse>(req.downloadHandler.text);
        if (resp == null || !resp.success) yield break;

        foreach (var p in resp.progresos)
        {
            int index = p.modulo_id - 1;
            if (index >= 0 && index < barrasProgreso.Length && barrasProgreso[index] != null)
            {
                float porcentaje = p.total_bloques > 0
                    ? (float)p.bloques_completados / p.total_bloques
                    : 0f;
                barrasProgreso[index].fillAmount = porcentaje;
            }
        }
    }

    [System.Serializable]
    private class ProgresoItem
    {
        public int  modulo_id;
        public int  bloques_completados;
        public int  total_bloques;
        public bool completado;
    }

    [System.Serializable]
    private class ProgresoResponse
    {
        public bool           success;
        public ProgresoItem[] progresos;
    }
}