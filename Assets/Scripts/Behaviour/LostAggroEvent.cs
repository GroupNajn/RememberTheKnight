using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Lost Aggro")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Lost Aggro", message: "[Agent] looses aggro on [Target]", category: "Events", id: "d8cfe28bbafafaa7f7e00bee372afc0d")]
public sealed partial class LostAggroEvent : EventChannel<GameObject, GameObject> { }

