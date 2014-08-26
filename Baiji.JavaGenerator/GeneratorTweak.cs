namespace CTripOSS.Baiji.JavaGenerator
{
    /// <summary>
    /// Code generator tweaks
    /// </summary>
    public enum GeneratorTweak
    {
        NONE,
        USE_PLAIN_JAVA_NAMESPACE,  // Use the java namespace, not the trip namespace
        GEN_COMMENTS,
        GEN_PUBLIC_FIELDS, // Generate public fields. If not specified, private fields will be generated.
    }
}
