using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Valve.VR.InteractionSystem.InteractableHoverEvents))]
public class BookTutorial : MonoBehaviour
{
    [SerializeField]
    GameObject machine;
    [SerializeField]
    GameObject secondMachine;

    [SerializeField]
    Material highlightMaterial;

    List<Renderer> machineRends;
    List<Renderer> mRends;

    [SerializeField]
    Image BackGround;

    [SerializeField]
    Color unHighlighted;

    [SerializeField]
    Color Highlighted;

    bool highlighted = false;

    private void Start()
    {
        machineRends = new List<Renderer>();
        mRends = new List<Renderer>();
        if (machine)
            GetRends(machineRends, machine);
        if (secondMachine)
            GetRends(machineRends, secondMachine);
        GetRends(mRends, gameObject);
    }

    public void HighlightMachine(bool Highlight)
    {
        if (machine || secondMachine)
        {
            if (Highlight)
            {
                highlighted = true;
                foreach (Renderer rend in machineRends)
                {
                    List<Material> temp = new List<Material>();
                    temp.AddRange(rend.sharedMaterials);
                    temp.Add(highlightMaterial);
                    rend.sharedMaterials = temp.ToArray();
                }
                foreach (Renderer rend in mRends)
                {
                    List<Material> temp = new List<Material>();
                    temp.AddRange(rend.sharedMaterials);
                    temp.Add(highlightMaterial);
                    rend.sharedMaterials = temp.ToArray();
                }
                if (BackGround)
                    BackGround.color = Highlighted;

            }
            else
            {
                highlighted = false;
                foreach (Renderer rend in machineRends)
                {
                    List<Material> temp = new List<Material>();
                    temp.AddRange(rend.sharedMaterials);
                    temp.RemoveAt(temp.Count - 1);
                    rend.sharedMaterials = temp.ToArray();
                }
                foreach (Renderer rend in mRends)
                {
                    List<Material> temp = new List<Material>();
                    temp.AddRange(rend.sharedMaterials);
                    temp.RemoveAt(temp.Count - 1);
                    rend.sharedMaterials = temp.ToArray();
                }
                if (BackGround)
                    BackGround.color = unHighlighted;
            }
        }
    }

    void GetRends(List<Renderer> rends, GameObject obj)
    {
        if (obj.activeInHierarchy)
        {
            Renderer rend = obj.gameObject.GetComponent<Renderer>();
            if (rend)
                rends.Add(rend);
        }
        foreach (Transform temp in obj.transform.GetComponentInChildren<Transform>())
        {
            GetRends(rends, temp.gameObject);
        }

    }

    private void OnDisable()
    {
        if(highlighted)
            HighlightMachine(false);
    }

    public void WorkOrder()
    {
        if (DialogueManager.dialogueInstance)
            DialogueManager.dialogueInstance.NoticeworkOrders();
    }
}
