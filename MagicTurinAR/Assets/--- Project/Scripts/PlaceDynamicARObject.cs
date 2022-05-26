// Copyright 2021 Niantic, Inc. All Rights Reserved.

using System.Collections.Generic;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Anchors;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Logging;

using UnityEngine;

namespace Niantic.ARDKExamples.Helpers
{
    //! A helper class that demonstrates hit tests based on user input
    /// <summary>
    /// A sample class that can be added to a scene and takes user input in the form of a screen touch.
    ///   A hit test is run from that location. If a plane is found, spawn a game object at the
    ///   hit location.
    /// </summary>
    public class PlaceDynamicARObject : MonoBehaviour
    {
        /// The camera used to render the scene. Used to get the center of the screen.
        public Camera Camera;
        public GameObject spawnedObjectParent;
        private int counterTouch = 0;
        /// The types of hit test results to filter against when performing a hit test.
        [EnumFlagAttribute]
        public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;

        /// The object we will place when we get a valid hit test result!
        public GameObject PlacementObjectPf;

        /// A list of placed game objects to be destroyed in the OnDestroy method.
        private GameObject _placedObject = null;

        /// Internal reference to the session, used to get the current frame to hit test against.
        private IARSession _session;

        [SerializeField] private float offsetY;
        private bool letTouch = false;

        public bool LetTouch
        {
            get => letTouch;
            set => letTouch = value;
        }

        public int GetCounterTouch()
        {
            return counterTouch;
        }

        public void SetCounterTouchToZero()
        {
            counterTouch = 0;
        }
        private void Start()
        {
            ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
        }

        private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
        {
            _session = args.Session;
            _session.Deinitialized += OnSessionDeinitialized;
        }

        private void OnSessionDeinitialized(ARSessionDeinitializedArgs args)
        {
            ClearObjects();
        }

        private void OnDestroy()
        {
            ARSessionFactory.SessionInitialized -= OnAnyARSessionDidInitialize;

            _session = null;

            ClearObjects();
        }

        private void ClearObjects()
        {
            Destroy(_placedObject);
            _placedObject = null;
        }

        private void Update()
        {
            if (_session == null)
            {
                return;
            }

            if (PlatformAgnosticInput.touchCount <= 0)
            {
                return;
            }

            var touch = PlatformAgnosticInput.GetTouch(0);
            if (touch.phase == TouchPhase.Began && GetCounterTouch() < 1 && letTouch)
            {
                ClearObjects();
                TouchBegan(touch);
               
            }
        }

        private void TouchBegan(Touch touch)
        {
          
            var currentFrame = _session.CurrentFrame;
            if (currentFrame == null)
            {
                return;
            }

            var results = currentFrame.HitTest
            (
              Camera.pixelWidth,
              Camera.pixelHeight,
              touch.position,
              HitTestType
            );

            int count = results.Count;
            Debug.Log("Hit test results: " + count);

            if (count <= 0)
                return;

            // Get the closest result
            var result = results[0];

            var hitPosition = result.WorldTransform.ToPosition();

            hitPosition.y += offsetY;


            _placedObject = Instantiate(PlacementObjectPf, hitPosition, Quaternion.identity);
            if(spawnedObjectParent != null)
                _placedObject.transform.parent = spawnedObjectParent.transform;
            counterTouch++;
            Vibration.VibratePop();

            var anchor = result.Anchor;
           
        }
    }
}