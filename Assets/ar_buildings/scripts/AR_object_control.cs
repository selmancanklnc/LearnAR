using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;

public class AR_object_control : MonoBehaviour
{

    public TMP_Text question;
    public Text correctAnswer;
    public Text wrongAnswer;

    public Button correctButton;
    public Button wrongButton;

    public Sprite defaultSprite;

    public Animator defAnimator;
    public Avatar avatarMath;
    public Avatar avatarEng;
    public Avatar avatarTurkish;
    public Avatar avatarSocial;

    public GameObject mathmodel, engmodel, socialmodel, turkishmodel;

    public GameObject effectTrue;
    public GameObject effectFalse;


    public GameObject scorePanel;
    public GameObject star2;
    public GameObject star3;
    public TMP_Text lessonName;

    //SORU RESMİ
    public Material questionImageMaterial;
    public GameObject questionPanel;
    public Renderer questionPRenderer;





    DatabaseReference reference;
    List<QuestionModel> questions = new List<QuestionModel>();


    public int waitTime = 0;
    public int questionCount = 0;
    public int mathQuestionCount = 0;
    public int correctCount = 0;



    [Header("烟花身上的刚体")]
    public Rigidbody rigid_body;

    [Header("出场的粒子系统")]
    public ParticleSystem particle_system;

    [Header("建筑物发父物体")]
    public GameObject game_objs_building_parent;



    void Start()
    {

        correctButton.onClick.AddListener(IncrementCorrect);


        int i = Config.building_index;
        switch (i)
        {
            case 0:
                defAnimator.avatar = avatarMath;
                mathmodel.SetActive(true);
                lessonName.text = "Matematik";
                break;
            case 1:
                defAnimator.avatar = avatarTurkish;
                turkishmodel.SetActive(true);
                lessonName.text = "Türkçe";
                break;
            case 2:
                defAnimator.avatar = avatarSocial;
                socialmodel.SetActive(true);
                lessonName.text = "Hayat Bilgisi";
                break;
            case 3:
                defAnimator.avatar = avatarEng;
                engmodel.SetActive(true);
                lessonName.text = "İngilizce";
                break;

            default:
                break;
        }
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        //if (!string.IsNullOrWhiteSpace(dbTableName))
        //{
        //    var rand = new System.Random();
        //    //var skipCountMax = questionCount - 9;

        //    //var skipCount = skipCountMax < 0 ? 0 : rand.Next(0, skipCountMax);

        //    reference.Child(dbTableName).EqualTo("", "").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(d =>
        //    {

        //        DataSnapshot snapshot = d.Result;

        //        if (snapshot?.Value != null)
        //        {
        //            foreach (var item in snapshot.Children)
        //            {
        //                var json = JsonConvert.SerializeObject(item.Value);
        //                //json = JsonConvert.DeserializeObject<string>(json);
        //                var model = JsonConvert.DeserializeObject<QuestionModel>(json);
        //                questions.Add(model);

        //            }
        //            //questions.ShuffleMe();
        //        }
        //        newQuestion();

        //    });
        //}

        //else
        //{
        //    newQuestion();
        //}
        reference.Child("questions").OrderByChild("TopicId").EqualTo(Config.topic_index).GetValueAsync().ContinueWithOnMainThread(d =>
        {

            DataSnapshot snapshot = d.Result;

            if (snapshot?.Value != null)
            {
                foreach (var item in snapshot.Children)
                {
                    var json = JsonConvert.SerializeObject(item.Value);
                    //json = JsonConvert.DeserializeObject<string>(json);
                    var model = JsonConvert.DeserializeObject<QuestionModel>(json);
                    questions.Add(model);

                }
                //questions.ShuffleMe();
            }
            newQuestion();

        });
        //correctButton.onClick.AddListener(() => TaskOnClick(2));
        //wrongButton.onClick.AddListener(() => TaskOnClick(3));




    }


    public void HomeScene()
    {
        SceneManager.LoadSceneAsync("main_ui");
    }

    public void RetryButton()
    {
        SceneManager.LoadSceneAsync("ar_buildings");
    }


    void IncrementCorrect()
    {

        correctCount++;
    }
    IEnumerator WaitForFunction()
    {
        yield return new WaitForSeconds(waitTime);
        waitTime = 2;
        if (effectTrue.activeSelf)
        {
            effectTrue.SetActive(false);

        }
        if (effectFalse.activeSelf)
        {
            effectFalse.SetActive(false);

        }
        wrongButton.image.overrideSprite = defaultSprite;
        correctButton.image.overrideSprite = defaultSprite;
        int i = Config.building_index;
        switch (i)
        {
            case 0:
                if (true)
                {
                    newOtherQuestion();
                }
                else
                {
                    newMathQuestion();

                }
                break;
            case 1:
            case 2:
            case 3:
                newOtherQuestion();
                break;

            default:
                break;
        }

    }
    IEnumerator WaitForFunction2()
    {
        yield return new WaitForSeconds(2);
        scorePanel.SetActive(true);
        if (correctCount > 3 & correctCount < 7)
        {
            star2.SetActive(true);
        }
        if (correctCount > 7)
        {
            star2.SetActive(true);
            star3.SetActive(true);
        }
    }


    public void newOtherQuestion()
    {
        try
        {
            if (!questions.Any())
            {
                StartCoroutine(WaitForFunction2());
            }

            var newQuestion = questions?.First();
            if (newQuestion == null)
            {
                StartCoroutine(WaitForFunction2());
            }
            var rand = new System.Random();

            var number3 = rand.Next(1, 10);
            var position = wrongButton.transform.position;
            var position2 = correctButton.transform.position;
            if (number3 % 2 == 0)
            {
                wrongButton.transform.position = position;
                correctButton.transform.position = position2;
            }
            else
            {
                wrongButton.transform.position = position2;
                correctButton.transform.position = position;
            }
            question.text = newQuestion.Question;
            correctAnswer.text = newQuestion.CorrectAnswer;
            wrongAnswer.text = newQuestion.WrongAnswer;
            if (newQuestion.Image != null)
            {
                if (questionImageMaterial.HasTexture("_MainTex"))
                {
                    questionImageMaterial.SetTexture("_MainTex", null);
                }
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
                tex.LoadImage(newQuestion.Image);
                //MemoryStream inputMemoryStream = new MemoryStream(newQuestion.Image);

                //System.Drawing.Image image = System.Drawing.Image.FromStream(inputMemoryStream);
                questionImageMaterial.SetTexture("_MainTex", tex);

                //var image = byteArrayToImage(newQuestion.Image);
                float imageWidth = 1;
                float imageHeight = 1;
                //if (image.Height > image.Width)
                //{
                //    imageHeight = 1;
                //    imageWidth = 0.5f;
                //}
                //else
                //{
                //    imageHeight = 1;
                //    imageWidth = 1;
                //}

                // GameObject'in boyutunu sığdır
                Vector3 newScale = new Vector3(imageWidth, imageHeight, 1f);
                questionPanel.transform.localScale = newScale;

                //float width = tex.width;
                //float height = tex.height; 
                //Vector2  newScale = new Vector2(width, height);
                //questionPanel.transform.localScale = newScale;
                 
                //questionPanel.transform.localScale = new Vector2(1,2); 
                if (!questionPanel.activeSelf)
                {
                    questionPanel.SetActive(true);
                }

            }
            else
            {
                if (questionPanel.activeSelf)
                {
                    questionPanel.SetActive(false);
                }

            }
            questions.Remove(newQuestion);
        }
        catch (Exception e)
        {
            Debug.LogError(e.GetBaseException().StackTrace);
            Debug.LogError(e.GetBaseException().Message);
        }


    }
    public void newMathQuestion()
    {
        if (mathQuestionCount == 10)
        {
            StartCoroutine(WaitForFunction2());
        }
        var rand = new System.Random();

        var process = rand.Next(0, 4);
        var answer = 0;
        var number1 = 0;
        var number2 = 0;
        var number3 = 0;
        switch (process)
        {
            case 0:
                number1 = rand.Next(1, 100);
                number2 = rand.Next(1, 100);
                number3 = rand.Next(1, 200);
                answer = number1 + number2;
                question.text = $"{number1} + {number2} = ?";
                break;
            case 1:
                number1 = rand.Next(1, 10);
                number2 = rand.Next(1, 10);
                number3 = rand.Next(1, 100);
                answer = number1 * number2;
                question.text = $"{number1} * {number2} = ?";
                break;
            case 2:
                number1 = rand.Next(1, 100);
                number2 = rand.Next(1, 100);
                number3 = rand.Next(1, 100);
                answer = Math.Max(number1, number2) - Math.Min(number1, number2);
                question.text = $"{Math.Max(number1, number2)} - {Math.Min(number1, number2)} = ?";
                break;
            case 3:
                number1 = rand.Next(1, 10);
                number2 = rand.Next(1, 10);
                number3 = rand.Next(1, 10);
                answer = (number1 * number2) / number1;
                question.text = $"{(number1 * number2)} / {number1} = ?";
                break;

        }
        if (answer == number3)
        {
            newMathQuestion();
        }
        var position = wrongButton.transform.position;
        var position2 = correctButton.transform.position;
        if (number3 % 2 == 0)
        {
            wrongButton.transform.position = position;
            correctButton.transform.position = position2;
        }
        else
        {
            wrongButton.transform.position = position2;
            correctButton.transform.position = position;
        }
        correctAnswer.text = answer.ToString();
        wrongAnswer.text = number3.ToString();

        mathQuestionCount++;


    }
    public void newQuestion()
    {
        StartCoroutine(WaitForFunction());

    }
    void OnEnable()
    {
        //把子物体弄高点，使其有一种从上到下掉下来的感觉
        this.game_objs_building_parent.transform.localPosition = new Vector3(0, 1f, 0);

        ////隐藏所有的物体
        //for (int i = 0; i < game_objs_building.Length; i++)
        //{
        //    this.game_objs_building[i].SetActive(false);
        //}

        //显示目标物体
        //this.game_objs_building[0].SetActive(true);
        //延时播放特效和声音
        Invoke("play_particle_and_audio", 0.2f);
    }

    //播放出场特效和音效
    private void play_particle_and_audio()
    {
        this.particle_system.Play();

        Audio_control.instance.play_show_sound();
    }

    //放烟花时的抖动效果
    public void shake()
    {
        this.rigid_body.AddForce(0f, 40f, 0f);
    }
    public System.Drawing.Image byteArrayToImage(byte[] bytesArr)
    {
        using (MemoryStream memstr = new MemoryStream(bytesArr))
        {
            System.Drawing.Image img = System.Drawing.Image.FromStream(memstr);
            memstr.Close();
            return img;
        }
    }
}
public class QuestionModel
{
    public string CorrectAnswer { get; set; }

    public string WrongAnswer { get; set; }
    public string Question { get; set; }
    public byte[] Image { get; set; }
    public int TopicId { get; set; }
}

public class TopicModel
{
    public string Name { get; set; }

    public int Id { get; set; }
    public int Lesson { get; set; }
    public int Class { get; set; }
}
public static class Helper
{
    public static void ShuffleMe<T>(this IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = list.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);

            T value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }

}