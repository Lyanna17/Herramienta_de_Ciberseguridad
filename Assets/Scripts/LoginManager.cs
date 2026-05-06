using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("Campos")]
    public TMP_InputField inputUsuario;
    public TMP_InputField inputContrasena;

    [Header("Feedback")]
    public TextMeshProUGUI textoError;
    public Button botonLogin;

    private const string URL_LOGIN = "http://127.0.0.1:8000/login-check";
    private const string ESCENA_SIGUIENTE = "modulos";

    public void OnBotonLoginClick()
    {
        string usuario    = inputUsuario.text.Trim();
        string contrasena = inputContrasena.text;

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
        {
            textoError.text = "Por favor completa todos los campos.";
            return;
        }

        botonLogin.interactable = false;
        textoError.text = "";
        StartCoroutine(EnviarLogin(usuario, contrasena));
    }

    private IEnumerator EnviarLogin(string usuario, string contrasena)
    {
        string json    = $"{{\"usuario\":\"{usuario}\",\"contrasena\":\"{contrasena}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new UnityWebRequest(URL_LOGIN, "POST");
        request.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        botonLogin.interactable = true;

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.DataProcessingError)
        {
            textoError.text = "Error de conexión con el servidor.";
            yield break;
        }

        LoginResponse resp = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

        if (resp.success)
        {
            PlayerPrefs.SetString("NombreEstudiante", resp.nombre);
            PlayerPrefs.SetString("UsuarioActual", inputUsuario.text.Trim());
            PlayerPrefs.SetInt("TutorialCompletado", resp.tutorial_completado ? 1 : 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(ESCENA_SIGUIENTE);
        }
        else
        {
            textoError.text = resp.message;
        }
    }

    [System.Serializable]
    private class LoginResponse
    {
        public bool   success;
        public string message;
        public string nombre;
        public bool   tutorial_completado;
    }
}