using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{
    public GameObject directoryLine;
    public GameObject responseLine;

    public InputField terminalInput;
    public GameObject userInputLine;
    public ScrollRect sr;
    public GameObject msgList;

    float initialMsgListHeight;

    Interpreter interpreter;
    TaskManager taskManager;
    TaskManager2 taskManager2;

    void Start()
    {
        interpreter = GetComponent<Interpreter>();
        initialMsgListHeight = msgList.GetComponent<RectTransform>().sizeDelta.y;
        taskManager  = FindObjectOfType<TaskManager>();
        taskManager2 = FindObjectOfType<TaskManager2>();
    }

    private void OnGUI()
    {
        if (terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            string userInput = terminalInput.text;
            ClearInputField();

            List<string> interpretation = interpreter.Interpret(userInput);

            if (interpretation.Count == 1 && interpretation[0] == "%%CLEAR%%")
            {
                ClearTerminal();
                userInputLine.transform.SetAsLastSibling();
                terminalInput.ActivateInputField();
                terminalInput.Select();
                return;
            }

            string displayInput = interpreter.EsperandoPassword()
                ? new string('*', userInput.Length)
                : userInput;

            AddDirectoryLine(displayInput);

            int lines = AddInterpreterLines(interpretation);
            ScrollToBottom(lines);
            userInputLine.transform.SetAsLastSibling();
            terminalInput.ActivateInputField();
            terminalInput.Select();

            // Notifica al TaskManager que corresponda según la escena activa
            if (taskManager != null)
                taskManager.OnComandoEjecutado(userInput.Trim());

            if (taskManager2 != null)
                taskManager2.OnComandoEjecutado(userInput.Trim());
        }
    }

    void ClearInputField()
    {
        terminalInput.text = "";
    }

    void ClearTerminal()
    {
        var toDestroy = new List<GameObject>();
        foreach (Transform child in msgList.transform)
        {
            if (child.gameObject != userInputLine)
                toDestroy.Add(child.gameObject);
        }
        foreach (GameObject go in toDestroy)
            Destroy(go);

        RectTransform rt = msgList.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, initialMsgListHeight);

        sr.verticalNormalizedPosition = 1f;
    }

    void AddDirectoryLine(string userInput)
    {
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 30.0f);

        GameObject msg = Instantiate(directoryLine, msgList.transform);
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);
        msg.GetComponentsInChildren<Text>()[1].text = interpreter.GetComponent<Interpreter>().name + " $ " + userInput;
    }

    int AddInterpreterLines(List<string> interpretation)
    {
        for (int i = 0; i < interpretation.Count; i++)
        {
            GameObject res = Instantiate(responseLine, msgList.transform);
            res.transform.SetAsLastSibling();

            Vector2 listSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(listSize.x, listSize.y + 25.0f);

            res.GetComponentInChildren<Text>().text = interpretation[i];
        }
        return interpretation.Count;
    }

    void ScrollToBottom(int lines)
    {
        if (lines > 10)
            sr.velocity = new Vector2(0, 450);
        else
            sr.verticalNormalizedPosition = 0;
    }
}