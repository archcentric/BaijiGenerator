service(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = service definition                                                             = !>
<! =                                                                                = !>
<! = called from the generator to render a new service java source file.            = !>
<! =                                                                                = !>
<! ================================================================================== !>

package <context.JavaPackage>;

import com.ctriposs.triprpc.common.TContract;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
<_docString(context.DocStringLines)>
<endif>

@TContract(serviceName = "<if(context.ServiceName)><context.ServiceName><endif>", serviceNamespace = "<if(context.ServiceNamespace)><context.ServiceNamespace><endif>", codeGeneratorVersion = "<global.CodeGenVersion>")
public interface <context.JavaName> extends com.ctriposs.baiji.specific.SpecificRecordBase <if(context.JavaParents)>, <context.JavaParents; separator=", "><endif> implements com.ctriposs.baiji.specific.SpecificRecord
{
    <context.Methods : { method |<_method(method)>}; separator="\n">
}
>>

struct(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = struct definition                                                              = !>
<! =                                                                                = !>
<! = called from the generator to render a new struct java source file.             = !>
<! =                                                                                = !>
<! ================================================================================== !>
package <context.JavaPackage>;

<if (context.IsServiceResponse)>
import com.ctriposs.triprpc.common.IHasResponseStatus;
<endif>
import java.util.*;
import com.ctriposs.baiji.*;
import com.ctriposs.baiji.specific.*;

import com.google.common.base.Objects;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
<_docString(context.DocStringLines)>
<endif>
public class <context.JavaName> extends SpecificRecordBase implements SpecificRecord<if (context.IsServiceResponse)>, IHasResponseStatus<endif>
{
    private static final long serialVersionUID = 1L;

    <context.SchemaField>

    public Schema getSchema() { return SCHEMA; }

    <_structbody(context)>

    <context.GetMethod>

    <context.PutMethod>

    <if(context.Fields)>
    <_equals(context)>

    <_hashCode(context)>

    <_toString(context)>
    <endif>
}<\n>
>>

intEnum(context, tweaks, global) ::= <<
<! ================================================================================== !>
<! =                                                                                = !>
<! = int based enum definition                                                      = !>
<! =                                                                                = !>
<! = called from the generator to render a new enum java source file.               = !>
<! =                                                                                = !>
<! ================================================================================== !>
package <context.JavaPackage>;

<if (tweaks.GEN_COMMENTS && context.DocStringLines)>
<_docString(context.DocStringLines)>
<endif>
public enum <context.JavaName>
{
    <context.Fields : { field|<_enumElement(field)>}; separator=",\n">;

    private final int value;

    <context.JavaName>(int value)
    {
        this.value = value;
    }

    <if (tweaks.GEN_COMMENTS)>
    /**
     * Get the integer value of this enum value, as defined in the Trip IDL.
     */
    <endif>
    public int getValue()
    {
        return value;
    }

    <if (tweaks.GEN_COMMENTS)>
    /**
     * Get the integer value of this enum value, as defined in the Trip IDL.
     */
    <endif>
    public static <context.JavaName> findByValue(int value) {
        switch (value) {
          <context.Fields : { field|<_enumFindByValueElement(field)>}; separator="\n">
          default:
            return null;
        }
    }
}<\n>
>>

_docString(lines) ::= <<
/**
 * <lines: {line|<line>}; separator="\n * ">
 */
>>

_enumFindByValueElement(field) ::= <<
case <field.Value>:
  return <field.JavaName>;
>>

_enumElement(field) ::= <<
<if (tweaks.GEN_COMMENTS && field.DocStringLines)>
<_docString(field.DocStringLines)>
<endif>
<field.JavaName><if(field.Value)>(<field.Value>)<endif>
>>

_method(method) ::= <<
<if (tweaks.GEN_COMMENTS && method.DocStringLines)>
<_docString(method.DocStringLines)>
<endif>
<method.JavaReturnType> <method.JavaMethodName>(<if(method.JavaArgumentType)><method.JavaArgumentType> <method.JavaArgumentName><endif>);
>>


_params(parameters) ::= <<
<if(parameters)>(
    <parameters: {param |<_param(param)>}; separator=",\n">
)<else>()<endif>
>>

_param(param) ::= <<
<param.GenType.JavaTypeName> <param.JavaName>
>>

_toString(context) ::= <<
@Override
public String toString()
{
    return Objects.toStringHelper(this)
        <context.Fields : { field |<_toStringField(field)>}; separator="\n">
        .toString();
}
>>

_toStringField(field) ::= <<
.add("<field.JavaName>", <field.JavaName>)
>>

_equals(context) ::= <<
@Override
public boolean equals(Object obj) {
    if (obj == null) return false;
    if (getClass() != obj.getClass()) return false;

    final <context.JavaName> other = (<context.JavaName>) obj;

    return 
        <context.Fields : { field |<_equalsField(field)>}; separator=" &&\n">;
}
>>

_equalsField(field) ::= <<
<if(field.GenType.IsBinary)>Arrays.equals(this.<field.JavaName>, other.<field.JavaName>)<else>Objects.equal(this.<field.JavaName>, other.<field.JavaName>)<endif>
>>

_hashCode(context) ::= <<
@Override
public int hashCode() {
    int result = 1;

    <context.Fields : { field |<_hashCodeField(field)>}; separator="\n">

    return result;
}
>>

_hashCodeField(field) ::= <<
result = 31 * result + (this.<field.JavaName> == null ? 0 : <if(field.GenType.IsBinary)>Arrays.hashCode(this.<field.JavaName>));<else>this.<field.JavaName>.hashCode());<endif>
>>
