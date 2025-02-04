﻿// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Fungus
{
    /// <summary>
    /// Applies a jolt of force to a GameObject's rotation and wobbles it back to its initial rotation.
    /// </summary>
    [CommandInfo("iTween", 
                 "Punch Rotation", 
                 "Applies a jolt of force to a GameObject's rotation and wobbles it back to its initial rotation.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class PunchRotation : iTweenCommand
    {
        [Tooltip("A rotation offset in space the GameObject will animate to")]
        [SerializeField] protected Vector3Data _amount;

        [Tooltip("Apply the transformation in either the world coordinate or local cordinate system")]
        [SerializeField] protected Space space = Space.Self;

        #region Public members

        public override void DoTween()
        {
            Hashtable tweenParams = new Hashtable();
            tweenParams.Add("name", _tweenName.Value);
            tweenParams.Add("amount", _amount.Value);
            tweenParams.Add("space", space);
            tweenParams.Add("time", _duration.Value);
            tweenParams.Add("easetype", easeType);
            tweenParams.Add("looptype", loopType);
            tweenParams.Add("oncomplete", "OniTweenComplete");
            tweenParams.Add("oncompletetarget", gameObject);
            tweenParams.Add("oncompleteparams", this);
            iTween.PunchRotation(_targetObject.Value, tweenParams);
        }

        public override bool HasReference(Variable variable)
        {
            return variable == _amount.vector3Ref;
        }

        #endregion

        #region Backwards compatibility

        [HideInInspector] [FormerlySerializedAs("amount")] public Vector3 amountOLD;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (amountOLD != default(Vector3))
            {
                _amount.Value = amountOLD;
                amountOLD = default(Vector3);
            }
        }

        #endregion
    }
}