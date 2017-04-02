using UnityEngine;
using System.Collections;

public class PhysicalSlider : MonoBehaviour
{
    enum SliderDirection
    {
        xAxis,
        yAxis,
        zAxis
    }

    [Tooltip("Set to zero when the slider is at StartPoint. Set to one when the slider is at EndPoint.")]
    public float CurrentValue = 0;

    [Tooltip("A transform at the position of the zero point of the slider")]
    public Transform StartPoint;

    [Tooltip("A transform at the position of the one point of the slider")]
    public Transform EndPoint;

    protected Vector3 SliderPath;

    RigidbodyConstraints constraints;

    private Transform tracked_object;

    float distance = 0.0f;

    [SerializeField]
    TextMesh textMesh;

    SliderDirection m_Dir = 0;


    virtual protected void Awake()
    {
        constraints = GetComponent<Rigidbody>().constraints;
        if (StartPoint == null)
        {
            Debug.LogError("This slider has no StartPoint.");
        }
        if (EndPoint == null)
        {
            Debug.LogError("This slider has no EndPoint.");
        }

        transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, CurrentValue);
        SliderPath = EndPoint.position - StartPoint.position;
        distance = Vector3.Distance(StartPoint.position, EndPoint.position);
        tracked_object = null;
        SetText(CurrentValue.ToString());
        if ((constraints & RigidbodyConstraints.FreezePositionX) != RigidbodyConstraints.FreezePositionX)
        {
            m_Dir = SliderDirection.xAxis;
        }
        else if ((constraints & RigidbodyConstraints.FreezePositionY) != RigidbodyConstraints.FreezePositionY)
        {
            m_Dir = SliderDirection.yAxis;
        }
        else if ((constraints & RigidbodyConstraints.FreezePositionZ) != RigidbodyConstraints.FreezePositionZ)
        {
            m_Dir = SliderDirection.zAxis;
        }
    }

    virtual protected void FixedUpdate()
    {
        if(tracked_object)
        {
            switch(m_Dir)
            {
                case SliderDirection.xAxis:
                    {
                        MoveX();
                        break;
                    }
                case SliderDirection.yAxis:
                    {
                        MoveY();
                        break;
                    }
                case SliderDirection.zAxis:
                    {
                        MoveZ();
                        break;
                    }
                default:
                    break;
            }
        }
        LocktoTrack();
        SetCurrValue((Vector3.Distance(StartPoint.position, transform.position) / distance));
        SetText(((int)(CurrentValue * 100)).ToString());
    }

    public void SetTrackedObject(Transform obj)
    {
        tracked_object = obj;
    }

    public void ReleaseTrackedObject()
    {
        if (tracked_object)
        {
            if (tracked_object.gameObject.GetComponent<UIInteractionController>())
                tracked_object.gameObject.GetComponent<UIInteractionController>().ReleaseObject();
            tracked_object = null;
        }
    }

    public bool Sliding()
    {
        return tracked_object;
    }

    public float GetCurrValue()
    {
        return CurrentValue;
    }

    public void SetCurrValue(float cValue)
    {
        CurrentValue = cValue;
    }

    public void SetText(string newText)
    {
        if (textMesh)
            textMesh.text = newText;
    }

    void MoveX()
    {
        if (tracked_object)
        {
            Vector3 new_pos = transform.position;
            new_pos.x = tracked_object.position.x;
            transform.position = new_pos; /*Vector3.Lerp(transform.position, new_pos, Time.deltaTime);*/
        }
    }

    void MoveY()
    {
            Vector3 new_pos = transform.position;
            new_pos.y = tracked_object.position.y;
            transform.position = new_pos;/*Vector3.Lerp(transform.position, new_pos, Time.deltaTime);*/
    }

    void MoveZ()
    {
            Vector3 new_pos = transform.position;
            new_pos.z = tracked_object.position.z;
            transform.position = new_pos; /*Vector3.Lerp(transform.position, new_pos, Time.deltaTime);*/
    }

    void LocktoTrack()
    {
        switch (m_Dir)
        {
            case SliderDirection.xAxis:
                {
                    if (transform.position.x > EndPoint.position.x)
                        transform.position = EndPoint.position;
                    if (transform.position.x < StartPoint.position.x)
                        transform.position = StartPoint.position;
                    break;
                }
            case SliderDirection.yAxis:
                {
                    if (transform.position.y > EndPoint.position.y)
                        transform.position = EndPoint.position;
                    if (transform.position.y < StartPoint.position.y)
                        transform.position = StartPoint.position;
                    break;
                }

            case SliderDirection.zAxis:
                {
                    if (transform.position.z > EndPoint.position.z)
                        transform.position = EndPoint.position;
                    if (transform.position.z < StartPoint.position.z)
                        transform.position = StartPoint.position;
                    break;
                }
            default:
                break;
        }
    }
}

