using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Controller : MonoBehaviour {
    public float max_cam;
    public float min_cam;
    public float buffer = 10f;


    public int vst = 0, hst = 0;
    public bool isHorizontal;

    public Vector3 st_width, st_height;

    Vector3 vst_offset, hst_offset;

    // Use this for initialization
    void Start () {
        st_width = new Vector3(52,0,0);
        st_height = new Vector3(0,52,0);
        vst_offset = new Vector3(-35,42,0);
        hst_offset = new Vector3(0,0,0);
        isHorizontal = true;
        // h_offset.transform.position = h_offset.transform.position + new Vector3(st_width, st_height);
    }
    
    // Update is called once per frame
    void Update () {
    }

    public void help(int st){
        Debug.Log("help me");
    }

    public IEnumerator GotoVerticalStreet(int st){
        Debug.Log("voff");
        vst = st;
        isHorizontal = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("VerticalScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone) yield return null;
        GameObject v = GameObject.Find("/VerticalOffset");
        v.transform.position = vst_offset + st_width * vst;
        SceneManager.UnloadSceneAsync("SampleScene");
    }

    public IEnumerator GotoHorizontalStreet(int st){
        Debug.Log("hoff");
        hst = st;
        isHorizontal = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone) yield return null;
        GameObject v = GameObject.Find("/HorizontalOffset");
        v.transform.position = hst_offset + st_height * hst;
        SceneManager.UnloadSceneAsync("VerticalScene");
    }

    public float getCameraMinX(){
        if(isHorizontal){
            return -14f;
        }
        return -18f + 52f * vst;
    }
    
    public float getCameraMaxX(){
        if(isHorizontal){
            return 188f;
        }
        return -12f + 52f * vst;
    }
}
