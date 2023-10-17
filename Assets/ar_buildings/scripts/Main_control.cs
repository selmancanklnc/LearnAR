using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR;
using System;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Newtonsoft.Json;
using TMPro;

public class Main_control : MonoBehaviour
{

    public int configTest;
    public int classTest;
    public GameObject model;
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    public GameObject gameobject_place_btn;
    public GameObject gameobject_reset_btn;
    public GameObject gameobject_hint_scanning;
    public Text Text_debug;
    public Bottom_btns_control bottom_btns_control;
    public Introduction_control introduction_control;

    public GameObject startScreen;
  









    //现实世界中的平面的位置
    private Pose placementPose;

    //是否识别到平面
    private bool placementPoseIsValid = false;

    //AR 数据源
    private ARSessionOrigin arOrigin;

    //AR foundation 释放射线的对象
    private ARRaycastManager raycastManager;
    DatabaseReference reference;

    public GameObject netControlPanel;

    bool debug = false;
    void Start()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            netControlPanel.SetActive(true);
            AudioListener.volume = 0;
        }
        else
        {
            netControlPanel.SetActive(false);
            AudioListener.volume = 1;
        }

        
        // PlayerPrefs'te kayıtlı "FirstTimeOpened" anahtarını kontrol et
        if (PlayerPrefs.GetInt("FirstTimeOpened", 0) == 0)
        {
            // İlk kez açılıyorsa başlangıç ekranını göster
            startScreen.SetActive(true);
            


            // PlayerPrefs'te "FirstTimeOpened" anahtarını 1 olarak ayarla
            PlayerPrefs.SetInt("FirstTimeOpened", 1);
        }
        else
        {
            // Daha önce açıldıysa başlangıç ekranını devre dışı bırak
            startScreen.SetActive(false);
           
        }
       


        //SEÇİLEN DERS   =>   0> MATEMATİK , 1> TÜRKÇE , 2> HAYAT BİLGİSİ , 3> İNGİLİZCE
        configTest = Config.building_index;


        //SEÇİLEN SINIF  =>   0> 1.SINIF , 1> 2.SINIF , 2> 3.SINIF , 3> 4.SINIF
        classTest = Config.class_index;


        reference = FirebaseDatabase.DefaultInstance.RootReference;


        this.arOrigin = FindObjectOfType<ARSessionOrigin>();


        this.raycastManager = FindObjectOfType<ARRaycastManager>();


        this.change_to_recognizing();


        if (debug)
        {
            gameobject_hint_scanning.SetActive(false);
            gameobject_place_btn.SetActive(true);
            Config.building_index = 0;
            Config.class_index = 0;
            Config.topic_index = 1;
        }
         


    }



    void Update()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            netControlPanel.SetActive(true);
            AudioListener.volume = 0;
        }
        else
        {
            netControlPanel.SetActive(false);
            AudioListener.volume = 1;
        }

        if (Config.ar_statu == AR_statu.recognizing && !debug)
        {

            this.UpdatePlacementPose();


            this.UpdatePlacementIndicator();
        }


    }

    //切换到识别平面状态
    public void change_to_recognizing()
    {
        Config.ar_statu = AR_statu.recognizing;

        //删除之前出现的物体
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ar_object");
        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }

        //隐藏放置按钮
        this.gameobject_place_btn.SetActive(false);

        //隐藏重置按钮
        this.gameobject_reset_btn.SetActive(false);

        //侧面文本面板准备显示
        this.introduction_control.reset_ready_show();
    }

    //切换到已经放置物体状态
    public void change_to_object_is_placed()
    {
        Config.ar_statu = AR_statu.object_is_placed;

        //放置物体
        this.PlaceObject();

        //显示重置按钮
        this.gameobject_reset_btn.SetActive(true);

        //显示放置按钮
        this.gameobject_place_btn.SetActive(false);

        //隐藏识别平面的提示框
        this.placementIndicator.SetActive(false);
    }

    //更新现实世界是否识别到平面，以及现实平面的位置
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        var hits = new List<ARRaycastHit>();

        //发射射线
        this.raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        this.placementPoseIsValid = hits.Count > 0;

        //DebugControl.Instance.AddOneDebug("发射射线返回的结果"+ hits.Count, ColorType.error, 48);
        //Debug.Log("发射射线返回的结果" + hits.Count);

        if (this.placementPoseIsValid)
        {
            this.placementPose = hits[0].pose;

            //平面到手机摄像头的距离
            float distance = (this.placementPose.position - Camera.main.transform.position).sqrMagnitude;

            //平面太远，太近 都不做处理
            if (distance < 0.15f || distance > 5.5f)
                this.placementPoseIsValid = false;
            else
            {
                var cameraForward = Camera.main.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

                this.placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }

            this.Text_debug.text = "识别到的平面到手机的距离: " + distance;
        }
    }

    //更新平面指示器是否显示和显示的位置
    private void UpdatePlacementIndicator()
    {
        if (this.placementPoseIsValid)
        {
            this.placementIndicator.SetActive(true);
            this.placementIndicator.transform.SetPositionAndRotation(this.placementPose.position, this.placementPose.rotation);

            //显示放置按钮，隐藏提示文字
            StartCoroutine(Canvas_grounp_fade.hide(this.gameobject_hint_scanning));

            this.gameobject_place_btn.SetActive(true);
        }
        else
        {
            this.placementIndicator.SetActive(false);

            //显示提示文字，隐藏放置按钮
            if (debug)
            {
                model.SetActive(true);
                gameobject_place_btn.SetActive(false);
            }

            StartCoroutine(Canvas_grounp_fade.show(this.gameobject_hint_scanning));

            this.gameobject_place_btn.SetActive(false);
        }
    }

    //放置物体
    private void PlaceObject()
    {
        if (debug)
        {
            model.SetActive(true);
            gameobject_place_btn.SetActive(false);

        }


        Instantiate(this.objectToPlace, this.placementPose.position, this.placementPose.rotation);

        //显示显示底部面板的按钮和侧面文本内容的按钮
        this.bottom_btns_control.reset_ready_show();
        this.introduction_control.reset_ready_show();
    }

    //切换模型
    public void switch_buidling()
    {
        //删除之前出现的物体
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ar_object");
        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }

        //重新生成物体
        this.PlaceObject();
    }

    //事件
    #region 
    //监听重置按钮
    public void on_reset_btn()
    {
        Audio_control.instance.play_btn_sound();

        //切换到识别状态
        this.change_to_recognizing();

        this.bottom_btns_control.is_hiding = true;
    }

    //监听返回按钮
    public void on_back_btn()
    {
        Audio_control.instance.play_btn_sound();

        SceneManager.LoadSceneAsync("main_ui");
    }

    //监听放置按钮
    public void on_place_btn()
    {
        //放置物体
        this.change_to_object_is_placed();
    }

    //隐藏下面和侧面面板的按钮
    public void on_hide_panels_btn()
    {
        if (Config.ar_statu == AR_statu.object_is_placed)
        {
            this.bottom_btns_control.reset_ready_show();
            this.introduction_control.reset_ready_show();
        }
    }
    #endregion
}
