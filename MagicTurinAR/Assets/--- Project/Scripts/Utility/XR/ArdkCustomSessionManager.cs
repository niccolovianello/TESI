using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Anchors;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.ReferenceImage;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Collections;
using UnityEngine;

namespace ____Project.Scripts.Utility.XR
{
    public class ArdkCustomSessionManager : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToSpawn;

        private IARSession _arSession;
        
        /*
        
        private readonly HashSet<string> _imagesToAttachTo = new HashSet<string>(
            new string[]{
                "imageToAttachTo",
                "anotherImageToAttachTo"
            });
 
        private bool ShouldAttachToImage(IARReferenceImage image)
        {
            return _imagesToAttachTo.Contains(image.Name);
        }
 
        Dictionary<Guid, GameObject> _attachedAnchors = new Dictionary<Guid, GameObject>();
 
        private void SetupCallbacks()
        {
            _arSession.AnchorsAdded += OnAnchorsAdded;
            _arSession.AnchorsUpdated += OnAnchorsUpdated;
        }
 
        private void OnAnchorsAdded(AnchorsArgs args)
        {
            foreach (var anchor in args.Anchors)
            {
                if (anchor.AnchorType != AnchorType.Image)
                    continue;

                if (!(anchor is IARImageAnchor imageAnchor))
                    continue;
 
                IARAnchor anchorToAttachTo;
                if (ShouldAttachToImage())
                {
                    anchorToAttachTo = anchor;
                }
                else
                {
                    anchorToAttachTo = _arSession.AddAnchor(anchor.Transform, 1.0f);
                }
    
                var attachedObject = Instantiate(prefabToSpawn, );  // Create whatever game object should be attached.
                _attachedAnchors[anchor.Identifier] = attachedObject;
                attachedObject.transform.position = anchor.Transform.ToPosition();
                attachedObject.transform.rotation = anchor.Transform.ToRotation();
            }
        }
 
        private void OnAnchorsUpdated(AnchorsArgs args)
        {
            foreach (var anchor in args.Anchors)
            {
                if (!_attachedAnchors.ContainsKey(anchor.Identifier))
                    continue;
 
                _detectedImages[anchor.Identifier].transform.position = anchor.Transform.ToPosition();
                _detectedImages[anchor.Identifier].transform.rotation = anchor.Transform.ToRotation();
            }
        }
        */

        private void Start()
        {
            _arSession = ARSessionFactory.Create();
            _arSession.Run(ARWorldTrackingConfigurationFactory.Create());
        }

        public IEnumerator RunSessionWithImageDetectionAsynchronously(IARWorldTrackingConfiguration config)
        {
            const float imageSize = 0.21f; // 30 centimeters.
 
            var imageSet = new HashSet<IARReferenceImage>();
 
            var imageProcessed = false;

            void AddImageToSet(IARReferenceImage image)
            {
                if (image != null) imageSet.Add(image);

                imageProcessed = true;
            }

            const string jpegFilePath = "path/to/your/file.jpg";
            ARReferenceImageFactory.CreateAsync("image1", jpegFilePath, imageSize, AddImageToSet);
 
            while (!imageProcessed)
                yield return null;
  
            imageProcessed = false;
 
            var jpegFileContents = Array.Empty<byte>(); // Load jpeg from a file, or receive the bytes from the network.
 
            ARReferenceImageFactory.CreateAsync
                ("image2", jpegFileContents, jpegFileContents.Length, imageSize, AddImageToSet);
 
            while (!imageProcessed)
                yield return null;
 
            imageProcessed = false;
 
            // The raw RGBA values of an image, possibly loaded from a file like a PNG, received from the network, or extracted from the camera.
            // In this case they're in ARGB format.
            var rawImage = Array.Empty<byte>();
            const int rawImageWidth = 300;
            const int rawImageHeight = 300;
            ARReferenceImageFactory.CreateAsync
            (
                "image3",
                rawImage,
                rawImageWidth,
                rawImageHeight,
                ByteOrderInfo.big32,
                AlphaInfo.First,
                4,
                imageSize,
                AddImageToSet
            );
  
            while (!imageProcessed)
                yield return null;
  
            imageProcessed = false;
 
            config.SetDetectionImagesAsync
            (
                imageSet.AsArdkReadOnly(),
                delegate { _arSession.Run(config); }
            );
        }
    }
}
