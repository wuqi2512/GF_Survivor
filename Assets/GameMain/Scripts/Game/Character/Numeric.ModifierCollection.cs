using GameFramework;
using System.Collections.Generic;

public partial class Numeric
{
    private class ModifierCollection : IReference
    {
        public List<Modifier> Modifiers;

        public ModifierCollection()
        {
            Modifiers = new List<Modifier>();
        }

        public float AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            return UpdateValue();
        }

        public float RemoveModifier(Modifier modifier)
        {
            Modifiers.Remove(modifier);
            return UpdateValue();
        }

        public float UpdateValue()
        {
            float value = 0;
            foreach (var item in Modifiers)
            {
                value += item.Value;
            }

            return value;
        }

        public void Clear()
        {
            Modifiers.Clear();
        }
    }
}