using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    Vector3 currentPos = new Vector3(0, 1, -10);

    VisualElement root;

    Dictionary<string, VisualElement> windows = new();
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        foreach(VisualElement child in root.Children())
        {
            windows.Add(child.name, child);
        }

        #region Menu Buttons
        Button playButton = windows["Menu"].Q<Button>("PlayButton");
        playButton.clicked +=  delegate { SceneManager.LoadScene("Game"); };
        Button settingButton = windows["Menu"].Q<Button>("SettingButton");
        settingButton.clicked += delegate { ChangeWindow("Setting"); };
        Button exitButton = windows["Menu"].Q<Button>("ExitButton");
        exitButton.clicked += delegate { Application.Quit(); };
        #endregion

        #region Setting Buttons
        #endregion

        #region Play Buttons
        #endregion
    }

    string currentWindow = "Menu";
    void ChangeWindow(string window)
    {
        if (window == "Main")
            currentPos = new Vector3(0, 1, -10);
        else currentPos = new Vector3(0, 1, 100);

        windows[currentWindow].AddToClassList("hide-current-menu");
        currentWindow = window;
        windows[currentWindow].RemoveFromClassList("hide-current-menu");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime);
    }
}
