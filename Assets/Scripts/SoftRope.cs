using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SoftRope : MonoBehaviour
{

  [SerializeField] uint length = 10;
  [SerializeField] GameObject segmentPrefab;

  int segc
  {
    get => segmentContainer.transform.childCount;
  }
  GameObject lastSeg
  {
    get => segc == 0 ? null : segmentContainer.transform.GetChild(segc - 1).gameObject;
  }
  GameObject anchor, segmentContainer;

  void Update()
  {
    anchor = transform.Find("anchor").gameObject;
    segmentContainer = transform.Find("segments").gameObject;

    GameObject oldseg = lastSeg;

    while (segc < length)
    {
      // make the rope longer
      GameObject newseg = Instantiate(segmentPrefab, segmentContainer.transform);
      newseg.name = $"segment{segc}";
      newseg.transform.localPosition = new Vector3(0, -0.1f * segc, 0);
      HingeJoint2D hj2d = newseg.GetComponent<HingeJoint2D>();
      RopeSegment rs_new = newseg.GetComponent<RopeSegment>();

      if (oldseg == null) // no segments exist
      {
        rs_new.prev = null;
        hj2d.connectedBody = anchor.GetComponent<Rigidbody2D>();
      }
      else
      {
        RopeSegment rs_old = oldseg.GetComponent<RopeSegment>();
        hj2d.connectedBody = oldseg.GetComponent<Rigidbody2D>();
        rs_old.next = newseg;
        rs_new.prev = oldseg;
      }
      oldseg = newseg;
    }
    while (segc > length)
    {
      // make the rope shorter
      DestroyImmediate(lastSeg);
    }
    if (segc > 0)
      lastSeg.GetComponent<RopeSegment>().next = null;
  }
}
