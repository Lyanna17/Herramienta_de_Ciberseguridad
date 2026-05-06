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

    Interpreter interpreter;

    void Start()
    {
        interpreter = GetComponent<Interpreter>();
    }

    private void OnGUI()
    {
        if(terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            //Guardar lo que el usuario escribio
            string userInput = terminalInput.text;

            //Limpiar el input field
            ClearInputField();

            //Instanciar gameobject 
            AddDirectoryLine(userInput);

            //Añadir la interpretacion
            int lines = AddInterpreterLines(interpreter.Interpret(userInput));

            //Scroll al final del scrollrect
            ScrollToBottom(lines);

            //Mover el user input line al final
            userInputLine.transform.SetAsLastSibling();

            //Enforcar de nuevo el input field
            terminalInput.ActivateInputField();
            terminalInput.Select();
    
        }
    }
    
    void ClearInputField()
    {
        terminalInput.text = "";
    }

    void AddDirectoryLine(string userInput)
    {
        //Vovler a poner el tamañado del command line container
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 30.0f);

        //Instanciar el directory line
        GameObject msg = Instantiate(directoryLine, msgList.transform);

        //Poner child index
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);

        //Poner texto al nuevo gameobject
        msg.GetComponentsInChildren<Text>()[1].text = interpreter.GetComponent<Interpreter>().name + " $ " + userInput;
    }

    int AddInterpreterLines(List<string> interpretation)
    {
        for(int i = 0; i < interpretation.Count; i++)
        {
            //Instanciar el response line
            GameObject res = Instantiate(responseLine, msgList.transform);

            //Ponerlo al final de los mensajes
            res.transform.SetAsLastSibling();

            //Obtener el tamaño de message list y volver al tamaño
            Vector2 listSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(listSize.x, listSize.y + 25.0f);

            res.GetComponentInChildren<Text>().text = interpretation[i];
        }

        return interpretation.Count;
    }

    void ScrollToBottom(int lines)
    {
        if(lines > 10)
        {
            sr.velocity = new Vector2(0, 450);
        }

        else
        {
            sr.verticalNormalizedPosition = 0;
        }
    }
}
