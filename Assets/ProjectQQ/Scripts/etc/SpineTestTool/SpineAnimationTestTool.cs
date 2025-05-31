using System.Collections.Generic;
using System.Text;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace ProjectQQ.Scripts.etc.SpineTestTool
{
    public class SpineAnimationTestTool : MonoBehaviour, IHasSkeletonDataAsset, IHasSkeletonComponent
    {
        public SkeletonAnimation skeletonAnimation;
        
        public SkeletonDataAsset SkeletonDataAsset { get{ return skeletonAnimation.skeletonDataAsset; } }
        public ISkeletonComponent SkeletonComponent { get { return skeletonAnimation; } }
        
        public bool useOverrideMixDuration;
        public float overrideMixDuration = 0.2f;

        public bool useOverrideAttachmentThreshold = true;

        [Range(0f, 1f)]
        public float attachmentThreshold = 0.5f;

        public bool useOverrideDrawOrderThreshold;
        [Range(0f, 1f)]
        public float drawOrderThreshold = 0.5f;
        
        [System.Serializable]
        public struct AnimationControl 
        {
            [SpineAnimation]
            public string animationName;
            public bool loop;
            public KeyCode key;

            [Space]
            public bool useCustomMixDuration;
            public float mixDuration;
        }
        
        [System.Serializable]
        public class ControlledTrack 
        {
            public List<AnimationControl> controls = new List<AnimationControl>();
        }
        
        [Space]
        public List<ControlledTrack> trackControls = new List<ControlledTrack>();

        [Header("# UIs")] 
        public TextMeshProUGUI boundAnimationsText;
        public TextMeshProUGUI skeletonNameText;
        public TMP_Dropdown skeletonDropdown;
        public TMP_Dropdown skinDropdown;
        
        private Dictionary<string, SkeletonDataAsset> skeletonDataAssets;
        
        void OnValidate () 
        {
            // Fill in the SkeletonData asset name
            if (skeletonNameText != null) {
                if (skeletonAnimation != null && skeletonAnimation.skeletonDataAsset != null) {
                    skeletonNameText.text = SkeletonDataAsset.name.Replace("_SkeletonData", "");
                }
            }

            // Fill in the control list.
            if (boundAnimationsText != null) {
                StringBuilder boundAnimationsStringBuilder = new StringBuilder();
                boundAnimationsStringBuilder.AppendLine("Animation Controls:");

                for (int trackIndex = 0; trackIndex < trackControls.Count; trackIndex++) {

                    if (trackIndex > 0)
                        boundAnimationsStringBuilder.AppendLine();

                    boundAnimationsStringBuilder.AppendFormat("---- Track {0} ---- \n", trackIndex);
                    foreach (AnimationControl ba in trackControls[trackIndex].controls) {
                        string animationName = ba.animationName;
                        if (string.IsNullOrEmpty(animationName))
                            animationName = "SetEmptyAnimation";

                        boundAnimationsStringBuilder.AppendFormat("[{0}]  {1}\n", ba.key.ToString(), animationName);
                    }

                }

                boundAnimationsText.text = boundAnimationsStringBuilder.ToString();

                skeletonDataAssets = new Dictionary<string, SkeletonDataAsset>();
                SkeletonDataAsset[] allAssets = Resources.LoadAll<SkeletonDataAsset>("SpineAnimations");

                foreach (var VARIABLE in allAssets)
                {
                    string key = VARIABLE.name;

                    if (!skeletonDataAssets.ContainsKey(key))
                    {
                        skeletonDataAssets.Add(key, VARIABLE);
                    }
                    else
                    {
                        Debug.LogWarning($"중복된 스켈레톤 데이터 키 발견 : {key}");
                    }
                }
				
                List<string> skeletonNames = new List<string>();
                foreach (var VARIABLE in skeletonDataAssets)
                {
                    skeletonNames.Add(VARIABLE.Key);
                }
				
                skeletonDropdown.ClearOptions();
                skeletonDropdown.AddOptions(skeletonNames);
            }
        }
        
        void Start () 
        {
            if (useOverrideMixDuration) {
                skeletonAnimation.AnimationState.Data.DefaultMix = overrideMixDuration;
            }
            
            OnSkeletonDropdownValueChanged();
        }
        
        void Update () 
        {
            AnimationState animationState = skeletonAnimation.AnimationState;

            // For each track
            for (int trackIndex = 0; trackIndex < trackControls.Count; trackIndex++) {

                // For each control in the track
                foreach (AnimationControl control in trackControls[trackIndex].controls) {

                    // Check each control, and play the appropriate animation.
                    if (Input.GetKeyDown(control.key)) {
                        TrackEntry trackEntry;
                        if (!string.IsNullOrEmpty(control.animationName)) {
                            trackEntry = animationState.SetAnimation(trackIndex, control.animationName, control.loop);
                        } else {
                            float mix = control.useCustomMixDuration ? control.mixDuration : animationState.Data.DefaultMix;
                            trackEntry = animationState.SetEmptyAnimation(trackIndex, mix);
                        }

                        if (trackEntry != null) {
                            if (control.useCustomMixDuration)
                                trackEntry.SetMixDuration(control.mixDuration, 0f); // use SetMixDuration(mixDuration, delay) to update delay correctly

                            if (useOverrideAttachmentThreshold)
                                trackEntry.MixAttachmentThreshold = attachmentThreshold;

                            if (useOverrideDrawOrderThreshold)
                                trackEntry.MixDrawOrderThreshold = drawOrderThreshold;
                        }

                        // Don't parse more than one animation per track.
                        break;
                    }
                }
            }

        }
        
        public void OnSkeletonDropdownValueChanged()
        {
            var choosenSkeleton = new SkeletonDataAsset();

            foreach (var VARIABLE in skeletonDataAssets)
            {
                if (VARIABLE.Key == skeletonDropdown.captionText.text)
                {
                    choosenSkeleton = VARIABLE.Value;
                    break;
                }
            }
			
            skeletonAnimation.skeletonDataAsset = choosenSkeleton;
            skeletonAnimation.AnimationName = "idle";
            skeletonAnimation.Initialize(true);

            RefreshSkinDropdown(choosenSkeleton);
        }
        
        private void RefreshSkinDropdown(SkeletonDataAsset choosenSkeleton)
        {
            skinDropdown.ClearOptions();
			
            List<string> skinNames = new List<string>();
            foreach (var VARIABLE in choosenSkeleton.GetSkeletonData(true).Skins)
            {
                skinNames.Add(VARIABLE.Name);
            }
            skinDropdown.AddOptions(skinNames);
        }

        public void OnSkinDropdownValueChanged()
        {
            skeletonAnimation.initialSkinName = skinDropdown.captionText.text;
            skeletonAnimation.Initialize(true);
        }

    }
}