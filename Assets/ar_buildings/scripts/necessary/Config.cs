using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
   
    //AR识别时的状态
    public static AR_statu ar_statu = AR_statu.recognizing;

    //烟花索引
    public static int building_index = 4;
    public static int class_index = 4;
    public static int topic_index =0;
 


}

//AR识别时的状态
public enum AR_statu
{
    recognizing,                           //识别平面中
    object_is_placed                       //物体已被放置
}