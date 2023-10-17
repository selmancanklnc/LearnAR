using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateCtrl : MonoBehaviour
{
    public Vector3 dir = new Vector3(0,0,1);

    private bool isStop;

    // Start is called before the first frame update
    void Start()
    {
        //ani = this.transform.DORotate(dir, 2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
        //ani.SetAutoKill(false);
        //ani.SetLoops(-1, LoopType.Incremental);
        //ani.PlayForward();
    }

    void Update()
    {
        if (!isStop)
        {
            this.transform.Rotate(dir * Time.deltaTime * 100f, Space.Self);
        }
    }

    public void StopRotate()
    {
        isStop = true;
    }

    private void OnEnable()
    {
        isStop = false;
    }
}
