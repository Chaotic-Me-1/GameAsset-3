using UnityEngine;

public class _IKSystem3d : MonoBehaviour
{
    public _Segment3d[] segments;
    public int childcount;
    public Transform target;

    public bool isReaching;
    public bool isDragging;
    private _Segment3d firstSegment;

    private _Segment3d lastSegment;


    // Use this for initialization
    private void Awake()
    {
        //lets buffer our segements in an array
        childcount = transform.childCount;
        segments = new _Segment3d[childcount];
        var i = 0;
        foreach (Transform child in transform)
        {
            segments[i] = child.GetComponent<_Segment3d>();
            i++;
        }


        firstSegment = segments[0];
        lastSegment = segments[childcount - 1];
    }

    // Update is called once per frame
    private void Update()
    {
        // Toggle IK on or off based on input
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isReaching = true;
            // Optionally, update target.position based on mouse movement or other input
        }
        else
        {
            isReaching = false;
        }

        if (isReaching)
        {
            lastSegment.reach(target.position);
            // Reset the first segment’s position to keep the chain anchored
            firstSegment.transform.position = transform.position;
            firstSegment.updateSegmentAndChildren();
        }
    }
}