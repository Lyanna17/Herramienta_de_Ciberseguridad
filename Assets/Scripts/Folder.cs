using UnityEngine;
using UnityEngine.UI;

public class Folder : MonoBehaviour
{
    public Text nameText;

    public void SetName(string folderName)
    {
        nameText.text = folderName;
    }
}