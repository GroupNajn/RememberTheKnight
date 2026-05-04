using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/EnemySpottedEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "EnemySpottedEvent", message: "[Target] spotted", category: "Events", id: "c0b8cc8044ce89355dff1046f2bad29f")]
public sealed partial class EnemySpottedEvent : EventChannel<GameObject> { }

