using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
	成都时代互动科技有限公司 www.epoching.com 
                 
	By Jeremy
*/
public class Touch_drag_rotate_scale_control : MonoBehaviour
{

    //init transform
    private Vector3 init_position;
    private Quaternion init_rotation;
    private Vector3 init_scale;

    //drag variable
    #region 
    [Header("Does it need to drag")]
    public bool is_dragable;

    [Header("long touch drag time")]
    public float long_touch_drag_time;

    private bool is_dragging = false;           //是否正在拖拽
    private int start_time_stamp;               //开始发射射线的时间戳
    private bool is_long_touch_timing = false;  //是否正在计时，拖拽长按
    private float distance_z;                   //发送射线摄像机到碰撞体 Z 轴上的距离
    private Vector3 drag_offset;                //点击拖拽时，鼠标到物体中心的偏差距离
    #endregion

    //rotation variable
    #region 
    [Header("Does it need to rotate")]
    public bool is_rotation;

    //[Header("If it need rotate,choose a rotation type")]
    //public Rotation_type rotation_type;

    [Header("Rotational speed 0~1")]
    [Range(0, 1)]
    public float rotation_speed;
    #endregion

    //scale variable
    #region 
    [Header("Does it need to scale")]
    public bool is_scale;

    [Header("scale speed")][Range(0.001f, 0.02f)]
    public float scale_speed;

    [Header("max scale and min scale")]
    public float max_scale;
    public float min_scale;

    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2) 
    #endregion

    //get the init transform
    void Start()
    {
        this.init_position = this.transform.position;
        this.init_rotation = this.transform.rotation;
        this.init_scale = this.transform.localScale;
    }

    void Update()
    {
        //没有触摸，就没有正在拖拽中，也不计时拖拽
        #region 
        if (Input.touchCount <= 0)
        {
            //没有被拖拽
            this.is_dragging = false;
            this.is_long_touch_timing = false;
            return;
        }
        #endregion

        //单点触摸
        #region 
        else if (Input.touchCount == 1)
        {
            //获取触摸位置
            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;

            //拖拽
            #region 
            if (this.is_dragable == true)
            {
                //发射射线
                #region 
                if (this.is_dragging == false)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(pos);
                    if (Physics.Raycast(ray, out hit) && (hit.transform.name == this.gameObject.name))
                    {
                        //开始发射射线的时间戳
                        if (this.is_long_touch_timing == false)
                        {
                            //获取时间戳
                            this.start_time_stamp = System.Environment.TickCount;

                            //撞到物体就开始计时
                            this.is_long_touch_timing = true;
                        }

                        if (System.Environment.TickCount - this.start_time_stamp >= 1000 * this.long_touch_drag_time)
                        {
                            this.is_dragging = true;
                            this.transform.localScale = this.transform.localScale * 0.9f;

                            //获取一个偏差位置和摄像机到控制物体的Z轴距离
                            this.distance_z = hit.transform.position.z - Camera.main.transform.position.z;
                            this.drag_offset = hit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.distance_z));
                        }
                    }
                }
                #endregion

                //拖拽物体
                #region 
                else if (is_dragging && touch.phase == TouchPhase.Moved)
                {
                    //限制不能拖出屏幕的条件
                    //if (Input.mousePosition.x > Screen.width / 5 && Input.mousePosition.x < Screen.width / 5 * 4 &&
                    // Input.mousePosition.y > Screen.height / 5 && Input.mousePosition.y < Screen.height / 5 * 4)
                    this.transform.position =
                        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.distance_z)) + drag_offset;

                }
                #endregion

                //抬起手指退出拖拽
                #region 
                else if (is_dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    this.is_dragging = false;
                    this.is_long_touch_timing = false;
                    this.transform.localScale = this.transform.localScale * 1.112f;
                }
                #endregion
            }



            #endregion

            //旋转
            #region 
            if (this.is_rotation == true)
            {
                if (touch.phase == TouchPhase.Moved && this.is_dragging == false)
                {
                    Vector2 deltaPos = touch.deltaPosition;

                    if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                    {
                        if (Mathf.Abs(deltaPos.x) > 5)
                        {
                            //取消拖拽
                            this.is_dragging = false;
                            this.is_long_touch_timing = false;

                            transform.Rotate(Vector3.down * deltaPos.x * this.rotation_speed, Space.Self); //todo 下次优化 把0.1提出去
                        }
                    }
                    else
                    {
                        //if (Mathf.Abs(deltaPos.y) > 5)
                        //{
                        //    //取消拖拽
                        //    this.is_dragging = false;
                        //    this.is_long_touch_timing = false;

                        //    transform.Rotate(Vector3.right * deltaPos.y * this.rotation_speed, Space.World); //todo 下次优化 把0.1提出去
                        //}
                    }
                }
            }

           
            #endregion
        }
        #endregion

        //多点，缩放
        #region 
        else
        {
            if (is_scale == true)
            {
                //没有长按拖动，计时器归零
                this.is_dragging = false;
                this.is_long_touch_timing = false;

                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    this.oldTouch2 = newTouch2;
                    this.oldTouch1 = newTouch1;
                    return;
                }

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = Vector2.Distance(newTouch1.position, newTouch2.position) - Vector2.Distance(oldTouch1.position, oldTouch2.position);

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset * this.scale_speed;

                //获取当前大小
                Vector3 localScale = transform.localScale;

                //修改scale
                if ((localScale.x + scaleFactor) < this.max_scale && (localScale.x + scaleFactor) > this.min_scale)
                {
                    transform.localScale = new Vector3(localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);
                }

                //记住最新的触摸点，下次使用  
                this.oldTouch1 = newTouch1;
                this.oldTouch2 = newTouch2;
            }
        }
        #endregion
    }

    //reset the transform 
    public void reset_transform()
    {
        this.transform.position = this.init_position;
        this.transform.rotation = this.init_rotation;
        this.transform.localScale = this.init_scale;
    }

}
