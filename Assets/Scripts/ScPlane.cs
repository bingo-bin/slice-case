using System.Collections.Generic;
using UnityEngine;


namespace SliceCase
{
    public class ScPlane : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Vector3 boxCastSize;
        [SerializeField] private Transform cutPointRoot;


        private RaycastHit[] hitBuffer;
        private HashSet<ScObject> sliceables;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            hitBuffer = new RaycastHit[20];
            sliceables = new HashSet<ScObject>();
        }


        private void Update()
        {
            int hitsCount = Physics.BoxCastNonAlloc(cutPointRoot.position + boxCastSize.y * cutPointRoot.up,
                0.5f * boxCastSize, -cutPointRoot.up, hitBuffer);

            if (hitsCount == 0)
            {
                return;
            }

            sliceables.Clear();

            for (int i = 0; i < hitsCount; i++)
            {
                ScObject s = hitBuffer[i].collider.gameObject.GetComponentInParent<ScObject>();
                if (s != null)
                {
                    sliceables.Add(s);
                }
            }

            Plane cutPlane = new Plane(transform.forward, transform.position);
            foreach (var s in sliceables)
            {
                s.Slice(cutPlane, Time.frameCount, result => { });
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, boxCastSize);
        }

        #endregion
    }
}
