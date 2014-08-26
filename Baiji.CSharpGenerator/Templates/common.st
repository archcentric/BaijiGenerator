service(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = service definition                                                             = !>
<! =                                                                                = !>
<! = called from the generator to render a new service csharp source file.          = !>
<! =                                                                                = !>
<! ================================================================================== !>
namespace <context.CSharpNamespace>
{
    using System;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    [TContract("<context.ServiceName>", "<context.ServiceNamespace>", "<global.CodeGenVersion>")]
    public interface <context.CSharpName><if(context.CSharpParents)> : <context.CSharpParents; separator=", "><endif>
    {
        <context.Methods : { method |<_method(method)>}; separator="\n">
    }
}
>>

client(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = client definition                                                              = !>
<! =                                                                                = !>
<! = called from the generator to render a new client csharp source file.           = !>
<! =                                                                                = !>
<! ================================================================================== !>
namespace <context.CSharpNamespace>
{
    using CTripOSS.TripRPC.Client;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    public class <context.CSharpName> : ServiceClientBase\<<context.CSharpName>\>
    {
        public const string OriginalServiceName = "<context.ServiceName>";

        public const string OriginalServiceNamespace = "<context.ServiceNamespace>";

        private <context.CSharpName>(string baseUri)
            : base(baseUri)
        {
        }

        private <context.CSharpName>(string serviceName, string serviceNamespace, string subEnv)
            : base(serviceName, serviceNamespace, subEnv)
        {
        }

        <context.Methods : { method |<_clientMethod(method)>}; separator="\n">
    }
}
>>

struct(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = struct definition                                                              = !>
<! =                                                                                = !>
<! = called from the generator to render a new struct csharp source file.           = !>
<! =                                                                                = !>
<! ================================================================================== !>
namespace <context.CSharpNamespace>
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
<if (context.IsServiceResponse)>
    using CTripOSS.TripRPC.Common;
<endif>
<if (tweaks.GEN_PROTOBUF_ATTRIBUTE)>
    using ProtoBuf;
<endif>
    using Baiji.Exception;
    using Baiji.Specific;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    [Serializable]
<if (tweaks.GEN_PROTOBUF_ATTRIBUTE)>
    [ProtoContract]
<endif>
    public partial class <context.CSharpName> : ISpecificRecord<if (context.IsServiceResponse)>, IHasResponseStatus<endif>
    {
        <context.SchemaField>

        <_structbody(context)>

        public Baiji.Schema.Schema Schema
        {
            get
            {
                return SCHEMA;
            }
        }

        <context.GetMethod>
        <context.PutMethod>
        <context.ToStringMethod>
        <context.EqualsMethod>
        <context.HashcodeMethod>
    }<\n>
}
>>

intEnum(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = int based enum definition                                                      = !>
<! =                                                                                = !>
<! = called from the generator to render a new enum chsarp source file.             = !>
<! =                                                                                = !>
<! ================================================================================== !>
namespace <context.CSharpNamespace>
{
    using System;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    public enum <context.CSharpName>
    {
        <context.fields : { field|<_enumElement(field)>}; separator=",\n">,
    }<\n>
}
>>

_docString(lines) ::= <<
/// \<summary>
/// <lines : {field|<field>}; separator="\n/// ">
/// \</summary>
>>

_enumElement(field) ::= <<
<if (tweaks.GEN_COMMENTS && field.DocStringLines)>
<_docString(field.DocStringLines)>
<endif>
<field.CSharpName> = <field.Value>
>>

_method(method) ::= <<
<if (tweaks.GEN_COMMENTS && method.DocStringLines)>
<_docString(method.DocStringLines)>
<endif>
<method.CSharpReturnType> <method.CSharpMethodName>(<if(method.CSharpArgumentType)><method.CSharpArgumentType> <method.CSharpArgumentName><endif>);
>>

_clientMethod(method) ::= <<
<if (tweaks.GEN_COMMENTS && method.DocStringLines)>
<_docString(method.DocStringLines)>
<endif>
public <method.CSharpReturnType> <method.CSharpMethodName>(<if(method.CSharpArgumentType)><method.CSharpArgumentType> <method.CSharpArgumentName><endif>)
{
    return base.Invoke\<<if(method.CSharpArgumentType)><method.CSharpArgumentType>, <endif><method.CSharpReturnType>\>("<method.CSharpMethodName>"<if (method.CSharpArgumentType)>, <method.CSharpArgumentName><endif>);
}
>>

_params(parameters) ::= <<
<if(parameters)>(
    <parameters: {param |<_param(param)>}; separator=",\n">
)<else>()<endif>
>>

_param(param) ::= <<
<param.GenType.CSharpTypeName> <param.CSharpName>
>>

