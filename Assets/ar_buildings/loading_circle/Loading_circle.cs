using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace epoching.loading_circle
{
    public class Loading_circle : MonoBehaviour
    {
        //全局显示
        public static void waiting()
        {
            //GameObject go = Resources.Load<GameObject>("Canvas_loading_circle"); 
            //Instantiate(go);
            GameObject go = Resources.Load<GameObject>("Canvas_loading_circle");
            Instantiate(go);
        }

        //全局隐藏
        public static void wait_over()
        {
            GameObject[] goes = GameObject.FindGameObjectsWithTag("loading_circle");

            for (int i = 0; i < goes.Length; i++)
            {
                goes[i].GetComponent<Loading_circle>().disappear();
            }
        }

        private void OnEnable()
        {
            ////set the canvas width and height
            this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);

            //16秒后 自动消失
            //Invoke("disappear", 16f);
        }

        public void disappear()
        {
            StartCoroutine(Canvas_group_fade.hide(this.gameObject, true));
        }

        private void OnDisable()
        {
            StopCoroutine("disappear");
        }
    }
}