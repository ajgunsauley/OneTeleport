using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public GameObject buttonPrefab;
    public GridLayoutGroup gridLayout;
    public FadeManager fader;
    public string[] scenes;

#if UNITY_EDITOR
     private static string[] ReadNames()
     {
         List<string> temp = new List<string>();
         foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
         {
             if (S.enabled)
             {
                 string name = S.path.Substring(S.path.LastIndexOf('/')+1);
                 name = name.Substring(0,name.Length-6);
                 temp.Add(name);
             }
         }
         return temp.ToArray();
     }
     [UnityEditor.MenuItem("CONTEXT/LevelManager/Update Scene Names")]
     private static void UpdateNames(UnityEditor.MenuCommand command)
     {
         LevelManager context = (LevelManager)command.context;
         context.scenes = ReadNames();
     }

    [UnityEditor.MenuItem("CONTEXT/LevelManager/Delete Player Prefs")]
    private static void DeletePlayerPrefs(UnityEditor.MenuCommand command) {
        PlayerPrefs.DeleteAll();
    }
     
     private void Reset()
     {
         scenes = ReadNames();
     }
#endif

    public void Start() {
        string unlockedLevel = PlayerPrefs.GetString("UnlockedLevel", "[NewGame]");
        Debug.Log("UnlockedLevel: " + unlockedLevel, this);
        int unlockedSceneIndex = 1;

        if (unlockedLevel != "[NewGame]") {
            foreach (string scene in scenes) {
                unlockedSceneIndex++;

                if (scene == unlockedLevel)
                    break;
            }
        }

        int sceneIndex = 1;
        foreach (string scene in scenes) {
            Button button = Instantiate(buttonPrefab, gridLayout.transform).GetComponent<Button>();
            Debug.Log("Unlocked " + sceneIndex + " <= " + unlockedSceneIndex + ": " + (sceneIndex <= unlockedSceneIndex), this);
            button.interactable = sceneIndex <= unlockedSceneIndex;
            button.onClick.AddListener(() => SelectLevel(scene));
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = sceneIndex++.ToString();
        }
    }

    public void SelectLevel(string levelName) {
        Debug.Log(levelName, this);
        fader.FadeIn(() => StartCoroutine(AsyncLevelLoad(levelName)));
    }

    private IEnumerator AsyncLevelLoad(string levelName) {
        SceneManager.LoadSceneAsync(levelName);
        yield return null;
    }
}
