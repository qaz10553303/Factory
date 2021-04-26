using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Vector3 targetPos;
    public float moveSpeed;
    public bool hasTarget;

    private float moveTimer;
    private Vector3 savedPos;

    public enum BoxType
    {
        RESOURCE,
        ITEM,
    }
    public BoxType type;

    public class ItemInfo
    {
        public int itemId;
        public int itemAmount;
    }

    public ItemInfo carriedItem=new ItemInfo();

    public List<List<Box>> listsContainThis = new List<List<Box>>();

    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (carriedItem.itemId == 0 || carriedItem.itemAmount == 0)
        {
            DestroyBox();
        }
    }



    public void MoveToTarget(Vector3 targetPos)
    {
        if (!hasTarget)
        {
            savedPos = transform.position;
            this.targetPos = new Vector3(targetPos.x,transform.position.y,targetPos.z);
            moveTimer = 0;
            moveSpeed = 1;
            hasTarget = true;
            StartCoroutine("_MoveToTarget");
        }
    }

    IEnumerator _MoveToTarget()
    {
        float dist = Vector3.Distance(targetPos, savedPos);
        while (moveTimer<1)
        {
            moveTimer += Time.deltaTime/(dist/moveSpeed);
            Vector3 newPos = Vector3.Lerp(savedPos, targetPos, moveTimer);
            transform.position = newPos;
            yield return 0;
        }
        transform.position = targetPos;
        hasTarget = false;
    }

    public void DestroyBox()
    {
        foreach (List<Box> list in listsContainThis)
        {
            if (list.Contains(this))
            {
                list.Remove(this);
            }
        }
        Destroy(this.gameObject);
    }

}
