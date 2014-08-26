service(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = service definition                                                             = !>
<! =                                                                                = !>
<! = called from the generator to render a new service csharp source file.          = !>
<! =                                                                                = !>
<! ================================================================================== !>
namespace <context.Namespace>
{
    using System;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    [TContract("<context.ServiceName>", "<context.ServiceNamespace>", "<global.CodeGenVersion>")]
    public interface <context.TypeName><if(context.Parents)> : <context.Parents; separator=", "><endif>
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
namespace <context.Namespace>
{
using CTripOSS.Baiji.Rpc;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    public class <context.TypeName> : ServiceClientBase\<<context.TypeName>\>
    {
        public const string OriginalServiceName = "<context.ServiceName>";

        public const string OriginalServiceNamespace = "<context.ServiceNamespace>";

        private <context.TypeName>(string baseUri)
            : base(baseUri)
        {
        }

        private <context.TypeName>(string serviceName, string serviceNamespace, string subEnv)
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
namespace <context.Namespace>
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using CTripOSS.Baiji.Exception;
    using CTripOSS.Baiji.Specific;
    using CTripOSS.Baiji.Utils;

<if (context.IsServiceResponse)>
    using CTripOSS.Baiji.Rpc.Common.IHasResponseStatus;
<endif>

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    [Serializable]
    public partial class <context.TypeName> : SpecificRecordBase<if (context.IsServiceResponse)>, IHasResponseStatus<endif>
    {
        public static readonly CTripOSS.Baiji.Schema.Schema SCHEMA = CTripOSS.Baiji.Schema.Schema.Parse("<context.EscapedSchemaText>");

        <if(context.Fields)>
        <_structbody(context)>
        <endif>

        public override CTripOSS.Baiji.Schema.Schema Schema
        {
            get
            {
                return SCHEMA;
            }
        }

        <_get(context)>

        <_put(context)>

        <if(context.Fields)>
        <_equals(context)>

        <_hashCode(context)>

        <_toString(context)>
        <endif>
    }
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
namespace <context.Namespace>
{
    using System;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
    <_docString(context.DocStringLines)>
<endif>
    public enum <context.TypeName>
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
<field.Name> = <field.Value>
>>

_method(method) ::= <<
<if (tweaks.GEN_COMMENTS && method.DocStringLines)>
<_docString(method.DocStringLines)>
<endif>
<method.ReturnType> <method.MethodName>(<if(method.ArgumentType)><method.ArgumentType> <method.ArgumentName><endif>);
>>

_clientMethod(method) ::= <<
<if (tweaks.GEN_COMMENTS && method.DocStringLines)>
<_docString(method.DocStringLines)>
<endif>
public <method.ReturnType> <method.MethodName>(<if(method.ArgumentType)><method.ArgumentType> <method.ArgumentName><endif>)
{
    return base.Invoke\<<if(method.ArgumentType)><method.ArgumentType>, <endif><method.ReturnType>\>("<method.MethodName>"<if (method.ArgumentType)>, <method.ArgumentName><endif>);
}
>>

_params(parameters) ::= <<
<if(parameters)>(
    <parameters: {param |<_param(param)>}; separator=",\n">
)<else>()<endif>
>>

_param(param) ::= <<
<param.GenType.TypeName> <param.CodeName>
>>

_get(structContext) ::= <<
// Used by DatumWriter. Applications should not call.
public override object Get(int fieldPos)
{
    switch (fieldPos)
    {
        <structContext.Fields : {field | <_getField(field, i0)>}; separator="\n">
        default: throw new BaijiRuntimeException("Bad index " + fieldPos + " in Get()");
    }
}
>>

_getField(fieldContext, i0) ::= <<
case <i0>: return this.<fieldContext.PropertyName>;
>>

_put(structContext) ::= <<
// Used by DatumReader. Applications should not call.
public override void Put(int fieldPos, object fieldValue)
{
    switch (fieldPos)
    {
        <structContext.Fields : {field | <_putField(field, i0)>}; separator="\n">
        default: throw new BaijiRuntimeException("Bad index " + fieldPos + " in Put()");
    }
}
>>

_putField(fieldContext, i0) ::= <<
case <i0>: this.<fieldContext.PropertyName> = (<fieldContext.GenType.TypeName>)fieldValue; break;
>>


_toString(context) ::= <<
public override string ToString()
{
    var __sb = new StringBuilder("<context.TypeName>(");

    bool __first = true;
    <context.Fields : { field |<_toStringField(field)>}; separator="\n">

    __sb.Append(")");
    return __sb.ToString();
}
>>

_toStringField(field) ::= <<
if (<field.PropertyName> != null)
{
    if(!__first) { __sb.Append(", "); }
    __first = false;
    __sb.Append("<field.PropertyName>: ");
    <if(field.GenType.IsStruct)>
    __sb.Append(<field.PropertyName> == null ? "<null>" : <field.PropertyName>.ToString());
    <else>
    __sb.Append(<field.PropertyName>);
    <endif>
}
>>

_equals(context) ::= <<
public override bool Equals(object that)
{
    var other = that as <context.TypeName>;
    if (other == null) return false;
    if (ReferenceEquals(this, other)) return true;

    return 
        <context.Fields : { field |<_equalsField(field)>}; separator=" &&\n">;
}
>>

_equalsField(field) ::= <<
<if(field.GenType.IsContainer)>
ObjectUtils.AreEqual(<field.PropertyName>, other.<field.PropertyName>)
<elseif(field.GenType.IsBinary)>
(<field.PropertyName> == null ? other.<field.PropertyName> == null : <field.PropertyName>.SequenceEqual(other.<field.PropertyName>))
<elseif(field.GenType.IsDouble)>
(<field.PropertyName> != null && other.<field.PropertyName> != null ? (Math.Abs(<field.PropertyName>.Value - other.<field.PropertyName>.Value) \< 1E-15) : Equals(<field.PropertyName>, other.<field.PropertyName>))
<else>
Equals(<field.PropertyName>, other.<field.PropertyName>)
<endif>
>>

_hashCode(context) ::= <<
public override int GetHashCode()
{
    int result = 1;

    <context.Fields : { field |<_hashCodeField(field)>}>

    return result;
}
>>

_hashCodeField(field) ::= <<
<if(field.GenType.IsContainer)>
result = (result * 397) ^ (<field.PropertyName> == null ? 0 : ObjectUtils.GetHashCode(<field.PropertyName>));
<elseif(field.GenType.IsBinary)>
if (<field.PropertyName> != null)
{
    foreach (byte elem in <field.PropertyName>)
    {
        result = (result * 397) ^ elem;
    }
}
<else>
result = (result * 397) ^ (<field.PropertyName> == null ? 0 : <field.PropertyName>.GetHashCode());
<endif>
>>
