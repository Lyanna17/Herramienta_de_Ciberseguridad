using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TutorialManager : MonoBehaviour
{
    [Header("Overlay")]
    public GameObject overlay;

    [Header("Welcome Panel")]
    public GameObject welcomePanel;
    public Button btnContinuar;

    [Header("Spotlight - Terminal")]
    public GameObject terminalIcon;
    public GameObject terminalTutorialPanel;
    public Button btnListoTerminal;

    [Header("Spotlight - Libro")]
    public GameObject libroIcon;
    public GameObject libroTutorialPanel;
    public Button btnListoLibro;

    [Header("Spotlight - BttonTask")]
    public GameObject bttonTaskIcon;
    public GameObject bttonTaskPanel;
    public Button btnListoTask;

    [Header("Animacion")]
    public float fadeSpeed = 1.5f;
    public float moveSpeed = 0.4f;

    private CanvasGroup canvasGroup;
    private Canvas tutorialCanvas;
    private GraphicRaycaster tutorialRaycaster;
    private Canvas libroCanvas;
    private GraphicRaycaster libroRaycaster;
    private Canvas taskCanvas;
    private GraphicRaycaster taskRaycaster;

    void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompletado", 0) == 1)
        {
            overlay.SetActive(false);
            welcomePanel.SetActive(false);
            terminalTutorialPanel.SetActive(false);
            libroTutorialPanel.SetActive(false);
            bttonTaskPanel.SetActive(false);
            return;
        }

        canvasGroup = welcomePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = welcomePanel.AddComponent<CanvasGroup>();

        welcomePanel.SetActive(true);
        terminalTutorialPanel.SetActive(false);
        libroTutorialPanel.SetActive(false);
        bttonTaskPanel.SetActive(false);
        canvasGroup.alpha = 0f;

        btnContinuar.onClick.AddListener(() => StartCoroutine(FadeOutThenSpotlightTerminal()));
        btnListoTerminal.onClick.AddListener(() => StartCoroutine(MoveSpotlightToLibro()));
        btnListoLibro.onClick.AddListener(() => StartCoroutine(MoveSpotlightToTask()));
        btnListoTask.onClick.AddListener(CerrarTutorial);

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOutThenSpotlightTerminal()
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        welcomePanel.SetActive(false);

        tutorialCanvas = terminalIcon.AddComponent<Canvas>();
        tutorialCanvas.overrideSorting = true;
        tutorialCanvas.sortingOrder = 10;
        tutorialRaycaster = terminalIcon.AddComponent<GraphicRaycaster>();

        terminalTutorialPanel.SetActive(true);
    }

    IEnumerator MoveSpotlightToLibro()
    {
        terminalTutorialPanel.SetActive(false);

        RectTransform terminalRect = terminalIcon.GetComponent<RectTransform>();
        RectTransform libroRect    = libroIcon.GetComponent<RectTransform>();
        RectTransform panelRect    = terminalTutorialPanel.GetComponent<RectTransform>();

        Vector2 startPos = panelRect.anchoredPosition;
        Vector2 endPos   = panelRect.anchoredPosition +
                           (libroRect.anchoredPosition - terminalRect.anchoredPosition);

        terminalTutorialPanel.SetActive(true);

        float elapsed = 0f;
        while (elapsed < moveSpeed)
        {
            elapsed += Time.deltaTime;
            float t  = elapsed / moveSpeed;
            t        = Mathf.SmoothStep(0f, 1f, t);
            panelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        terminalTutorialPanel.SetActive(false);

        if (tutorialRaycaster != null) Destroy(tutorialRaycaster);
        yield return null;
        if (tutorialCanvas != null) Destroy(tutorialCanvas);

        libroCanvas = libroIcon.AddComponent<Canvas>();
        libroCanvas.overrideSorting = true;
        libroCanvas.sortingOrder = 10;
        libroRaycaster = libroIcon.AddComponent<GraphicRaycaster>();

        libroTutorialPanel.SetActive(true);
    }

    IEnumerator MoveSpotlightToTask()
    {
        libroTutorialPanel.SetActive(false);

        RectTransform libroRect    = libroIcon.GetComponent<RectTransform>();
        RectTransform taskRect     = bttonTaskIcon.GetComponent<RectTransform>();
        RectTransform panelRect    = libroTutorialPanel.GetComponent<RectTransform>();

        Vector2 startPos = panelRect.anchoredPosition;
        Vector2 endPos   = panelRect.anchoredPosition +
                           (taskRect.anchoredPosition - libroRect.anchoredPosition);

        libroTutorialPanel.SetActive(true);

        float elapsed = 0f;
        while (elapsed < moveSpeed)
        {
            elapsed += Time.deltaTime;
            float t  = elapsed / moveSpeed;
            t        = Mathf.SmoothStep(0f, 1f, t);
            panelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        libroTutorialPanel.SetActive(false);

        if (libroRaycaster != null) Destroy(libroRaycaster);
        yield return null;
        if (libroCanvas != null) Destroy(libroCanvas);

        taskCanvas = bttonTaskIcon.AddComponent<Canvas>();
        taskCanvas.overrideSorting = true;
        taskCanvas.sortingOrder = 10;
        taskRaycaster = bttonTaskIcon.AddComponent<GraphicRaycaster>();

        bttonTaskPanel.SetActive(true);
    }

    void CerrarTutorial()
    {
        if (taskRaycaster != null) Destroy(taskRaycaster);
        StartCoroutine(DestroyCanvasAfterFrame());

        bttonTaskPanel.SetActive(false);
        overlay.SetActive(false);
        welcomePanel.SetActive(false);

        FindObjectOfType<TaskManager>().ToggleTaskPanel();

        StartCoroutine(MarcarTutorialCompletado());
    }

    IEnumerator DestroyCanvasAfterFrame()
    {
        yield return null;
        if (taskCanvas != null) Destroy(taskCanvas);
    }

    IEnumerator MarcarTutorialCompletado()
    {
        string usuario = PlayerPrefs.GetString("UsuarioActual", "");
        if (string.IsNullOrEmpty(usuario)) yield break;

        string json    = $"{{\"usuario\":\"{usuario}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:8000/tutorial-completado", "POST");
        request.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.SetInt("TutorialCompletado", 1);
            PlayerPrefs.Save();
        }
    }
}