using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class objectPlacement : MonoBehaviour
{

    [SerializeField]
    TextMesh m_Text;
    [SerializeField]
    ObjectPooler placementPool;
    [SerializeField]
    StructureObjectPool structurePool;
    public GameObject prefab;
    public bool Struct;
    GameObject Holder;
    bool textActive = false;
    // Use this for initialization
    private void Start()
    {
        Holder = transform.parent.parent.gameObject;
    }
    public void SetObject(Hand hand = null)
    {
        if (!Struct)
        {
            GameObject finding = GameObject.Find(prefab.name);
            if (finding)
            {
                if (finding.transform.parent && finding.transform.parent.GetComponent<Hand>())
                {
                    finding.transform.parent.GetComponent<Hand>().DetachObject(finding);
                }
                SnapToHand snapper = finding.GetComponent<SnapToHand>();
                if (snapper)
                {
                    snapper.Drop();
                    snapper.SetHand(hand);
                    snapper.SnapOrShoot();
                }
                Throwable throwing = prefab.GetComponent<Throwable>();
                if(throwing)
                {
                    hand.AttachObject(finding, throwing.attachmentFlags | Hand.AttachmentFlags.SnapOnAttach);
                    finding.GetComponent<Throwable>().attachmentFlags = prefab.GetComponent<Throwable>().attachmentFlags;
                }
                finding.name = prefab.name;
            }
        }
        else
        {
            GameObject placing = placementPool.Spawn(structurePool.transform);
            placing.GetComponent<StructurePlacing>().SetObject(structurePool);
            placing.GetComponent<StructurePlacing>().device = hand;
        }

        Holder.SetActive(false);
    }

    public void ActivateText(bool Active, string mText = null)
    {
        textActive = Active;
        m_Text.gameObject.SetActive(Active);
        //if (Active)
        //{
        //    if (mText != null)
        //        m_Text.text = mText;
        //    else
        //    {
        //        if (prefab.GetComponent<StructurePlacing>())
        //            m_Text.text = prefab.GetComponent<StructurePlacing>().RealObject.name;
        //        else
        //            m_Text.text = prefab.name;
        //    }
        //}
    }
}
