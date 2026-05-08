using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Request Aggro")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Request Aggro", message: "[Agent] requests aggro on [Target]", category: "Events", id: "4620a7a93106d695a268802d01703ebd")]
public sealed partial class RequestAggroEvent : EventChannel<GameObject, GameObject> { }

