﻿// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Loads a saved value and stores it in a Boolean, Integer, Float or String variable. If the key is not found then the variable is not modified.
    /// </summary>
    [CommandInfo("Variable", 
                 "Load Variable", 
                 "Loads a saved value and stores it in a Boolean, Integer, Float or String variable. If the key is not found then the variable is not modified.")]
    [AddComponentMenu("")]
    public class LoadVariable : Command
    {
        [Tooltip("Name of the saved value. Supports variable substition e.g. \"player_{$PlayerNumber}\"")]
        [SerializeField] protected string key = "";

        [Tooltip("Variable to store the value in.")]
        [VariableProperty(typeof(BooleanVariable),
                          typeof(IntegerVariable), 
                          typeof(FloatVariable), 
                          typeof(StringVariable))]
        [SerializeField] protected Variable variable;

        #region Public members

        public override void OnEnter()
        {
            var flowchart = GetFlowchart();

            // Prepend the current save profile (if any) and make sure all inputs are valid
            string prefsKey = SetSaveProfile.SaveProfile + "_" + flowchart.SubstituteVariables(key);
            bool validKey = key != "" && PlayerPrefs.HasKey(prefsKey);
            bool validVariable = variable != null;

            if (!validKey || !validVariable)
            {
                Continue();
                return;
            }

            System.Type variableType = variable.GetType();

            if (variableType == typeof(BooleanVariable))
            {
                BooleanVariable booleanVariable = variable as BooleanVariable;
                if (booleanVariable != null)
                {
                    // PlayerPrefs does not have bool accessors, so just use int
                    booleanVariable.Value = (PlayerPrefs.GetInt(prefsKey) == 1);
                }
            }
            else if (variableType == typeof(IntegerVariable))
            {
                IntegerVariable integerVariable = variable as IntegerVariable;
                if (integerVariable != null)
                {
                    integerVariable.Value = PlayerPrefs.GetInt(prefsKey);
                }
            }
            else if (variableType == typeof(FloatVariable))
            {
                FloatVariable floatVariable = variable as FloatVariable;
                if (floatVariable != null)
                {
                    floatVariable.Value = PlayerPrefs.GetFloat(prefsKey);
                }
            }
            else if (variableType == typeof(StringVariable))
            {
                StringVariable stringVariable = variable as StringVariable;
                if (stringVariable != null)
                {
                    stringVariable.Value = PlayerPrefs.GetString(prefsKey);
                }
            }

            Continue();
        }
        
        public override string GetSummary()
        {
            if (key.Length == 0)
            {
                return "Error: No stored value key selected";
            }
        
            if (variable == null)
            {
                return "Error: No variable selected";
            }

            return "'" + key + "' into " + variable.Key;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable in_variable)
        {
            return this.variable == in_variable ||
                base.HasReference(in_variable);
        }

        #endregion
        #region Editor caches
#if UNITY_EDITOR
        protected override void RefreshVariableCache()
        {
            base.RefreshVariableCache();

            var f = GetFlowchart();

            f.DetermineSubstituteVariables(key, referencedVariables);
        }
#endif
        #endregion Editor caches
    }
}