using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.EventHandlers;
using System;
using UnityEngine;


namespace SliceCase
{
    public class ScObject : BzSliceableObjectBase, IBzObjectSlicedEvent
    {
        private int sliceId = -1;


        public void Slice(Plane plane, int sliceId, Action<BzSliceTryResult> callBack)
        {
            if (this.sliceId == sliceId)
            {
                callBack?.Invoke(new BzSliceTryResult(false, null));
                return;
            }

            this.sliceId = sliceId;
            Slice(plane, callBack);
        }


        protected override BzSliceTryData PrepareData(Plane plane)
        {
            var colliders = gameObject.GetComponentsInChildren<Collider>();
            return new BzSliceTryData
            {
                addData = null, componentManager = new StaticComponentManager(gameObject, plane, colliders),
                plane = plane
            };
        }


        protected override void OnSliceFinished(BzSliceTryResult result) { }


        public void ObjectSliced(GameObject original, GameObject resultNeg, GameObject resultPos)
        {
            if (resultPos != null)
            {
                Destroy(resultPos);
            }
        }
    }
}
