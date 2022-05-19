// Copyright 2021 Niantic, Inc. All Rights Reserved.

using System;
using System.IO;

using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities.BinarySerialization;
using Niantic.ARDK.Utilities.BinarySerialization.ItemSerializers;

using UnityEngine;

    /// <summary>
    /// A manager that handles outgoing and incoming messages
    /// </summary>
    
public class MessagingManagerChestQuest
    
{
        // Reference to the networking object
        private IMultipeerNetworking _networking;

        // References to the local game controller and ball
        private SharedARManagerScript _controller;
        private Chest _chest;

        // Enums for the various message types
        private enum _MessageType :
          uint
        {
            WheelRotatedMessage = 1,
            SpawnGameObjectsMessage
        }

        // Initialize manager with relevant data and references
        internal void InitializeMessagingManager
        (
          IMultipeerNetworking networking,
          SharedARManagerScript controller
        )
        {
            _networking = networking;
            _controller = controller;

            _networking.PeerDataReceived += OnPeerDataReceived;
        }

        // After the game is created, give a reference to the ball
        
    internal void SetChestReference(Chest chest)
        {
            _chest = chest;
        }
    internal void SpawnGameObjects(Vector3 position)
    {
        Debug.Log("SpawnChest");
        _networking.BroadcastData
        (
          (uint)_MessageType.SpawnGameObjectsMessage,
          SerializationHelper.SerializeVector3(position),
          TransportType.ReliableUnordered, sendToSelf: false
        );
    }

    internal void BroadCastRoundRotation(int roundIndex)
    {
        Debug.Log("Rotation of round: " + roundIndex);
        _networking.BroadcastData
        (
          (uint)_MessageType.WheelRotatedMessage,
          SerializationHelper.SerializeInt(roundIndex),
          TransportType.ReliableOrdered,sendToSelf: false
        );
    }


    void OnPeerDataReceived(PeerDataReceivedArgs args)
    {
        var data = args.CopyData();
        // Log the details of the peer message
        switch ((_MessageType)args.Tag)
        {
            case _MessageType.SpawnGameObjectsMessage:

                _controller.InstantiateObjects(SerializationHelper.DeserializeVector3(data));
                
                Debug.LogFormat("DataReceivedBy: {0}", args.Peer);
                break;

            case _MessageType.WheelRotatedMessage:

                _chest.RotateRound(SerializationHelper.DeserializeInt(data));               
                break;


            default:
                throw new ArgumentException("Received unknown tag from message");
        }

       

        // Properly handle the message depending on the tag and contents
    }

    internal void Destroy()
    {
        _networking.PeerDataReceived -= OnPeerDataReceived;
    }
}

public static class SerializationHelper
{
    public static byte[] SerializeVector3(Vector3 vector)
    {
        using (var stream = new MemoryStream())
        {
            GlobalSerializer.Serialize(stream, vector);
            return stream.ToArray();
        }
    }
    public static Vector3 DeserializeVector3(byte[] data)
    {
        using (var stream = new MemoryStream(data))
            return (Vector3)GlobalSerializer.Deserialize(stream);
    }

    public static byte[] SerializeInt(int integer)
    {
        using (var stream = new MemoryStream())
        {
            GlobalSerializer.Serialize(stream, integer);
            return stream.ToArray();
        }
    }
    public static int DeserializeInt(byte[] data)
    {
        using (var stream = new MemoryStream(data))
            return (int)GlobalSerializer.Deserialize(stream);
    }

    public static byte[] SerializeBool(bool boolean)
    {
        using (var stream = new MemoryStream())
        {
            GlobalSerializer.Serialize(stream, boolean);
            return stream.ToArray();
        }
    }
    public static bool DeserializeBool(byte[] data)
    {
        using (var stream = new MemoryStream(data))
            return (bool)GlobalSerializer.Deserialize(stream);
    }
    public static byte[] SerializePositionAndRotation(Vector3 position, Vector3 rotation)
    {
        using (var stream = new MemoryStream())
        {
            using (var serializer = new BinarySerializer(stream))
            {
                serializer.Serialize(position);
                serializer.Serialize(rotation);
                // here any other object can be serialized.
                return stream.ToArray();
            }
        }
    }

    private static Vector3[] DeserializePositionAndRotation(byte[] data)
    {
        using (var stream = new MemoryStream(data))
        {
            using (var deserializer = new BinaryDeserializer(stream))
            {
                var result = new Vector3[2];
                result[0] = (Vector3)deserializer.Deserialize(); // position
                result[1] = (Vector3)deserializer.Deserialize(); // rotation
                                                                 // The number and order of the Deserialize() calls should match the Serialize() calls.
                return result;
            }
        }
    }
}

