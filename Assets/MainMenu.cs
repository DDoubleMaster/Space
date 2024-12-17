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
    void OnEnable()
    {
        Time.timeScale = 1;
        root = GetComponent<UIDocument>().rootVisualElement;

        currentPos = new Vector3(0, 1, -10);
        Invoke("LoadAction", 3);

        foreach (VisualElement child in root.Children())
            windows.Add(child.name, child);

        #region Menu Buttons
        Button playButton = windows["Menu"].Q<Button>("PlayButton");
        playButton.clicked += delegate { PlayAction(); };
        Button settingButton = windows["Menu"].Q<Button>("SettingButton");
        settingButton.clicked += delegate {  };
        Button exitButton = windows["Menu"].Q<Button>("ExitButton");
        exitButton.clicked += delegate { Application.Quit(); };
        #endregion

        #region Setting Buttons
        #endregion

        #region Play Buttons
        #endregion
    }

    void LoadAction()
    {
        root.Q<VisualElement>("Menu").RemoveFromClassList("hide-current-window");
    }

    void PlayAction()
    {
        foreach (VisualElement child in root.Children())
            child.AddToClassList("hide-current-window");

        currentPos = new Vector3(0, 1, 100);
        Invoke("TempMove", 3);
    }

    void TempMove()
    {
        SceneManager.LoadScene("Game");
    }

    string currentWindow = "Menu";
    void ChangeWindow(string window)
    {
        //if (window == "Main")
        //    currentPos = new Vector3(0, 1, -10);
        //else currentPos = new Vector3(0, 1, 100);

        windows[currentWindow].AddToClassList("hide-current-window");
        currentWindow = window;
        windows[currentWindow].RemoveFromClassList("hide-current-window");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentPos);
        transform.position = Vector3.Lerp(transform.position, currentPos, 2 * Time.deltaTime);
    }
}
