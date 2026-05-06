using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    private const string URL_BASE = "http://127.0.0.1:8000";

    public void Logout()
    {
        PlayerPrefs.DeleteKey("NombreEstudiante");
        PlayerPrefs.DeleteKey("UsuarioActual");
        PlayerPrefs.DeleteKey("TutorialCompletado");
        PlayerPrefs.Save();

        SceneManager.LoadScene("login");
    }
}