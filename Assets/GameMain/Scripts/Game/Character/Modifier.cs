using cfg;
using GameFramework;

public class Modifier : IReference
{
    public NumericType NumericType;
    public ModifierType ModifierType;
    public float Value;

    public static Modifier Create(NumericType numericType, ModifierType modifierType, float value)
    {
        Modifier modifier = ReferencePool.Acquire<Modifier>();
        modifier.NumericType = numericType;
        modifier.ModifierType = modifierType;
        modifier.Value = value;

        return modifier;
    }

    public void Clear()
    {
        NumericType = NumericType.None;
        Value = 0f;
    }
}