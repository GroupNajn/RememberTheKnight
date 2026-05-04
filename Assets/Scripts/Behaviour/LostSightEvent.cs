using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/LostSightEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Lost Sight Event", message: "Enemy moved out of sight, last known [Position]", category: "Events", id: "66462329a214cf587a8a5391fad81c21")]
public sealed partial class LostSightEvent : EventChannel<Vector3> { }

