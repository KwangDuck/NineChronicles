using System;
using System.Collections.Generic;
using Nekoyume.Game.Item;
using Nekoyume.Helper;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine;

namespace Nekoyume.Game.Character
{
    [RequireComponent(typeof(SkeletonAnimation))]
    public class SkeletonAnimationController : MonoBehaviour
    {
        [Serializable]
        public class StateNameToAnimationReference
        {
            public string stateName;
            public AnimationReferenceAsset animation;
        }

        public const string DefaultPMAShader = "Spine/Skeleton";
        public const string DefaultStraightAlphaShader = "Sprites/Default";
        private const string WeaponSlot = "weapon";
        private const string TailSlot = "tail_0002";

        public List<StateNameToAnimationReference> statesAndAnimations = new List<StateNameToAnimationReference>();

        public SkeletonAnimation SkeletonAnimation { get; private set; }

        private Spine.Animation TargetAnimation { get; set; }

        private Skin _clonedSkin;
        private bool _applyPMA;
        
        private Shader _weaponShader;
        private AtlasPage _weaponAtlasPage;
        private int _weaponSlotIndex;
        private RegionAttachment _defaultWeaponAttachment;
        private RegionAttachment _currentWeaponAttachment;
        
        private Shader _tailShader;
        private AtlasPage _tailAtlasPage;
        private int _tailSlotIndex;
        private RegionAttachment _defaultTailAttachment;
        private RegionAttachment _currentTailAttachment;

        #region Mono

        private void Awake()
        {
            foreach (var entry in statesAndAnimations)
            {
                entry.animation.Initialize();
            }

            SkeletonAnimation = GetComponent<SkeletonAnimation>();

            _clonedSkin = SkeletonAnimation.skeleton.Data.DefaultSkin.GetClone();
            _applyPMA = SkeletonAnimation.pmaVertexColors;
            
            _weaponShader = _applyPMA ? Shader.Find(DefaultPMAShader) : Shader.Find(DefaultStraightAlphaShader);
            _weaponAtlasPage = new UnityEngine.Material(_weaponShader).ToSpineAtlasPage();
            _weaponSlotIndex = SkeletonAnimation.skeleton.FindSlotIndex(WeaponSlot);
            _defaultWeaponAttachment = MakeWeaponAttachment(SpriteHelper.GetPlayerSpineTextureWeapon(GameConfig.DefaultAvatarWeaponId));
            
            _tailShader = _applyPMA ? Shader.Find(DefaultPMAShader) : Shader.Find(DefaultStraightAlphaShader);
            _tailAtlasPage = new UnityEngine.Material(_tailShader).ToSpineAtlasPage();
            _tailSlotIndex = SkeletonAnimation.skeleton.FindSlotIndex(TailSlot);
            _defaultTailAttachment = MakeTailAttachment(SpriteHelper.GetPlayerSpineTextureTail(null));
        }

        #endregion

        /// <summary>Sets the horizontal flip state of the skeleton based on a nonzero float. If negative, the skeleton is flipped. If positive, the skeleton is not flipped.</summary>
        public void SetFlip(float horizontal)
        {
            if (Math.Abs(horizontal) > 0f)
            {
                SkeletonAnimation.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
            }
        }

        /// <summary>Plays an  animation based on the hash of the state name.</summary>
        public TrackEntry PlayAnimationForState(int shortNameHash, int layerIndex)
        {
            var foundAnimation = GetAnimationForState(shortNameHash);
            if (foundAnimation == null)
            {
                return null;
            }

            return PlayNewAnimation(foundAnimation, layerIndex);
        }

        public TrackEntry PlayAnimationForState(string stateName, int layerIndex)
        {
            var foundAnimation = GetAnimationForState(stateName);
            if (foundAnimation == null)
            {
                return null;
            }

            return PlayNewAnimation(foundAnimation, layerIndex);
        }

        /// <summary>Play a non-looping animation once then continue playing the state animation.</summary>
        public void PlayOneShot(Spine.Animation oneShot, int layerIndex)
        {
            var state = SkeletonAnimation.AnimationState;
            state.SetAnimation(0, oneShot, false);
            state.AddAnimation(0, TargetAnimation, true, 0f);
        }

        private int StringToHash(string s)
        {
            return Animator.StringToHash(s);
        }

        public void UpdateWeapon(Sprite sprite)
        {
            if (sprite is null)
            {
                _clonedSkin.SetAttachment(_weaponSlotIndex, WeaponSlot, _defaultWeaponAttachment);
            }
            else
            {
                var newWeapon = MakeWeaponAttachment(sprite);
                _clonedSkin.SetAttachment(_weaponSlotIndex, WeaponSlot, newWeapon);
            }

            var skeleton = SkeletonAnimation.skeleton;
            skeleton.SetSkin(_clonedSkin);
            skeleton.SetSlotsToSetupPose();
            SkeletonAnimation.Update(0);
        }

        public void UpdateTail(Sprite sprite)
        {
            if (sprite is null)
            {
                _clonedSkin.SetAttachment(_tailSlotIndex, TailSlot, _defaultTailAttachment);
            }
            else
            {
                var newTail = MakeTailAttachment(sprite);
                _clonedSkin.SetAttachment(_tailSlotIndex, TailSlot, newTail);
            }

            var skeleton = SkeletonAnimation.skeleton;
            skeleton.SetSkin(_clonedSkin);
            skeleton.SetSlotsToSetupPose();
            SkeletonAnimation.Update(0);
        }

        private RegionAttachment MakeWeaponAttachment(Sprite sprite)
        {
            var attachment = _applyPMA
                ? sprite.ToRegionAttachmentPMAClone(_weaponShader)
                : sprite.ToRegionAttachment(_weaponAtlasPage);

            return attachment;
        }

        private RegionAttachment MakeTailAttachment(Sprite sprite)
        {
            var attachment = _applyPMA
                ? sprite.ToRegionAttachmentPMAClone(_tailShader)
                : sprite.ToRegionAttachment(_tailAtlasPage);

            return attachment;
        }

        /// <summary>Gets a Spine Animation based on the hash of the state name.</summary>
        private Spine.Animation GetAnimationForState(int shortNameHash)
        {
            var foundState = statesAndAnimations.Find(entry => StringToHash(entry.stateName) == shortNameHash);
            return foundState?.animation;
        }

        private Spine.Animation GetAnimationForState(string stateName)
        {
            var foundState = statesAndAnimations.Find(entry => entry.stateName == stateName);
            return foundState?.animation;
        }

        /// <summary>Play an animation. If a transition animation is defined, the transition is played before the target animation being passed.</summary>
        private TrackEntry PlayNewAnimation(Spine.Animation target, int layerIndex)
        {
            var loop = target.Name == nameof(CharacterAnimation.Type.Idle)
                       || target.Name == nameof(CharacterAnimation.Type.Run)
                       || target.Name == nameof(CharacterAnimation.Type.Casting);

            TargetAnimation = target;
            return SkeletonAnimation.AnimationState.SetAnimation(layerIndex, target, loop);
        }
    }
}
