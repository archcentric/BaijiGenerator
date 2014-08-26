namespace CTripOSS.Baiji.CSharpGenerator
{
    /// <summary>
    /// Code generator tweaks
    /// </summary>
    public enum GeneratorTweak
    {
        NONE,
        ADD_DISPOSABLE_INTERFACE, // Make generated Service extend IDisposable and add a Dispose() method
        USE_PLAIN_CSHARP_NAMESPACE,  // Use the csharp namespace, not the TripIdl namespace
        GEN_PROTOBUF_ATTRIBUTE,
        GEN_COMMENTS,
        GEN_CLIENT_PROXY, // Generate client proxy instead of service stub
    }
}
