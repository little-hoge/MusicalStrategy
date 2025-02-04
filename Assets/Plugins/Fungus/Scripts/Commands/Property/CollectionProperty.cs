﻿/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    // <summary>
    /// Get or Set a property of a Collection component
    /// </summary>
    [CommandInfo("Property",
                 "Collection",
                 "Get or Set a property of a Collection component")]
    [AddComponentMenu("")]
    public class CollectionProperty : BaseVariableProperty
    {
		//generated property
        public enum Property 
        { 
            Capacity, 
            Count, 
            IsFixedSize, 
            IsReadOnly, 
            IsSynchronized, 
            Name, 
        }

		
        [SerializeField]
        protected Property property;
		
        [SerializeField]
        protected CollectionData collectionData;

        [SerializeField]
        [VariableProperty(typeof(IntegerVariable),
                          typeof(BooleanVariable),
                          typeof(StringVariable))]
        protected Variable inOutVar;

        public override void OnEnter()
        {
            var ioi = inOutVar as IntegerVariable;
            var iob = inOutVar as BooleanVariable;
            var ios = inOutVar as StringVariable;


            var target = collectionData.Value;

            switch (getOrSet)
            {
                case GetSet.Get:
                    switch (property)
                    {
                        case Property.Capacity:
                            ioi.Value = target.Capacity;
                            break;
                        case Property.Count:
                            ioi.Value = target.Count;
                            break;
                        case Property.IsFixedSize:
                            iob.Value = target.IsFixedSize;
                            break;
                        case Property.IsReadOnly:
                            iob.Value = target.IsReadOnly;
                            break;
                        case Property.IsSynchronized:
                            iob.Value = target.IsSynchronized;
                            break;
                        case Property.Name:
                            ios.Value = target.Name;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                case GetSet.Set:
                    switch (property)
                    {
                        case Property.Capacity:
                            target.Capacity = ioi.Value;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                default:
                    break;
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (collectionData.Value == null)
            {
                return "Error: no collection set";
            }
            if (inOutVar == null)
            {
                return "Error: no variable set to push or pull data to or from";
            }

            return getOrSet.ToString() + " " + property.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            if (collectionData.collectionRef == variable || inOutVar == variable)
                return true;

            return false;
        }

    }
}