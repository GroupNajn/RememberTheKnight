using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Give Aggro")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Give Aggro", message: "[Agent] is allowed aggro in [Radius] of [Target]", category: "Events", id: "2d9541fc9fea6c3ab21074421b644879")]
public sealed partial class GiveAggroEvent : EventChannel<GameObject, float, GameObject> { }

