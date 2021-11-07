using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DisplayOne : MonoBehaviour
{
    [Range(0, 16)]
    public int idx = 0;
    int idxMem;


    [Range(0, 5)]
    public int autoRotate = 0;
    float autoRotateMem = 0;




    private void OnDrawGizmos()
    {
        if (idx != idxMem) UpdateI();
    }

    private void Update()
    {
        if (idx != idxMem) UpdateI();

        if (autoRotate != autoRotateMem)
        {
            if (autoRotateMem == 0) StartCoroutine("AutoRotate");
            autoRotateMem = autoRotate;
        }
    }


    void UpdateI()
    {
        transform.GetChild(Mathf.Min(idxMem, transform.childCount - 1)).gameObject.SetActive(false);
        transform.GetChild(Mathf.Min(idx,    transform.childCount - 1)).gameObject.SetActive(true);
        idxMem = idx;
    }





    IEnumerator AutoRotate()
    {
        yield return new WaitForSeconds(autoRotate - 0.1f);

        while (autoRotate > 0)
        {
            idx++;
            idx %= transform.childCount;
            yield return new WaitForSeconds(autoRotate - 0.1f);
        }
    }
}
