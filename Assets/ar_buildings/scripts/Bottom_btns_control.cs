using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.RemoteConfig;
using UnityEngine.SceneManagement;
using System;
using bear.j.easy_dialog;
//1.可以开关收起来--
//2.可以刷新广告锁
//3.监听上锁按钮
//4.监听切换模型按钮

public class Bottom_btns_control : MonoBehaviour
{
    [Header("建筑物按钮")]
    public Button[] choose_btns;

    [Header("解锁按钮")]
    public Button[] lock_btns;

    [Header("显示底部面板的按钮")]
    public GameObject game_obj_btn_show;

    [Header("自己的rect transform")]
    public RectTransform rect_transform_self;

    [Header("Main_contrl")]
    public Main_control main_control;

    //是否处于真正显示状态
    public bool is_showing=false;

    //是否处于正在隐藏状态
    public bool is_hiding = false;

    private float panel_height;

    // Start is called before the first frame update
    void OnEnable()
    {
        this.panel_height = this.rect_transform_self.sizeDelta.y;
        //初始化显示按钮,选择面板归位
        //this.game_obj_btn_show.SetActive(true);
        this.rect_transform_self.position = new Vector3(this.rect_transform_self.position.x, -this.panel_height, this.rect_transform_self.position.z);

        ////设置按钮透明处不能被点击
        //for (int i = 0; i < this.choose_btns.Length; i++)
        //{
        //    this.choose_btns[i].GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        //}

        //先隐藏面板
        this.reset_ready_show();
    }

    //update 间隔固定时间执行	    
    private float interval_time = 0.025f;//间隔时间,单位秒
    private float time_stamp = 0;    //时间戳
    void Update()
    {
        //向上运动出现
        #region 
        if (this.is_showing)
        {
            this.time_stamp += Time.deltaTime;

            if (this.time_stamp > this.interval_time)
            {
                this.time_stamp = 0;

                //执行想要执行的东西
                float x = this.rect_transform_self.position.x;
                float y = this.rect_transform_self.position.y;
                float z = this.rect_transform_self.position.z;

                if (y >= 0)
                {
                    this.is_showing = false;
                    this.rect_transform_self.position = new Vector3(x, 0, z);
                    this.game_obj_btn_show.SetActive(false);
                    return;
                }
                else
                {
                    this.rect_transform_self.position = new Vector3(x, y + 20, z);
                }
            }
        }
        #endregion

        //向下运动隐藏
        #region 
        else if (this.is_hiding)
        {
            this.time_stamp += Time.deltaTime;

            if (this.time_stamp > this.interval_time)
            {
                this.time_stamp = 0;

                //执行想要执行的东西
                float x = this.rect_transform_self.position.x;
                float y = this.rect_transform_self.position.y;
                float z = this.rect_transform_self.position.z;

                if (y <= -this.panel_height)
                {
                    this.is_hiding = false;
                    this.rect_transform_self.position = new Vector3(x, -this.panel_height, z);

                    //显示 或者隐藏 显示面板的按钮
                    if (Config.ar_statu == AR_statu.recognizing)
                    {
                        this.game_obj_btn_show.SetActive(false);
                    }
                    else
                    {
                        this.game_obj_btn_show.SetActive(true);
                    }
                    return;
                }
                else
                {
                    this.rect_transform_self.position = new Vector3(x, y - 20, z);
                }
            }
        }
        #endregion
    }

    //监听选择按钮的事件
    public void on_btn_choose(int num)
    {
        if (this.is_hiding || this.is_showing)
            return;

        //播放按钮声音
        Audio_control.instance.play_btn_sound();

        //修改练习歌曲索引
        Config.building_index = num;

        //隐藏面板
        this.time_stamp = 0;
        this.is_hiding = true;

        //切换建筑物
        this.main_control.switch_buidling();
    }

    //重置到准备显示状态
    public void reset_ready_show()
    {
        this.is_hiding = true;
    }

    //出现按钮,点了之后显示选择面板
    public void on_show_btn()
    {
        if (this.is_hiding || this.is_showing)
            return;
        if (Config.ar_statu != AR_statu.object_is_placed)
            return;

        //播放按钮声音
        Audio_control.instance.play_btn_sound();

        //隐藏显示面板按钮
        this.game_obj_btn_show.SetActive(false);

        //显示面板
        this.time_stamp = 0;
        this.is_showing = true;


        //if (this.rect_transform_self.position.y >= 0)
        //{
        //    this.time_stamp = 0;
        //    this.is_hiding = true;
        //}

        //if (this.rect_transform_self.position.y <= -this.panel_height)
        //{
        //    this.time_stamp = 0;
        //    this.is_showing = true;
        //}
    }


}
