using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using bear.j.easy_dialog;
using epoching.loading_circle;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Firebase.Database;
using Newtonsoft.Json;
using Firebase.Extensions;
using TMPro;
using GoogleMobileAds.Api;

public class Main_ui_control : MonoBehaviour
{
    public InterstitialAd interstitialAd;

    //public Button[] buildings_btns;
    public GameObject netControlPanel;
    public GameObject scrolviewcontent;
    public UnityEngine.UI.Button tamplate;
    //Konu Sayısı
    //public int btnCount = 10;
    DatabaseReference reference;
    public struct userAttributes { }
    public struct appAttributes { }
    public GameObject warningPanel;

    void Awake()
    {

        new Config();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

    }


    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //Konu Sayısını Arttır




    }
    public void LoadInterstitialAd(string adModGoogleId)
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(adModGoogleId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                if (interstitialAd != null && interstitialAd.CanShowAd())
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd.Show();
                }
                else
                {
                    Debug.LogError("Interstitial ad is not ready yet.");
                }
            });
    }
    public void AdShow()
    {
        reference.Child("settings").GetValueAsync().ContinueWithOnMainThread(d =>
        {

            DataSnapshot snapshot = d.Result;
            if (snapshot?.Value != null)
            {
                var json = JsonConvert.SerializeObject(snapshot.Value);
                //json = JsonConvert.DeserializeObject<string>(json);

                var model = JsonConvert.DeserializeObject<Settings>(json);

                var adModGoogleId = model.adModGoogleId;
                if (!string.IsNullOrWhiteSpace(adModGoogleId))
                {
                    MobileAds.Initialize((InitializationStatus initStatus) =>
                    {

                        LoadInterstitialAd(adModGoogleId);

                    });
                }

            }
        });
        //skor


    }
    private void Update()
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


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            warningPanel.SetActive(true);
        }
    }

    public void OnYesButtonPressed()
    {
        Application.Quit();
    }

    public void OnNoButtonPressed()
    {
        warningPanel.SetActive(false);
    }


    //SINIF SEÇİMİ
    public void on_class_select(int sınıfNum)
    {
        Audio_control.instance.play_btn_sound();
        Config.class_index = sınıfNum;

    }


    //DERS SEÇİMİ
    public void on_lesson_choose(int lessonNum)
    {
        Loading_circle.waiting();
        Audio_control.instance.play_btn_sound();
        Config.building_index = lessonNum;
        var children = scrolviewcontent.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (var child in children)
        {
            if (child != tamplate)
            {
                UnityEngine.Object.DestroyImmediate(child.gameObject);

            }
        }
        reference.Child("topics")
             .OrderByChild("Lesson")
             .EqualTo(Config.building_index)
            //.OrderByChild("Class")
            //.EqualTo(Config.class_index)
            .GetValueAsync().ContinueWithOnMainThread(d =>
        {

            DataSnapshot snapshot = d.Result;

            if (snapshot?.Value != null)
            {
                var topics = new List<TopicModel>();
                foreach (var item in snapshot.Children)
                {

                    var json = JsonConvert.SerializeObject(item.Value);
                    //json = JsonConvert.DeserializeObject<string>(json);
                    var model = JsonConvert.DeserializeObject<TopicModel>(json);
                    if (model.Class == Config.class_index)
                    {
                        topics.Add(model);
                    }


                }
                topics=topics.OrderBy(a=>a.Id).ToList();
                foreach (var model in topics)
                {
                    UnityEngine.UI.Button btn = (UnityEngine.UI.Button)Instantiate(tamplate);
                    btn.GetComponentInChildren<TMP_Text>().text = model.Name;
                    btn.onClick.AddListener(() =>
                    {
                        Config.topic_index = model.Id;
                        on_Subject_choose();
                    });
                    btn.transform.SetParent(scrolviewcontent.transform);
                }
                Loading_circle.wait_over();

                //questions.ShuffleMe();
            }
            else
            {
                Loading_circle.wait_over();

            }

        });

    }


    //KONU SEÇİMİ
    public void on_Subject_choose()
    {

        Loading_circle.waiting();
        //Konu Seçildi Soru Ekranına Geç
        StartCoroutine(this.load_scene("ar_buildings"));

    }




    //SORU EKRANINA GEÇERKEN BEKLET
    public IEnumerator load_scene(string scene)
    {
        //开始等待
        Loading_circle.waiting();

        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        while (!op.isDone)
        {
            //print(op.progress * 100);
            yield return new WaitForEndOfFrame();
        }

        //结束等待
        Loading_circle.wait_over();
    }





}
public class Settings
{
    public string adModGoogleId;
    public string adMobBannerGoogleId;
}