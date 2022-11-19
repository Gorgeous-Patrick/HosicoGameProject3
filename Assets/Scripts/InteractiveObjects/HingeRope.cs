using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeRope : MonoBehaviour
{
    HingeJoint2D hinge;
    GameObject _upperObject;
    GameObject _lowerObject = null;
    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        if (hinge == null)
        {
            return;
        }
        _upperObject = hinge.connectedBody.gameObject;
        if (_upperObject != null)
        {
            _upperObject.GetComponent<HingeRope>()._lowerObject = gameObject;
        }
    }

    public GameObject upperObject
    {
        get
        {
            return _upperObject;
        }
    }
    public GameObject lowerObject
    {
        get
        {
            return _lowerObject;
        }
    }
}
