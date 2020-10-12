using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 点击按钮切换场景
/// 
/// 
/// </summary>
public class ButtonController : MonoBehaviour {

    public string scene_name = ""; // 目标场景的名字

    //-- 切换场景
    public void LoadindScene() {
        SceneManager.LoadScene(scene_name);
        print(123123);
    }
}
