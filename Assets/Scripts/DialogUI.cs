using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public GameObject panel;
    public GameObject descriptionPanel;
    public TMP_Text dialogueText;
    public TMP_Text descriptionText;
    public int selected;
    private int options;
    public GameObject button;
    public GameObject buttonParent;
    public string entityname;

    private Dictionary<int,GameObject> buttons = new Dictionary<int,GameObject>();

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        descriptionPanel.SetActive(false);
    }

    public void Show(string name, string text)
    {
        entityname = name;
        descriptionText.text = entityname + "\n\"" + text+ "\"";
        panel.SetActive(true);
        descriptionPanel.SetActive(true);
    }

    public void SetText(string text)
    {
        descriptionText.text = entityname + "\n\"" + text + "\"";
    }

    public void SetMenuTitle(string text)
    {
        dialogueText.text = text;
    }

    public string GetAnswer()
    {
        return buttons[selected].transform.Find("MenuItemName").gameObject.GetComponent<TMP_Text>().text;
    }

    public void LateUpdate(){
        if (selected >= options) {
            selected = 0;
        }
        if (selected < 0) {
            selected = options-1;
        }
        foreach( KeyValuePair<int, GameObject> btn in buttons )
        {

            btn.Value.transform.Find("Icon").gameObject.SetActive(btn.Key == selected);
        }
    }

    public void SetDialogueTitle(string name){
        entityname = name;
    }


    public void SetMenu(string menuname, string[] menu)
    {
        buttons = new Dictionary<int,GameObject>();
        string text = menuname + "\n\n";
        options = menu.Length;
        foreach (Transform child in buttonParent.transform) {
            GameObject.Destroy(child.gameObject);
        }
        selected = 0;
        foreach (string item in menu)
        {
           buttons.Add(selected,Instantiate(button,buttonParent.transform));
           buttons[selected].transform.Find("MenuItemName").gameObject.GetComponent<TMP_Text>().text = item;
           selected++;
        }
        selected = 0;
        dialogueText.text = text;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
