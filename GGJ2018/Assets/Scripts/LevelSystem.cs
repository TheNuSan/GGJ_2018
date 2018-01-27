using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour {

    public string NextLevelOverride;

	public void GotToNextLevel() {
        string NextLevel = NextLevelOverride;
        if(NextLevelOverride.Length==0) {
            Scene CurrentScene = SceneManager.GetActiveScene();

            /*if (CurrentScene.IsValid()) {
                Scene NextScene = SceneManager.GetSceneAt(CurrentScene.buildIndex + 1);
                if (NextScene.IsValid()) {
                    NextLevel = NextScene.name;
                }
            }*/

            
            string CurrentName = CurrentScene.name;
            string Ending = CurrentName.Substring(CurrentName.Length - 2);
            int LevelNumber = 0;
            if(int.TryParse(Ending, out LevelNumber)) {
                string LevelStr = (LevelNumber+1).ToString();
                if (LevelStr.Length < 2) LevelStr = "0" + LevelStr;
                NextLevel = CurrentName.Substring(0, CurrentName.Length - 2) + LevelStr;
            }
            
        }

        if (NextLevel.Length > 0) {
            if (SceneUtility.GetBuildIndexByScenePath(NextLevel) >= 0) {
                
                SceneManager.LoadScene(NextLevel);
                
            }
        }
    }

}
