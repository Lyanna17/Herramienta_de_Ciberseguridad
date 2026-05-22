using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ProgresoManager : MonoBehaviour
{
    [Header("Referencia al TaskManager del módulo activo")]
    public TaskManager taskManager;

    [Header("ID del módulo actual (1 = Comandos Básicos, 2 = Contraseñas)")]
    public int moduloId = 1;

    private const string URL_GUARDAR   = "http://127.0.0.1:8000/progreso/guardar";
    private const string URL_CONSULTAR = "http://127.0.0.1:8000/progreso/consultar";
    private string usuario;

    void Start()
    {
        usuario = PlayerPrefs.GetString("UsuarioActual", "");

        if (taskManager != null)
            taskManager.OnBloqueCompletado += OnBloqueCompletado;

        // Al cargar la escena, consulta en qué bloque quedó
        if (!string.IsNullOrEmpty(usuario))
            StartCoroutine(ConsultarProgreso());
    }

    void OnDestroy()
    {
        if (taskManager != null)
            taskManager.OnBloqueCompletado -= OnBloqueCompletado;
    }

    private void OnBloqueCompletado(int bloquesCompletados, int totalBloques)
    {
        // bloqueActual ya avanzó, entonces bloque_actual = bloquesCompletados
        StartCoroutine(EnviarProgreso(bloquesCompletados, totalBloques, bloquesCompletados));
    }

    private IEnumerator ConsultarProgreso()
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
            Debug.LogWarning("[ProgresoManager] Error al consultar: " + req.error);
            taskManager.ReanudarDesdeBloque(0);
            yield break;
        }

        ProgresoResponse resp = JsonUtility.FromJson<ProgresoResponse>(req.downloadHandler.text);
        if (resp == null || !resp.success)
        {
            taskManager.ReanudarDesdeBloque(0);
            yield break;
        }

        bool encontrado = false;
        foreach (var p in resp.progresos)
        {
            if (p.modulo_id == moduloId)
            {
                Debug.Log($"[ProgresoManager] Reanudando desde bloque {p.bloque_actual}");
                taskManager.ReanudarDesdeBloque(p.bloque_actual);
                encontrado = true;
                break;
            }
        }

        if (!encontrado)
            taskManager.ReanudarDesdeBloque(0);
    }

    private IEnumerator EnviarProgreso(int bloquesCompletados, int totalBloques, int bloqueActual)
    {
        string json = $"{{" +
            $"\"usuario\":\"{usuario}\"," +
            $"\"modulo_id\":{moduloId}," +
            $"\"bloques_completados\":{bloquesCompletados}," +
            $"\"total_bloques\":{totalBloques}," +
            $"\"bloque_actual\":{bloqueActual}" +
            $"}}";

        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        using UnityWebRequest req = new UnityWebRequest(URL_GUARDAR, "POST");
        req.uploadHandler   = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Accept",       "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogWarning("[ProgresoManager] Error al guardar: " + req.error);
        else
            Debug.Log("[ProgresoManager] Progreso guardado: " + req.downloadHandler.text);
    }

    [System.Serializable]
    private class ProgresoItem
    {
        public int  modulo_id;
        public int  bloques_completados;
        public int  total_bloques;
        public int  bloque_actual;
        public bool completado;
    }

    [System.Serializable]
    private class ProgresoResponse
    {
        public bool          success;
        public ProgresoItem[] progresos;
    }
}