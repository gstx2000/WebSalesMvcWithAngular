using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HelpAttribute : Attribute
{
    public string Description { get; }

    public HelpAttribute(string description)
    {
        Description = description;
    }
}
